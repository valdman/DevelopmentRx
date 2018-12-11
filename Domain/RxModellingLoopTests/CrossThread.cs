using System;
using System.Threading;
using Moq;
using RxModellingLoop;
using RxModellingLoop.TimeScheduling;
using Xunit;

namespace RxModellingLoopTests
{
    public class CrossThread
    {
        //[Fact]
        public void DisposingLetNoEventsBeDispatched()
        {
            var brokerMock = new Mock<IObservable<IRxModellingEvent>>();
            var timeScheduler = new ModelTimeScheduler(ModelTimeStep.Day, DateTimeOffset.Parse("2019-01-01"));

            var loop = new EventLoop(timeScheduler, brokerMock.Object);

            loop.Run(time =>
            {
                time.OnUniformTestPassed(t =>
                {
                    
                }, 0.2);
                    
            }, 15);
            
            Thread.Sleep(80);
            
            loop.Dispose();
            Console.WriteLine("loop disposed");
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }
    }
}