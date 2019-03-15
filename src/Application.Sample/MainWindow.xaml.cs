using Restless.Controls.Chart;
using System;
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
        private DataSeries data;
        private DataRange dataRange;
        // x
        private AxisPlacement xAxisPlacement;
        private TickVisibility xAxisTickVisibility;
        private string xAxisTextFormat;
        private IDoubleConverter xAxisTextProvider;
        private bool isXAxisReversed;
        private object topTitle;
        private object bottomTitle;
        // y
        private AxisPlacement yAxisPlacement;
        private TickVisibility yAxisTickVisibility;
        private string yAxisTextFormat;
        private IDoubleConverter yAxisTextProvider;
        private bool isYAxisReversed;
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

            XAxisPlacement = AxisPlacement.Top;
            XAxisTickVisibility = TickVisibility.Default;
            // XAxisTextFormat = "dd-MMM-yy";
            //XAxisTextFormat = "N1";
            //XAxisTextProvider = new DoubleToDateConverter();

            YAxisPlacement = AxisPlacement.DefaultY;
            
            YAxisTickVisibility = TickVisibility.Default;
            //YAxisTextFormat = "N1";

            Loaded += MainWindowLoaded;
        }
        #endregion

        #region Data Properties


        /// <summary>
        /// Gets the chart data range.
        /// </summary>
        public DataSeries Data
        {
            get => data;
            private set => SetProperty(ref data, value);
        }

        /// <summary>
        /// Gets the chart data range.
        /// </summary>
        public DataRange DataRange
        {
            get => dataRange;
            private set => SetProperty(ref dataRange, value);
        }
        #endregion

        #region X Axis properties
        /// <summary>
        /// Gets the placement of the X axis
        /// </summary>
        public AxisPlacement XAxisPlacement
        {
            get => xAxisPlacement;
            private set => SetProperty(ref xAxisPlacement, value);
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
        /// Gets a value that indicates if the X axis is reversed.
        /// </summary>
        public bool IsXAxisReversed
        {
            get => isXAxisReversed;
            private set => SetProperty(ref isXAxisReversed, value);
        }

        ///// <summary>
        ///// Gets or sets the zoom for the X axis.
        ///// </summary>
        //public double XZoomFactor
        //{
        //    get => xZoomFactor;
        //    set
        //    {
        //        if (SetProperty(ref xZoomFactor, value))
        //        {
        //            if (xRangeOriginal != null)
        //            {
        //                MainChart.XAxis.Range = xRangeOriginal.Zoom(xZoomFactor);
        //            }
        //        }
        //    }
        //}


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
        /// Gets the placement of the X axis
        /// </summary>
        public AxisPlacement YAxisPlacement
        {
            get => yAxisPlacement;
            private set => SetProperty(ref yAxisPlacement, value);
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
        /// Gets a value that indicates if the X axis is reversed.
        /// </summary>
        public bool IsYAxisReversed
        {
            get => isYAxisReversed;
            private set => SetProperty(ref isYAxisReversed, value);
        }

        ///// <summary>
        ///// Gets or sets the zoom for the Y axis.
        ///// </summary>
        //public double YZoomFactor
        //{
        //    get => yZoomFactor;
        //    set
        //    {
        //        if (SetProperty(ref yZoomFactor, value))
        //        {
        //            if (yRangeOriginal != null)
        //            {
        //                MainChart.YAxis.Range = yRangeOriginal.Zoom(yZoomFactor);
        //            }
        //        }
        //    }
        //}

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

        #region Private methods
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            LoadViaDataRange();
            //long min = DateTime.Now.AddMonths(-6).Ticks;
            //long max = DateTime.Now.Ticks;

            //Range xRange = new Range(min, max);
            //Range yRange = new Range(191685.94, 250611.08);
            //Range yRange = new Range(0, 2);

            //xRange = new Range(0, 5000);
            //yRange = new Range(-100, 100);

            //DataRange = new DataRange(xRange, yRange);

            //Random rand = new Random();
            //DateTime now = DateTime.Now;
            DataSeries data = new DataSeries();

            //for (int k = 0; k <= 100; k++)
            //{
            //    data.Add(new DataPoint(k, k));
            //}

            //int min = 10000;
            //int max = 20000;
            //for (int k = -30; k <= 0; k++)
            //{
            //    int value = rand.Next(min, max + 1);
            //    DataPoint point = new DataPoint(now.AddDays(k).Ticks, value);
            //    data.Add(point);
            //}

            //Data = data;
        }

        private void LoadViaData()
        {
        }

        private void LoadViaDataRange()
        {
            // XAxis[-129.15904, 229.15904]
            // YAxis[-129.15904, 229.15904]
            Range x = new Range(-129.15904, 229.15904);
            Range y = new Range(-129.15904, 229.15904);

            DataRange = new DataRange(x, y);
        }

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
            switch (XAxisPlacement)
            {
                case AxisPlacement.Top:
                    XAxisPlacement = AxisPlacement.Bottom;
                    BottomTitle = TopTitle;
                    TopTitle = null;
                    break;

                case AxisPlacement.Bottom:
                    XAxisPlacement = AxisPlacement.Top;
                    TopTitle = BottomTitle;
                    BottomTitle = null;
                    break;
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
            IsXAxisReversed = !IsXAxisReversed;
        }
        #endregion

        #region Y axis click handlers
        private void ButtonClickYAxisPlacement(object sender, RoutedEventArgs e)
        {
            switch (YAxisPlacement)
            {
                case AxisPlacement.Left:
                    YAxisPlacement = AxisPlacement.Right;
                    RightTitle = LeftTitle;
                    LeftTitle = null;
                    break;
                case AxisPlacement.Right:
                    YAxisPlacement = AxisPlacement.Left;
                    LeftTitle = RightTitle;
                    RightTitle = null;
                    break;
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
            IsYAxisReversed = !IsYAxisReversed;
        }
        #endregion

        private void XZoomSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double factor = e.NewValue > e.OldValue ? 1.2 : 1 / 1.2;
            if (MainChart != null)
            MainChart.XAxis.Range = MainChart.XAxis.Range.Zoom(factor);

            
        }


        private void YZoomSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double factor = e.NewValue > e.OldValue ? 1.2 : 1 / 1.2;
            if (MainChart != null)
            MainChart.YAxis.Range = MainChart.YAxis.Range.Zoom(factor);
        }
    }
}
