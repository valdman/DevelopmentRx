using System;
using System.Reactive.Linq;
using RxModellingLoop.TimeScheduling;
using Sample.Events;

namespace Sample
{
    public class Manager : Actor
    {
        public Manager(ModelTimeScheduler timeScheduler, EventBroker broker) : base(timeScheduler, broker)
        {
        }

        public void GiveNewTaskToTeam(string task)
        {
            Console.WriteLine($"Publish {task} on {TimeScheduler.CurrentTime.AsOffset():dd-MM}");
            Broker.Publish(new NewDemandingsPublished(task));
        }
    }
}