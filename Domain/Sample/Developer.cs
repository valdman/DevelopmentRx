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
            Console.WriteLine($"{Name} начал делать {taskToDo} в {TimeScheduler.CurrentTime}");
            var abort = TimeScheduler.Schedule(ModelTimeSpan.FromModelTimeUnits(3), 
                () => { Console.WriteLine($"{Name} завершил работу над {taskToDo}"); });
            
            AbortCurrentJob = TimeScheduler.Schedule(ModelTimeSpan.FromModelTimeUnits(5),
                () =>
                {
                    Console.WriteLine($"Остановлена работа над {taskToDo} в {TimeScheduler.CurrentTime}");
                });
        }
        
        public string Name { get; } 

        private Action AbortCurrentJob { get; set; } = () => { };
    }
}