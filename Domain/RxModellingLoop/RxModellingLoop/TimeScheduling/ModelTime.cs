using System;
using System.Collections.Generic;

namespace RxModellingLoop.TimeScheduling
{
    public struct ModelTime
    {
        public static ModelTime operator+ (ModelTime left, ModelTime right)
        {
            throw new NotImplementedException();
            //return new ModelTime(left.Ticks + right.Ticks);
        }
            
        public static ModelTime operator- (ModelTime left, ModelTime right)
        {    
            throw new NotImplementedException();
            //return new ModelTime(left.Ticks - right.Ticks);
        }
         
        public override string ToString()
        {
            return Ticks.ToString();
        }

        public DateTimeOffset AsOffset()
        {
            Func<int, TimeSpan> span;
            switch (Step)
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
                    span = (t) => TimeSpan.FromDays(365 * t);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _begin + span(Ticks);
        }
            
        public ModelTime(int ticks, ModelTimeStep step, DateTimeOffset begin)
        {
            _begin = begin;
            Ticks = ticks;
            Step = step;
        }

        public int Ticks { get; internal set; }
        internal ModelTimeStep Step { get; }
        private readonly DateTimeOffset _begin;
        
        private sealed class TimeRelationalComparer : IComparer<ModelTime>
        {
            public int Compare(ModelTime x, ModelTime y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return x.Ticks.CompareTo(y.Ticks);
            }
        }
        public static IComparer<ModelTime> TimeComparer { get; } = new TimeRelationalComparer();
    }
}