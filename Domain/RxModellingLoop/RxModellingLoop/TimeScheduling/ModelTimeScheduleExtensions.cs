using System;
using MathNet.Numerics.Distributions;

namespace RxModellingLoop.TimeScheduling
{
    public static class ModelTimeScheduleExtensions
    {
        private static readonly ContinuousUniform ContinuousUniform = new ContinuousUniform(0, 1);
        private static readonly Normal Normal = new Normal(0.5, 1);
        
        
        public static void StrictlyEachTick(this ModelTime now, Action<ModelTime> action, int tickNumber = 0)
        {
            if(now.Ticks % tickNumber != 0) return;
            action(now);
        }

        public static void OnUniformTestPassed(this ModelTime now, Action<ModelTime> action, double probability)
        {
            var quantile = 1 - probability;
            var test = new Func<bool>(() => 
                ContinuousUniform.InvCDF(0, 1, ContinuousUniform.Sample()) >= quantile);
            OnContinuousDistributionTest(now, test , action, probability);
        }
        
        public static void OnNormalTestPassed(this ModelTime now, Action<ModelTime> action, double probability)
        {
            var quantile = 1 - probability;
            var test = new Func<bool>(() => Normal.InvCDF(0.5, 1, Normal.Sample()) >= quantile);
            OnContinuousDistributionTest(now, test, action, probability);
        }

        private static void OnContinuousDistributionTest(ModelTime now, Func<bool> test, Action<ModelTime> action, double probability)
        {
            if (!test()) return;
            action(now);
        }
    }
}