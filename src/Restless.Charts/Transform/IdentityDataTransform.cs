namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides identity transformation between data and plot coordinates.
    /// </summary>
    public class IdentityDataTransform : DataTransform
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityDataTransform"/> class.
        /// </summary>
        public IdentityDataTransform()
        {
        }

        /// <summary>
        /// Returns a value in data coordinates without conversion.
        /// </summary>
        /// <param name="dataValue">A value in data coordinates.</param>
        /// <returns>The value of <paramref name="dataValue"/>. No change.</returns>
        public override double DataToPlot(double dataValue)
        {
            return dataValue;
        }

        /// <summary>
        /// Returns a value in plot coordinates without conversion.
        /// </summary>
        /// <param name="plotValue">A value in plot coordinates.</param>
        /// <returns>The value of <paramref name="plotValue"/>. No change.</returns>
        public override double PlotToData(double plotValue)
        {
            return plotValue;
        }
    }
}
