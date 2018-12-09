using System;
using System.Threading;
using RxModellingLoop.TimeScheduling;

namespace RxModellingLoop
{
        public class EventLoop : IDisposable
        {
            internal EventLoop()
            {
                
            }
            public ModelTime Time { get; internal set; }
            public IObservable<IRxModellingEvent> EventBroker { get; internal set; }
            internal CancellationTokenSource CancellationToken { get; set; }

            public void Dispose()
            {
                CancellationToken.Cancel();
            }
        }
}
