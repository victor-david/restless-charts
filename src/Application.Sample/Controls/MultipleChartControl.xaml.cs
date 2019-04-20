using Restless.Controls.Chart;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for MultipleChartControl.xaml
    /// </summary>
    public partial class MultipleChartControl : ChartControlBase
    {
        #region Constructor
        public MultipleChartControl()
        {
            InitializeComponent();
            Line2.ChartStyle = LineChartStyle.Filled;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the count of data sets that this control supports.
        /// </summary>
        public override int DataSetCount => 1;
        #endregion

        #region Protected methods
        /// <summary>
        /// Creates chart data using the specified data set.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        protected override void OnCreateChartData(int dataSet)
        {
            Bar1.CreateChartData(dataSet);
            Line1.CreateChartData(dataSet);
            Line2.CreateChartData(Line2.DataSetCount-1);
            Pie1.CreateChartData(Pie1.DataSetCount);
        }
        #endregion
    }
}
