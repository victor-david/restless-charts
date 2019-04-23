using Restless.Controls.Chart;
using System;
using System.Windows.Media;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for BarChartControl.xaml
    /// </summary>
    public partial class PieChartControl : ChartControlBase
    {
        #region Private
        private DataSeries data;
        private double holeSize;
        private string valueFormat;
        private string legendHeader;
        #endregion

        #region Constructor
        public PieChartControl()
        {
            InitializeComponent();
            HoleSize = PieChart.DefaultHoleSize;
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

        /// <summary>
        /// Gets or sets the hole size.
        /// </summary>
        public double HoleSize
        {
            get => holeSize;
            set => SetProperty(ref holeSize, value);
        }

        /// <summary>
        /// Gets the value format.
        /// </summary>
        public string ValueFormat
        {
            get => valueFormat;
            private set => SetProperty(ref valueFormat, value);
        }

        /// <summary>
        /// Gets the legend header.
        /// </summary>
        public string LegendHeader
        {
            get => legendHeader;
            private set => SetProperty(ref legendHeader, value);
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
            TopTitle.Text = "Work Force Distribution (Millions)";
            LegendHeader = "Work Force Sector";
            ValueFormat = "N2";
            CreateChartData(100, 500, 10, 100.0, GetWorkForceLegend);
        }

        private void CreateChartData2()
        {
            TopTitle.Text = "Taxes";
            LegendHeader = "Tax Type";
            ValueFormat = "C0";
            CreateChartData(1000, 25000, 5, 0, GetTaxLegend);
        }

        private void CreateChartData3()
        {
            TopTitle.Text = "Employees Per Departments";
            LegendHeader = "Department";
            ValueFormat = null;
            CreateChartData(500, 1000, 6, 0, GetDepartmentLegend);
        }

        private void CreateChartData(int minY, int maxY, int sliceCount, double divFactor, Func<int, string> getLegend)
        {
            DataSeries data = DataSeries.Create(sliceCount);
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            for (int slice = 0; slice < sliceCount; slice++)
            {
                double sliceValue = generator.GetValue();
                if (divFactor > 0) sliceValue = sliceValue / divFactor;
                data.Add(0, sliceValue);
                data.DataInfo.SetInfo(slice, getLegend(slice), BrushUtility.GetRandomLinearBrush());
            }
            data.DataInfo.SetBorder(Brushes.Black, 2.0);
            data.DataInfo.SetPrimaryText(Brushes.DarkGray);
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
