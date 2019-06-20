using Restless.Controls.Chart;
using System;
using System.Diagnostics;
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
        private MappedValueConverter xAxisConverter;
        private IDoubleConverter yAxisConverter;
        #endregion

        #region Constructor
        public LineChartControl()
        {
            InitializeComponent();
            ChartStyle = LineChartStyle.Standard;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the count of data sets that this control supports.
        /// </summary>
        public override int DataSetCount => 4;

        /// <summary>
        /// Gets the data.
        /// </summary>
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

        /// <summary>
        /// Gets the X axis converter.
        /// </summary>
        public MappedValueConverter XAxisConverter
        {
            get => xAxisConverter;
            private set => SetProperty(ref xAxisConverter, value);
        }

        /// <summary>
        /// Gets the Y axis converter.
        /// </summary>
        public IDoubleConverter YAxisConverter
        {
            get => yAxisConverter;
            private set => SetProperty(ref yAxisConverter, value);
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
                case 4:
                    CreateChartData4();
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
            TopTitle.Text = "Exchange Rate USD to MXN";
            XAxisConverter = new MappedValueConverter();
            double divisor = 100.0;
            YAxisConverter = new DivisionConverter(divisor);
            int maxX = 120;
            int minY = 18;
            int maxY = 20;

            DataSeries data = DataSeries.Create();

            Random random = new Random();

            data.DataInfo.SetInfo(0, "Balance", Brushes.Red, Brushes.WhiteSmoke, Brushes.DarkRed);

            DateTime rateDate = DateTime.Now.AddDays(-maxX);

            for (int x = 0; x <= maxX; x++)
            {
                double yValue = random.Next(minY, maxY + 1);
                yValue += random.NextDouble();
                data.Add(x, yValue * divisor);
                XAxisConverter.AddToMap(x, rateDate.ToString("MMM dd\nyyyy"));
                rateDate = rateDate.AddDays(1);
            }

            data.ExpandX(4);
            data.DataRange.Y.IncreaseMaxBy(0.015);
            data.DataRange.Y.DecreaseMinBy(0.010);

            Data = data;
        }

        private void CreateChartData2()
        {
            TopTitle.Text = "Line Chart With Irregularly Spaced X";
            XAxisConverter = null;
            YAxisConverter = null;

            DataSeries data = DataSeries.Create();
            data.Add(2, 53491);
            data.Add(17, 109948);
            data.Add(19, 74687);
            data.Add(33, 67901);
            data.Add(35, 87100);
            data.MakeYAutoZero();
            data.DataRange.Y.Include(120000);
            data.ExpandX(4);
            data.DataInfo.SetInfo(0, "Balance", Brushes.Blue, Brushes.DarkBlue);
            Data = data;
        }

        /// <summary>
        /// Creates data - multiple series.
        /// </summary>
        private void CreateChartData3()
        {
            TopTitle.Text = "Multiple Series Line Chart";
            XAxisConverter = null;
            YAxisConverter = null;

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

        /// <summary>
        /// Creates data - logarithms
        /// </summary>
        private void CreateChartData4()
        {
            TopTitle.Text = "Logarithms";
            double divisor = 10.0;
            YAxisConverter = new DivisionConverter(divisor);


            int maxX = 25;

            DataSeries data = DataSeries.Create(3);

            data.DataInfo.SetInfo(0, "Natural Logarithm", Brushes.Red, Brushes.White, Brushes.DarkBlue);
            data.DataInfo.SetInfo(1, "Log 10", Brushes.Blue);
            data.DataInfo.SetInfo(2, "Log 16", Brushes.DarkGreen);

            for (int x = 2; x <= maxX; x++)
            {
                // Base of Euler's number, about 2.71828.
                double y = Math.Log(x);
                data.Add(x, y * divisor);

                // base 10
                y = Math.Log10(x);
                data.Add(x, y * divisor);

                // base 16
                y = Math.Log(x, 16);
                data.Add(x, y *divisor);
            }

            data.ExpandX(1.0);
            data.MakeYAutoZero();

            Data = data;
        }
        #endregion
    }
}
