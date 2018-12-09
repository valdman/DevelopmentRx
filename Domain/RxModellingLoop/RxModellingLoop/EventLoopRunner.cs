using System;
using System.Threading;
using RxModellingLoop.TimeScheduling;

namespace RxModellingLoop
{
    public class EventLoopRunner
    {
        private readonly DateTimeOffset _begin;
        private ModelTimeStep Step { get; }

        public EventLoopRunner(ModelTimeStep step, DateTimeOffset begin)
        {
            _begin = begin;
            Step = step;
        }

        public EventLoop RunEventLoop(IObservable<IRxModellingEvent> broker, Action<ModelTime> onLoopStep, int stop = -1)
        {
            var timePassed = new ModelTime(0, Step, _begin);
            var cts = new CancellationTokenSource();

            var loop = new EventLoop
            {
                CancellationToken = cts,
                Time = timePassed,
                EventBroker = broker
            };
            
            ThreadPool.QueueUserWorkItem(obj =>
            {
                var cancellationToken = (CancellationTokenSource) obj;
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (stop > 0 && stop <= timePassed.Ticks)
                    {
                        loop.Dispose();
                    }
                    timePassed.Ticks++;
                    onLoopStep(timePassed);
                }
            }, cts);

            return loop;
        }
    }
}