using System;
using System.Threading.Tasks;
using Moq;
using RxModellingLoop;
using RxModellingLoop.TimeScheduling;
using Xunit;
using Xunit.Abstractions;

namespace RxModellingLoopTests
{
    public class EventStreams
    {
        private const int LongSimulationTimeInTicks = 1000000;

        [Theory]
        [InlineData(0.123)]
        [InlineData(0.5)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task UniformTestProvidesFairEventDensity(double eventStreamDensity)
        {
            var brokerMock = new Mock<IObservable<IRxModellingEvent>>();
            var timeScheduler = new ModelTimeScheduler(ModelTimeStep.Day, DateTimeOffset.Parse("2019-01-01"));
            var triggeredTimes = 0;
            
            var loop = new EventLoop(timeScheduler, brokerMock.Object);
            await loop.Run(time =>
            {
                time.OnUniformTestPassed(t =>
                {
                   triggeredTimes++;
                }, eventStreamDensity);
                    
            }, LongSimulationTimeInTicks);
            var actualDensity = (double) triggeredTimes / LongSimulationTimeInTicks;
            
            _output.WriteLine($"exp: {eventStreamDensity} vs act: {actualDensity}");
            AssertPlus.EqualFloats(
                expected: eventStreamDensity,
                actual: (double)triggeredTimes/LongSimulationTimeInTicks
            );
        }
        
        //[Fact]
        public async Task NormalTestProvidesFairEventDensity()
        {
            
            var brokerMock = new Mock<IObservable<IRxModellingEvent>>();
            var timeScheduler = new ModelTimeScheduler(ModelTimeStep.Day, DateTimeOffset.Parse("2019-01-01"));
            var triggeredTimes = 0;
            var eventStreamDensity = 0.123;
            
            var loop = new EventLoop(timeScheduler, brokerMock.Object);
            await loop.Run(time =>
            {
                time.OnNormalTestPassed(t =>
                {
                    triggeredTimes++;
                }, eventStreamDensity);
                    
            }, LongSimulationTimeInTicks);
            var actualDensity = (double) triggeredTimes / LongSimulationTimeInTicks;
            
            _output.WriteLine($"exp: {eventStreamDensity} vs act: {actualDensity}");
            AssertPlus.EqualFloats(
                expected: eventStreamDensity,
                actual: (double)triggeredTimes/LongSimulationTimeInTicks
            );
        }
        
        public EventStreams(ITestOutputHelper output)
        {
            _output = output;
        }
        private readonly ITestOutputHelper _output;
    }
}