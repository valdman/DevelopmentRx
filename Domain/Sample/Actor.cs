using System;
using RxModellingLoop;
using RxModellingLoop.TimeScheduling;

namespace Sample
{
    public class Actor
    {
        protected readonly ModelTimeScheduler TimeScheduler;
        protected readonly EventBroker Broker;

        public Actor(ModelTimeScheduler timeScheduler, EventBroker broker)
        {
            TimeScheduler = timeScheduler;
            this.Broker = broker ?? throw new ArgumentNullException(paramName: nameof(broker));
        } 
    }
}
