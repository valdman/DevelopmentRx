using System;
using System.Collections.Generic;
using System.Linq;

namespace RxModellingLoop.TimeScheduling
{
    public class ModelTimeScheduler
    {
        public Action Schedule(ModelTime onTime, Action action)
        {
            if (_schedule.TryGetValue(onTime, out var actions))
            {
                actions.Add(action);
            }
            else
            {
                actions = new List<Action> {action};
                _schedule.Add(onTime, actions);
            }
            
            
            return () => TryCancelAction(onTime, () => 
                actions.Remove(action)
            );
        }
        
        public Action Schedule(ModelTimeSpan afterSpan, Action action)
        {
            var multiplier = GetTimeSpanMultiplier(modelTimeStep: TimeStep, spanStep: afterSpan.StepType);
            var occurOnTime = new ModelTime(afterSpan.Steps * multiplier, TimeStep, CurrentTime.Begin);

            return Schedule(occurOnTime, action);
        }
        
        public static int GetTimeSpanMultiplier(ModelTimeStep modelTimeStep, ModelTimeStep spanStep)
        {
            if (modelTimeStep == spanStep || spanStep == ModelTimeStep.Undefined)
            {
                return 1;
            }
            if ((int)spanStep < (int) modelTimeStep)
            {
                throw new ArgumentException("Can't time afterSpan that smaller than model time");
            }
            
            var degrade = new Func<ModelTimeStep, ModelTimeStep>(a => (ModelTimeStep) ((int) a - 1));
            switch (spanStep)
            {
                case ModelTimeStep.Millisecond:
                    return 1000 * GetTimeSpanMultiplier(modelTimeStep, degrade(spanStep));
                case ModelTimeStep.Second:
                    return 60 * GetTimeSpanMultiplier(modelTimeStep, degrade(spanStep));
                case ModelTimeStep.Minute:
                    return 60 * GetTimeSpanMultiplier(modelTimeStep, degrade(spanStep));
                case ModelTimeStep.Hour:
                    return 60 * GetTimeSpanMultiplier(modelTimeStep, degrade(spanStep));
                case ModelTimeStep.Day:
                    return 24 * GetTimeSpanMultiplier(modelTimeStep, degrade(spanStep));
                case ModelTimeStep.Month:
                    return 30 * GetTimeSpanMultiplier(modelTimeStep, degrade(spanStep));
                case ModelTimeStep.Year:
                    return 12 * GetTimeSpanMultiplier(modelTimeStep, degrade(spanStep));
                default:
                    throw new ArgumentOutOfRangeException(nameof(modelTimeStep), modelTimeStep, null);
            }
        }
        
        internal void SetTime(ModelTime newTime)
        {
            var selection = new Func<KeyValuePair<ModelTime, List<Action>>, bool>(pair => pair.Key.TickEqual(newTime));
            var actionGroups = _schedule.Where(selection).ToList();
            for (var i = 0; i < actionGroups.Count; i++)
            {
                var actionGroup = actionGroups[i];
                actionGroup.Value.ForEach(a => a());
            }

            _schedule = _schedule.Except(actionGroups).ToDictionary(p => p.Key, p => p.Value);

            CurrentTime = newTime;
        }

        //TODO: onCancellationFailed ?
        private void TryCancelAction(ModelTime actionTime, Action onCancellationSuccess)
        {
            if (actionTime > CurrentTime)
            {
                Console.WriteLine($"Abort modelTimeStep {actionTime} on {CurrentTime}");
                onCancellationSuccess();
            }
        }
          
        public ModelTimeScheduler(ModelTimeStep step, DateTimeOffset beginning)
        {
            CurrentTime = new ModelTime(0, step, beginning);
            TimeStep = step;
        }
        
        public ModelTime CurrentTime { get; private set; }
        internal ModelTimeStep TimeStep { get; }
        private Dictionary<ModelTime, List<Action>> _schedule = new Dictionary<ModelTime, List<Action>>();
    }
}