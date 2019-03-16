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
        private DataSeriesCollection data;
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
            // CreateTestData1();
            CreateTestData2();
        }

        private void CreateTestData1()
        {
            int max = 10;
            Random rand = new Random();

            DataSeriesCollection data = new DataSeriesCollection();

            DataSeries series = data.Add();

            for (int x = 0; x <= max; x++)
            {
                int y = rand.Next(-50, 100);
                series.Add(x, y);
            }

            data.MakeYZeroCentered();
            data.ExpandX(0.75);
            data.DataRange.Y.IncreaseMaxBy(0.05);
            data.DataRange.Y.DecreaseMinBy(0.05);
            Data = data;
        }

        /// <summary>
        /// This test data represents dates and amounts.
        /// </summary>
        private void CreateTestData2()
        {
            XAxisTextFormat = "dd-MMM-yy";
            XAxisTextProvider = new DoubleToDateConverter();

            DataSeriesCollection data = new DataSeriesCollection();
            DataSeries series = data.Add();

            DateTime now = DateTime.Now;
            Random rand = new Random();

            int min = 10000;
            int max = 20000;
            int y = 1000;
            for (int k = -15; k <= 0; k++)
            {
                int value = rand.Next(min, max + 1);
                series.Add(now.AddDays(k).Ticks, y);
                y += 100;
            }

            //for (int k = -35; k <= -18; k++)
            //{
            //    int value = rand.Next(min, max + 1);
            //    series.Add(now.AddDays(k).Ticks, value);
            //    y += 100;
            //}


            long ticksPerDay = now.Ticks - now.AddDays(-1).Ticks;
            data.ExpandX(ticksPerDay);
            data.DataRange.Y.Include(0);
            data.DataRange.Y.IncreaseMaxBy(0.125);


            Data = data;

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
            XAxisPlacement = (XAxisPlacement == AxisPlacement.Bottom) ? AxisPlacement.Top : AxisPlacement.Bottom;
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
    }
}
