using RxModellingLoop.TimeScheduling;

namespace RxModellingLoop
{
    public interface IRxModellingEvent
    {
        ModelTime TimeOccured { get; }
    }
}