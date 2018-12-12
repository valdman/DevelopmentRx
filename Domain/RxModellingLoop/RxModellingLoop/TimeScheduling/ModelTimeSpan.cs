namespace RxModellingLoop.TimeScheduling
{
    public class ModelTimeSpan
    {
        public static ModelTimeSpan FromModelTimeUnits(int steps)
        {
            return new ModelTimeSpan
            {
                Steps = steps, StepType = ModelTimeStep.Undefined
            };
        }
        
        //TODO: Add 'From' Methods for non-model intervals ('FromDays, FromMonth...')
        
        private ModelTimeSpan(){}

        internal ModelTimeStep StepType { get; private set; }
        internal int Steps { get; private set; }
    }
}