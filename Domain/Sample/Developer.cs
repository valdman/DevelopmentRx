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
            Console.WriteLine($"Start {taskToDo} on {TimeScheduler.CurrentTime.AsOffset():dd-MM} by ${Name}");
            AbortCurrentJob = TimeScheduler.Schedule(new ModelTime(TimeScheduler.CurrentTime.Ticks + 5, ModelTimeStep.Day, TimeScheduler.CurrentTime.Begin),
                () =>
                {
                    Console.WriteLine($"Finish {taskToDo} on {TimeScheduler.CurrentTime.AsOffset():dd-MM} by ${Name}");
                });
        }
        
        public string Name { get; }

        private Action AbortCurrentJob { get; set; } = () => { };
    }
}