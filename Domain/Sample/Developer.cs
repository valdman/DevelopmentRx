using System;
using System.Reactive.Linq;
using RxModellingLoop;
using RxModellingLoop.TimeScheduling;
using Sample.Events;

namespace Sample
{
    public class Developer : Actor
    {
        public Developer(ModelTimeScheduler timeScheduler, EventBroker broker, string name) : base(timeScheduler, broker)
        {
            Name = name;
            broker.OfType<NewDemandingsPublished>().Subscribe(demand =>
            {
                AbortCurrentJob();
                DevelopTask(demand.Description);
            });
        }

        public void DevelopTask(string taskToDo)
        {
            Console.WriteLine($"Start {taskToDo} on {TimeScheduler.CurrentTime} by ${Name}");
            AbortCurrentJob = TimeScheduler.Schedule(ModelTimeSpan.FromModelTimeUnits(5),
                () =>
                {
                    Console.WriteLine($"Finish {taskToDo} on {TimeScheduler.CurrentTime} by ${Name}");
                });
        }
        
        public string Name { get; }

        private Action AbortCurrentJob { get; set; } = () => { };
    }
}