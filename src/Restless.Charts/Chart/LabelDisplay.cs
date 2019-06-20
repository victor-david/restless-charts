namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides an enumeration that describes how labels are placed on a <see cref="PieChart"/>.
    /// </summary>
    public enum LabelDisplay
    {
        /// <summary>
        /// No labels will be applied.
        /// </summary>
        None,
        /// <summary>
        /// A label with the name of the series.
        /// </summary>
        Name,
        /// <summary>
        /// A label with the name of the series and the series value.
        /// </summary>
        NameValue,
        /// <summary>
        /// A label with the name of the series and the percentage of the total that the value represents.
        /// </summary>
        NamePercentage
    }
}
