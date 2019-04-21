using Restless.Controls.Chart;
using System;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for BarChartControl.xaml
    /// </summary>
    public partial class PieChartControl : ChartControlBase
    {
        #region Private
        private DataSeries data;
        #endregion

        #region Constructor
        public PieChartControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the count of data sets that this control supports.
        /// </summary>
        public override int DataSetCount => 3;
        
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
                case 3:
                    CreateChartData3();
                    break;
            }
        }
        #endregion

        #region Private methods
        private void CreateChartData1()
        {
            TopTitle.Text = "Work Force";
            CreateChartData(100, 500, 10, GetWorkForceLegend);
        }

        private void CreateChartData2()
        {
            TopTitle.Text = "Taxes";
            CreateChartData(1000, 25000, 5, GetTaxLegend);
        }

        private void CreateChartData3()
        {
            TopTitle.Text = "Departments";
            CreateChartData(500, 1000, 6, GetDepartmentLegend);
        }

        private void CreateChartData(int minY, int maxY, int sliceCount, Func<int, string> getLegend)
        {
            DataSeries data = DataSeries.Create(sliceCount);
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            for (int slice = 0; slice < sliceCount; slice++)
            {
                int sliceValue = generator.GetValue();
                data.Add(0, sliceValue);
                data.DataInfo.SetInfo(slice, getLegend(slice), BrushUtility.GetRandomLinearBrush());
            }

            Data = data;
        }
        #endregion

        #region Legend labels (private)
        private string[] workForce =
        {
            "Services", "Wholesale", "Financial", "Health Care",
            "Federal Goverment", "Agriculture", "Retail", "Transportation",
            "Information", "Hospitality"
        };

        private string[] taxes =
        {
            "Federal", "State", "Municipality", "Household", "Other"
        };

        private string[] departments =
        {
            "Accounting", "Human Resources", "Shipping",
            "Information Technology", "Executive", "Legal"
        };

        private string GetWorkForceLegend(int idx)
        {
            return (idx < workForce.Length) ? workForce[idx] : null;
        }

        private string GetTaxLegend(int idx)
        {
            return (idx < taxes.Length) ? taxes[idx] : null;
        }

        private string GetDepartmentLegend(int idx)
        {
            return (idx < departments.Length) ? departments[idx] : null;
        }
        #endregion
    }
}
