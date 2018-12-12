using System;
using System.Reflection;
using RxModellingLoop.TimeScheduling;
using Xunit;

namespace RxModellingLoopTests
{
    public class ModelTimeSpanTests
    {
        [Theory]
        [InlineData(ModelTimeStep.Day, ModelTimeStep.Day, 1)]
        [InlineData(ModelTimeStep.Day, ModelTimeStep.Year, 360)]
        [InlineData(ModelTimeStep.Day, ModelTimeStep.Month, 30)]
        [InlineData(ModelTimeStep.Second, ModelTimeStep.Day, 60 * 60 * 24)]
        public void TimeSpansFromDateOperateCorrect(ModelTimeStep modelStep, ModelTimeStep spanStep, int res)
        {
            var now = DateTimeOffset.Parse("2019-01-01");
            var scheduler = new ModelTimeScheduler(modelStep, now);
            
            var transform = scheduler.GetType().GetMethod("FromModelTimeUnits", BindingFlags.NonPublic | BindingFlags.Instance);
            var multiplier = ModelTimeScheduler.GetTimeSpanMultiplier(modelStep, spanStep);
            Assert.Equal(multiplier, res);
        }
    }
}