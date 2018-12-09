using RxModellingLoop;
using RxModellingLoop.TimeScheduling;

namespace Sample.Events
{
    public class ProjectStarted : IRxModellingEvent
    {
        public ProjectStarted(ModelTime occured)
        {
            TimeOccured = occured;
        }

        public ModelTime TimeOccured { get; }
    }
}