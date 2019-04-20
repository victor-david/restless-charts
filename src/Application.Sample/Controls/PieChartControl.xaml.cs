using Restless.Controls.Chart;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for BarChartControl.xaml
    /// </summary>
    public partial class PieChartControl : ChartControlBase
    {
        public PieChartControl()
        {
            InitializeComponent();
            TopTitle.Text = "The Rain In Spain";

        }

        public override int DataSetCount => 2;

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

        /// <summary>
        /// Creates data - single series.
        /// </summary>
        private void CreateChartData1()
        {
            LastDataSet = 1;
            TopTitle.Text = "Slices of Pie";
            
            int minY = 100;
            int maxY = 500;
            int sliceCount = 10;

            DataSeries data = DataSeries.Create(sliceCount);
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            for (int slice = 0; slice < sliceCount; slice++)
            {
                int y = generator.GetValue();
                data.Add(0, y);
                data.DataInfo.SetInfo(slice, BrushUtility.GetRandomLinearBrush());
            }

            Data = data;
        }

        private void CreateChartData2()
        {
            LastDataSet = 2;
            TopTitle.Text = "Pizza";

            int minY = 1000;
            int maxY = 25000;
            int sliceCount = 5;

            DataSeries data = DataSeries.Create(sliceCount);
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            for (int slice = 0; slice < sliceCount; slice++)
            {
                int sliceValue = generator.GetValue();
                data.Add(0, sliceValue);
                data.DataInfo.SetInfo(slice, BrushUtility.GetRandomLinearBrush());
            }

            Data = data;
        }
    }
}
