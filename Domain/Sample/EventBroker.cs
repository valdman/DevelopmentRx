using System;
using System.Reactive.Subjects;
using RxModellingLoop;
using Sample.Events;

namespace Sample
{
    public class EventBroker : IObservable<IRxModellingEvent>
    {
        private Subject<IRxModellingEvent> subscribitions = new Subject<IRxModellingEvent>();
        
        public IDisposable Subscribe(IObserver<IRxModellingEvent> observer)
        {
            return subscribitions.Subscribe(observer);
        }

        public void Publish(IRxModellingEvent @event)
        {
            subscribitions.OnNext(@event);
        }
    }
}