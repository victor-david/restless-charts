namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides an enumeration that describes the result of tick evaluation.
    /// </summary>
    public enum TickEvaluationResult
    {
        /// <summary>
        /// Tick evaluation is okay.
        /// </summary>
        Ok,
        /// <summary>
        /// Evaluation result indicates the number of ticks needs to be increased.
        /// </summary>
        Increase,
        /// <summary>
        /// Evaluation result indicates the number of ticks needs to be decreased.
        /// </summary>
        Decrease
    }
}
