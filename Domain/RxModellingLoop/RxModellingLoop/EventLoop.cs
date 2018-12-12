using System;
using System.Threading;
using System.Threading.Tasks;
using RxModellingLoop.TimeScheduling;

namespace RxModellingLoop
{
        public class EventLoop : IDisposable
        {
            public EventLoop(ModelTimeStep step, DateTimeOffset beginning, IObservable<IRxModellingEvent> broker)
                : this(new ModelTimeScheduler(step, beginning), broker)
            {
            }

            public EventLoop(ModelTimeScheduler scheduler, IObservable<IRxModellingEvent> broker)
            {
                var cts = new CancellationTokenSource();

                CancellationTokenSource = cts;
                TimeScheduler = scheduler;
                EventBroker = broker;
            }

            //TODO: Enable stop on ModelTimeSpan, not just 'tick number'
            public Task Run(Action<ModelTime> onLoopStep, int stop = -1)
            {
                var ct = CancellationTokenSource.Token;
                
                var simulationTask = new Task(() =>
                {
                    var aborted = (object)false;
                    var trueBoxed = (object) true;
                    ct.Register(() =>
                    {
                        Interlocked.Exchange(ref aborted, trueBoxed);
                    });
                    while (!(bool)aborted && stop > 0 && stop >= TimeScheduler.CurrentTime.Ticks)
                    {
                        TimeScheduler.SetTime(new ModelTime(
                            TimeScheduler.CurrentTime.Ticks + 1,
                            TimeScheduler.TimeStep,
                            TimeScheduler.CurrentTime.Begin)
                        );
                        onLoopStep(TimeScheduler.CurrentTime);
                    }

                    Dispose();
                });
            
                simulationTask.Start();

                return simulationTask;
            }

            private ModelTimeScheduler TimeScheduler { get; }
            private IObservable<IRxModellingEvent> EventBroker { get; }
            private CancellationTokenSource CancellationTokenSource { get; }

            public void Dispose()
            {
                CancellationTokenSource.Cancel();
                Thread.Sleep(50);
            }
        }
}
