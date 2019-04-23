using Restless.Controls.Chart;
using System.Windows.Media;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for BarChartControl.xaml
    /// </summary>
    public partial class BarChartControl : ChartControlBase
    {
        #region Private
        private DataSeries data;
        #endregion

        #region Constructor
        public BarChartControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the count of data sets that this control supports.
        /// </summary>
        public override int DataSetCount => 2;

        /// <summary>
        /// Gets the data.
        /// </summary>
        public DataSeries Data
        {
            get => data;
            private set => SetProperty(ref data, value);
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Creates chart data using the specified data set.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        protected override void OnCreateChartData(int dataSet)
        {
            switch (dataSet)
            {
                case 1:
                    CreateChartData1();
                    break;
                case 2:
                    CreateChartData2();
                    break;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Creates data - single series.
        /// </summary>
        private void CreateChartData1()
        {
            TopTitle.Text = "Bar Chart Single Data Series";

            int maxX = 14;
            int minY = 100;
            int maxY = 5000;

            DataSeries data = DataSeries.Create();
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            data.DataInfo.SetInfo(0, "Balance", Brushes.Red, Brushes.WhiteSmoke, Brushes.DarkGray);

            for (int x = 1; x <= maxX; x++)
            {
                int y = generator.GetValue();
                data.Add(x, y);
            }

            data.ExpandX(1.0);
            data.DataRange.Y.Include(maxY);
            data.MakeYAutoZero();

            Data = data;
        }

        private void CreateChartData2()
        {
            TopTitle.Text = "Bar Chart Multiple Data Series";

            int maxX = 20;
            int minY = 1000;
            int maxY = 25000;

            DataSeries data = DataSeries.Create(3);
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            data.DataInfo.SetInfo(0, "Balance", Brushes.SteelBlue, Brushes.WhiteSmoke, Brushes.Blue);
            data.DataInfo.SetInfo(1, "Transactions", Brushes.Firebrick);
            data.DataInfo.SetInfo(2, "Callbacks", Brushes.Indigo);

            for (int x = 0; x < maxX; x++)
            {
                int y = generator.GetValue();
                data.Add(x, y);

                y = generator.GetValue();
                data.Add(x, y);

                y = generator.GetValue();
                data.Add(x, y);
            }

            data.ExpandX(1.0);
            data.DataRange.Y.Include(maxY);
            data.MakeYAutoZero();

            Data = data;
        }
        #endregion

    }
}
