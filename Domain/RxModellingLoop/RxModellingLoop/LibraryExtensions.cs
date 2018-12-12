using System;
using RxModellingLoop.TimeScheduling;

namespace RxModellingLoop
{
    public static class LibraryExtensions
    {
        public static DateTimeOffset AsOffset(this ModelTime time, ModelTimeStep step)
        {
            Func<int, TimeSpan> span;
            switch (step)
            {
                case ModelTimeStep.Millisecond:
                    span = (t) => TimeSpan.FromMilliseconds(t);
                    break;
                case ModelTimeStep.Second:
                    span = (t) => TimeSpan.FromSeconds(t);
                    break;
                case ModelTimeStep.Minute:
                    span = (t) => TimeSpan.FromMinutes(t);
                    break;
                case ModelTimeStep.Hour:
                    span = (t) => TimeSpan.FromHours(t);
                    break;
                case ModelTimeStep.Day:
                    span = (t) => TimeSpan.FromDays(t);
                    break;
                case ModelTimeStep.Month:
                    span = (t) => TimeSpan.FromDays(t * 30);
                    break;
                case ModelTimeStep.Year:
                    span = (t) => TimeSpan.FromDays(360 * t);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return time.Begin + span(time.Ticks);
        }
    }
}