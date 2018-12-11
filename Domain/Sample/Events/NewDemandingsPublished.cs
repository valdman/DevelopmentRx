using RxModellingLoop;
using RxModellingLoop.TimeScheduling;

namespace Sample.Events
{
    public class NewDemandingsPublished : IRxModellingEvent
    {
        public NewDemandingsPublished(string description)
        {
            Description = description;
        }
        
        public string Description { get; }
    }
}