using Restless.Controls.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Private
        // data
        private DataSeriesCollection data;
        // chart
        private Orientation chartOrientation;

        // x
        private bool isXAxisPlacementReversed;
        private TickVisibility xAxisTickVisibility;
        private string xAxisTextFormat;
        private IDoubleConverter xAxisTextProvider;
        private bool isXAxisValueReversed;
        private object topTitle;
        private object bottomTitle;
        // y
        private bool isYAxisPlacementReversed;
        private TickVisibility yAxisTickVisibility;
        private string yAxisTextFormat;
        private IDoubleConverter yAxisTextProvider;
        private bool isYAxisValueReversed;
        private object leftTitle;
        private object rightTitle;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            TopTitle = TryFindResource("TopTitle");
            //LeftTitle = TryFindResource("LeftTitle");
            //RightTitle = TryFindResource("LeftTitle");

            IsXAxisPlacementReversed = true;
            XAxisTickVisibility = TickVisibility.Default;
            YAxisTickVisibility = TickVisibility.Default;

            ChartOrientation = Orientation.Vertical;

            Loaded += MainWindowLoaded;
        }
        #endregion

        #region Chart properties
        /// <summary>
        /// Gets the orientation of the chart
        /// </summary>
        public Orientation ChartOrientation
        {
            get => chartOrientation;
            private set => SetProperty(ref chartOrientation, value);
        }
        #endregion

        #region Data Properties
        /// <summary>
        /// Gets the chart data range.
        /// </summary>
        public DataSeriesCollection Data
        {
            get => data;
            private set => SetProperty(ref data, value);
        }

        ///// <summary>
        ///// Gets the chart data range.
        ///// </summary>
        //public DataRange DataRange
        //{
        //    get => dataRange;
        //    private set => SetProperty(ref dataRange, value);
        //}
        #endregion

        #region X Axis properties
        /// <summary>
        /// Gets a value that determines if the placement of the X axis is reversed from its default.
        /// </summary>
        public bool IsXAxisPlacementReversed
        {
            get => isXAxisPlacementReversed;
            private set => SetProperty(ref isXAxisPlacementReversed, value);
        }

        /// <summary>
        /// Gets the tick visibility for the X axis.
        /// </summary>
        public TickVisibility XAxisTickVisibility
        {
            get => xAxisTickVisibility;
            private set => SetProperty(ref xAxisTickVisibility, value);
        }

        /// <summary>
        /// Gets the text format for the X axis.
        /// This property is not used if <see cref="XAxisTextProvider"/> is non null.
        /// </summary>
        public string XAxisTextFormat
        {
            get => xAxisTextFormat;
            private set => SetProperty(ref xAxisTextFormat, value);
        }

        /// <summary>
        /// Gets the text provider converter for the X axis.
        /// When this property is non null, <see cref="XAxisTextFormat"/> is not used.
        /// </summary>
        public IDoubleConverter XAxisTextProvider
        {
            get => xAxisTextProvider;
            private set => SetProperty(ref xAxisTextProvider, value);
        }

        /// <summary>
        /// Gets a value that indicates if the values on the X axis are reversed.
        /// </summary>
        public bool IsXAxisValueReversed
        {
            get => isXAxisValueReversed;
            private set => SetProperty(ref isXAxisValueReversed, value);
        }

        /// <summary>
        /// Gets the top title.
        /// </summary>
        public object TopTitle
        {
            get => topTitle;
            private set => SetProperty(ref topTitle, value);
        }

        /// <summary>
        /// Gets the bottom title.
        /// </summary>
        public object BottomTitle
        {
            get => bottomTitle;
            private set => SetProperty(ref bottomTitle, value);
        }
        #endregion

        #region Y Axis properties
        /// <summary>
        /// Gets a value that determines if the placement of the Y axis is reversed from its default.
        /// </summary>
        public bool IsYAxisPlacementReversed
        {
            get => isYAxisPlacementReversed;
            private set => SetProperty(ref isYAxisPlacementReversed, value);
        }

        /// <summary>
        /// Gets the tick visibility for the Y axis.
        /// </summary>
        public TickVisibility YAxisTickVisibility
        {
            get => yAxisTickVisibility;
            private set => SetProperty(ref yAxisTickVisibility, value);
        }

        /// <summary>
        /// Gets the text format for the Y axis.
        /// This property is not used if <see cref="xAxisTextProvider"/> is non null.
        /// </summary>
        public string YAxisTextFormat
        {
            get => yAxisTextFormat;
            private set => SetProperty(ref yAxisTextFormat, value);
        }

        /// <summary>
        /// Gets the text provider converter for the Y axis.
        /// When this property is non null, <see cref="YAxisTextFormat"/> is not used.
        /// </summary>
        public IDoubleConverter YAxisTextProvider
        {
            get => yAxisTextProvider;
            private set => SetProperty(ref yAxisTextProvider, value);
        }

        /// <summary>
        /// Gets a value that indicates if the values on the Y axis are reversed.
        /// </summary>
        public bool IsYAxisValueReversed
        {
            get => isYAxisValueReversed;
            private set => SetProperty(ref isYAxisValueReversed, value);
        }

        /// <summary>
        /// Gets the left title.
        /// </summary>
        public object LeftTitle
        {
            get => leftTitle;
            private set => SetProperty(ref leftTitle, value);
        }

        /// <summary>
        /// Gets the right title.
        /// </summary>
        public object RightTitle
        {
            get => rightTitle;
            private set => SetProperty(ref rightTitle, value);
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Property Changed

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region X axis click handlers

        private void ButtonClickXAxisPlacement(object sender, RoutedEventArgs e)
        {
            IsXAxisPlacementReversed = !IsXAxisPlacementReversed;
            if (XAxisTickVisibility == TickVisibility.None)
            {
                XAxisTickVisibility = TickVisibility.Default;
            }
        }

        private void ButtonClickXAxisTicks(object sender, RoutedEventArgs e)
        {
            switch (XAxisTickVisibility)
            {
                case TickVisibility.MajorAndMinor:
                    XAxisTickVisibility = TickVisibility.MajorOnly;
                    break;
                case TickVisibility.MajorOnly:
                    XAxisTickVisibility = TickVisibility.None;
                    break;
                case TickVisibility.None:
                    XAxisTickVisibility = TickVisibility.MajorAndMinor;
                    break;
            }
        }

        private void ButtonClickXAxisReverse(object sender, RoutedEventArgs e)
        {
            IsXAxisValueReversed = !IsXAxisValueReversed;
        }

        private void ButtonClickXAxisProvider(object sender, RoutedEventArgs e)
        {
            if (XAxisTextProvider == null)
            {
                XAxisTextProvider = new DoubleToDateConverter();
                XAxisTextFormat = "dd-MMM-yy";
            }
            else
            {
                XAxisTextProvider = null;
                XAxisTextFormat = null;
            }
        }
        #endregion

        #region Y axis click handlers
        private void ButtonClickYAxisPlacement(object sender, RoutedEventArgs e)
        {
            IsYAxisPlacementReversed = !IsYAxisPlacementReversed;
            if (YAxisTickVisibility == TickVisibility.None)
            {
                YAxisTickVisibility = TickVisibility.Default;
            }
        }

        private void ButtonClickYAxisTicks(object sender, RoutedEventArgs e)
        {
            switch (YAxisTickVisibility)
            {
                case TickVisibility.MajorAndMinor:
                    YAxisTickVisibility = TickVisibility.MajorOnly;
                    break;
                case TickVisibility.MajorOnly:
                    YAxisTickVisibility = TickVisibility.None;
                    break;
                case TickVisibility.None:
                    YAxisTickVisibility = TickVisibility.MajorAndMinor;
                    break;
            }
        }

        private void ButtonClickYAxisReverse(object sender, RoutedEventArgs e)
        {
            IsYAxisValueReversed = !IsYAxisValueReversed;
        }

        private void ButtonClickYAxisProvider(object sender, RoutedEventArgs e)
        {
            YAxisTextFormat = string.IsNullOrEmpty(YAxisTextFormat) ? "N3" : null;
        }

        #endregion

        #region Chart click handlers
        private void ButtonClickChartOrientation(object sender, RoutedEventArgs e)
        {
            ChartOrientation = ChartOrientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical;
        }

        private void ButtonClickRestoreChart(object sender, RoutedEventArgs e)
        {
            MainChart.RestoreSizeAndPosition();
        }


        #endregion

        #region Create data
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            //CreateTestData1();
            CreateTestData2();
        }

        private void CreateTestData1()
        {
            int maxX = 200;
            int minY = -150;
            int maxY = 150;
            
            Random rand = new Random();

            DataSeriesCollection data = new DataSeriesCollection();

            DataSeries series = data.Add();

            for (int x = 0; x < maxX; x++)
            {
                int y = rand.Next(minY, maxY + 1);
                series.Add(x, y);
            }

            data.ExpandX(0.75);
            data.DataRange.Y.IncreaseMaxBy(0.05);
            data.MakeYZeroCentered();
            //data.DataRange.Y.DecreaseMinBy(0.05);
            Data = data;
        }

        /// <summary>
        /// This test data represents dates and amounts.
        /// </summary>
        private void CreateTestData2()
        {
            XAxisTextFormat = "MMM-yy";
            XAxisTextProvider = new DoubleToDateConverter();

            DataSeriesCollection data = new DataSeriesCollection();
            DataSeries series = data.Add();
            DateTime now = DateTime.Now;
            DateTime start = GetMonth(now, -12);

            List<DateTime> months = new List<DateTime>();

            for (int k = 0; k < 12; k++)
            {
                months.Add(GetMonth(start, k));
            }

            Random rand = new Random();

            int min = 10000;
            int max = 20000;

            foreach (DateTime x in months)
            {
                int y = rand.Next(min, max + 1);
                series.Add(x.Ticks, y);

            }
            
            data.ExpandY(1000);
            data.DataRange.Y.Include(0);
            Data = data;
        }

        private DateTime GetMonth(DateTime date, int monthsToAdd)
        {
            return new DateTime(date.AddMonths(monthsToAdd).Year, date.AddMonths(monthsToAdd).Month, 1);
        }
        #endregion
    }
}
