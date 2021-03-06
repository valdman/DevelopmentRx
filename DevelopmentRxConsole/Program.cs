﻿using System;
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
    internal static class Program
    {
        private static Task Run(IComponentContext c)
        {
            #region DI_Load
            var broker = c.Resolve<EventBroker>();
            var begin = DateTimeOffset.Parse("2019-01-01");
            var timeScheduler = new ModelTimeScheduler(ModelTimeStep.Day, begin);
                
            var manager = c.Resolve<Manager>(new NamedParameter("timeScheduler", timeScheduler));
            var dev1 = c.Resolve<Developer>(
                new NamedParameter("timeScheduler", timeScheduler), 
                new NamedParameter("name", "Борис"));
            var dev2 = c.Resolve<Developer>(
                new NamedParameter("timeScheduler", timeScheduler), 
                new NamedParameter("name", "Дмитрий"));
            var dev3 = c.Resolve<Developer>(
                new NamedParameter("timeScheduler", timeScheduler), 
                new NamedParameter("name", "Илья"));
            #endregion

            var loop = new EventLoop(timeScheduler, broker);

            var a = ModelTimeSpan.FromModelTimeUnits(3);

            timeScheduler.Schedule(ModelTimeSpan.FromModelTimeUnits(3), () =>
            {
                Console.WriteLine("Случается на 3 шаге модельного времени"); 
            });
            return loop.Run(time =>
            {
                time.OnUniformTestPassed(t =>
                {
                    manager.GiveNewTaskToTeam("'Важная задача'");
                }, density: 0.5);
                    
            }, 15);
        }

#region Init
        private static void Main()
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();
            var options = new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromMilliseconds(10)
            };

            var server = new BackgroundJobServer(options);
            
            var cb = new ContainerBuilder();
            cb.RegisterType<EventBroker>().SingleInstance();
            cb.RegisterType<ModelTimeScheduler>().InstancePerDependency();
            cb.Register<Manager>((c, p) =>
                new Manager(
                    p.Named<ModelTimeScheduler>("timeScheduler"), c.Resolve<EventBroker>()
                )).SingleInstance();
            cb.Register((c, p) =>
                new Developer(
                    p.Named<ModelTimeScheduler>("timeScheduler"), c.Resolve<EventBroker>(), p.Named<string>("name")
            ));

            using (var c = cb.Build())
            {
                Run(c).Wait();
            }
            server.Dispose();
        }
    }
#endregion
}
