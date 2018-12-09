using System;
using RxModellingLoop;
using RxModellingLoop.TimeScheduling;

namespace Sample
{
    public class Actor
    {
        protected EventBroker broker { get; }

        public Actor(EventBroker broker)
        {
            this.broker = broker ?? throw new ArgumentNullException(paramName: nameof(broker));
        } 
    }
}
