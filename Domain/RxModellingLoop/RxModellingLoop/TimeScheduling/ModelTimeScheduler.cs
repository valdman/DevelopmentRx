using System;
using System.Collections.Generic;
using System.Linq;

namespace RxModellingLoop.TimeScheduling
{
    public class ModelTimeScheduler
    {
        public ModelTimeScheduler(ModelTimeStep step, DateTimeOffset beginning)
        {
            CurrentTime = new ModelTime(0, step, beginning);
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

        private void TryCancelAction(ModelTime actionTime, Action cancellation)
        {
            if (actionTime > CurrentTime)
            {
                Console.WriteLine($"Abort from {actionTime.AsOffset():dd-MM} on {CurrentTime.AsOffset():dd-MM}");
                cancellation();
            }
        }
        
        public ModelTime CurrentTime { get; private set; }
        private Dictionary<ModelTime, List<Action>> _schedule = new Dictionary<ModelTime, List<Action>>();
    }
}