using System;
using Xunit;

namespace RxModellingLoopTests
{
    public static class AssertPlus
    {
        public static void EqualFloats(double expected, double actual)
        {
            Assert.True(Math.Abs(expected - actual) < 1e-2);
        }
    }
}