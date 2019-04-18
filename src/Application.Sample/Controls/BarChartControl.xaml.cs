using Restless.Controls.Chart;
using System.Windows.Controls;
using System.Windows.Media;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for BarChartControl.xaml
    /// </summary>
    public partial class BarChartControl : ChartControlBase
    {
        public BarChartControl()
        {
            InitializeComponent();
            //DataContext = this;
        }

        public override int DataSetCount => 3;

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
        public override void CreateChartData(int dataSet)
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

        /// <summary>
        /// Creates data - single series.
        /// </summary>
        private void CreateChartData1()
        {
            LastDataSet = 1;
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

        private void CreateChartData2()
        {
            LastDataSet = 2;
        }

        private void CreateChartData3()
        {
            LastDataSet = 3;

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

    }
}
