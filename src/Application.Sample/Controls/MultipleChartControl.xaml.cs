using Restless.Controls.Chart;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for MultipleChartControl.xaml
    /// </summary>
    public partial class MultipleChartControl : ChartControlBase
    {
        public MultipleChartControl()
        {
            InitializeComponent();
            Line2.ChartStyle = LineChartStyle.Filled;
        }

        public override int DataSetCount => 1;

        private DataSeries data;

        public DataSeries Data
        {
            get => data;
            private set => SetProperty(ref data, value);
        }

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
    }
}
