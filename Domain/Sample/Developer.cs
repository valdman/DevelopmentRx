using System;
using System.Reactive.Linq;
using Hangfire;
using RxModellingLoop;
using RxModellingLoop.TimeScheduling;
using Sample.Events;

namespace Sample
{
    public class Developer : Actor
    {
        public Developer(EventBroker broker, string name) : base(broker)
        {
            Name = name;
            broker.OfType<NewDemandingsPublished>().Subscribe(demand =>
            {
                AbortCurrentJob();
                DevelopTask(demand.TimeOccured, demand.Description);
            });
        }

        public void DevelopTask(ModelTime time, string taskToDo)
        {
            Console.WriteLine($"Start doing {taskToDo}");
        }
        
        public string Name { get; }

        private Action AbortCurrentJob { get; set; } = () => { };
    }
}