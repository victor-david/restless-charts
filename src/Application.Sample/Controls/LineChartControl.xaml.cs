using Restless.Controls.Chart;
using System;
using System.Windows.Media;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for LineChartControl.xaml
    /// </summary>
    public partial class LineChartControl : ChartControlBase
    {
        #region Private
        private DataSeries data;
        private LineChartStyle chartStyle;
        #endregion

        #region Constructor
        public LineChartControl()
        {
            InitializeComponent();
            ChartStyle = LineChartStyle.Standard;
        }
        #endregion

        #region Properties

        public override int DataSetCount => 3;
        

        public DataSeries Data
        {
            get => data;
            private set => SetProperty(ref data, value);
        }

        /// <summary>
        /// Gets or sets the line chart style.
        /// </summary>
        public LineChartStyle ChartStyle
        {
            get => chartStyle;
            set => SetProperty(ref chartStyle, value);
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
                case 3:
                    CreateChartData3();
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
            LastDataSet = 1;
            TopTitle.Text = "Single Series Line Chart";
            int maxX = 14;
            int minY = 100;
            int maxY = 5000;

            DataSeries data = DataSeries.Create();
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            data.DataInfo.SetInfo(0, "Balance", Brushes.Red);
            data.PrimaryTextBrushes.SetBrush(0, Brushes.WhiteSmoke);
            data.SecondaryTextBrushes.SetBrush(0, Brushes.DarkRed);

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

        /// <summary>
        /// Creates data - multiple series.
        /// </summary>
        private void CreateChartData2()
        {
            LastDataSet = 2;
            TopTitle.Text = "Multiple Series Line Chart (Filled)";

            //XAxisTextFormat = null;
            //XAxisTextProvider = null;
            //SetTopTitle("Data Set #3");

            int maxX = 20;
            int minY = 1000;
            int maxY = 25000;

            DataSeries data = DataSeries.Create(3);
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            data.DataInfo.SetInfo(0, "Balance", Brushes.SteelBlue);
            data.PrimaryTextBrushes.SetBrush(0, Brushes.WhiteSmoke);
            data.SecondaryTextBrushes.SetBrush(0, Brushes.Blue);

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

        /// <summary>
        /// Creates data - logarithms
        /// </summary>
        private void CreateChartData3()
        {
            LastDataSet = 3;
            TopTitle.Text = "Logarithms";

            //XAxisTextFormat = null;
            //XAxisTextProvider = null;
            //YAxisTextFormat = null;

            int maxX = 25;

            DataSeries data = DataSeries.Create(3);

            data.DataInfo.SetInfo(0, "Natural Logarithm", Brushes.Red);
            data.PrimaryTextBrushes.SetBrush(0, Brushes.White);
            data.SecondaryTextBrushes.SetBrush(0, Brushes.DarkBlue);

            data.DataInfo.SetInfo(1, "Log 10", Brushes.Blue);
            data.DataInfo.SetInfo(2, "Log 16", Brushes.DarkGreen);

            for (int x = 2; x <= maxX; x++)
            {
                // Base of Euler's number, about 2.71828.
                double y = Math.Log(x);
                data.Add(x, y);

                // base 10
                y = Math.Log10(x);
                data.Add(x, y);

                // base 16
                y = Math.Log(x, 16);
                data.Add(x, y);
            }

            data.ExpandX(1.0);
            data.MakeYAutoZero();

            Data = data;
        }


















        #endregion
    }
}
