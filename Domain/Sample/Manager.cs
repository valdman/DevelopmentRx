using System;
using System.Reactive.Linq;
using RxModellingLoop.TimeScheduling;
using Sample.Events;

namespace Sample
{
    public class Manager : Actor
    {
        public Manager(EventBroker broker) : base(broker)
        {
        }

        public void GiveNewTaskTo(ModelTime time, string task)
        {
            Console.WriteLine($"Publishing {task}");
            broker.Publish(new NewDemandingsPublished(time, task));
        }
    }
}