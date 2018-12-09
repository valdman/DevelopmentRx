using RxModellingLoop;
using RxModellingLoop.TimeScheduling;

namespace Sample.Events
{
    public class NewDemandingsPublished : IRxModellingEvent
    {
        public NewDemandingsPublished(ModelTime time, string description)
        {
            TimeOccured = time;
            Description = description;
        }
        
        public string Description { get; }
        public ModelTime TimeOccured { get; }
    }
}