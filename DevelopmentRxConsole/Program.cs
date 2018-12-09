using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Hangfire;
using Hangfire.MemoryStorage;
using RxModellingLoop;
using RxModellingLoop.TimeScheduling;
using Sample;

namespace DevelopmentRxConsole
{
    class Program
    {
        static void Run(IComponentContext c)
        {
            var broker = c.Resolve<EventBroker>();
                
            var manager = c.Resolve<Manager>();
            var dev1 = c.Resolve<Developer>(new NamedParameter("name", "Boris"));
            var dev2 = c.Resolve<Developer>(new NamedParameter("name", "Mosh"));
            var dev3 = c.Resolve<Developer>(new NamedParameter("name", "Sus"));

            var loopRunner = new EventLoopRunner(ModelTimeStep.Day, DateTimeOffset.Now);
            var loop1 = loopRunner.RunEventLoop(broker, time =>
            {
                time.OnUniformTestPassed(t =>
                {
                    manager.GiveNewTaskTo(t, $"l1 t1 at {t.AsOffset():dd-MM}");
                }, 0.1);
                time.OnUniformTestPassed(t =>
                {
                    manager.GiveNewTaskTo(t, $"l1 t2 at {t.AsOffset():dd-MM}");
                }, 0.5);
                time.OnUniformTestPassed(t =>
                {
                    manager.GiveNewTaskTo(t, $"l1 t3 at {t.AsOffset():dd-MM}");
                }, 0.2);
            }, 10);
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
            loop1.Dispose();
        }
        
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();
            var options = new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromMilliseconds(10)
            };

            var server = new BackgroundJobServer(options);
            
            var cb = new ContainerBuilder();
            cb.RegisterType<EventBroker>().InstancePerLifetimeScope();
            cb.RegisterType<Manager>().SingleInstance();
            cb.Register((c, p) =>
                new Developer(
                    c.Resolve<EventBroker>(), p.Named<string>("name")
            ));

            using (var c = cb.Build())
            {
                Run(c);
            }
            server.Dispose();
        }
    }
}
