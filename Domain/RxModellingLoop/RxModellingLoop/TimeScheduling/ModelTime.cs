using System;
using System.Collections.Generic;

namespace RxModellingLoop.TimeScheduling
{
    public struct ModelTime
    {
        public static bool operator >(ModelTime left, ModelTime right)
        {
            return new TimeRelationalComparer().Compare(left, right) == 1;
        }
        
        public static bool operator <(ModelTime left, ModelTime right)
        {
            return new TimeRelationalComparer().Compare(left, right) == -1;
        }
        
        public override string ToString()
        {
            return Ticks.ToString();
        }

        public bool TickEqual(ModelTime right)
        {
            return Ticks == right.Ticks;
        }
            
        public ModelTime(int ticks, ModelTimeStep step, DateTimeOffset begin)
        {
            if (step == ModelTimeStep.Undefined)
            {
                throw new ArgumentException("Model time step couldn't be Undefined. (This exception can be triggered by using 0 as ModelTimeStep)");
            }
            Begin = begin;
            Ticks = ticks;
        }

        internal int Ticks { get; }
        internal DateTimeOffset Begin { get; }
        
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
    }
}