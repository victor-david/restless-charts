using Restless.Controls.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Application.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Private
        // data
        private ActiveChartType chartType;
        private ChartControlBase chartControl;

        //private ChartBase chart;
        //private BarChart barChart;
        //private LineChart lineChart;
        //private PieChart pieChart;
        private int activeDataSet;
        // chart
        private Brush chartBackground;
        private Orientation chartOrientation;
        private bool isAxisGridVisible;
        private bool isNavigationHelpButtonVisible;
        private bool isNavigationHelpVisible;
        private bool isLegendHelpButtonVisible;
        private bool isLegendVisible;
        // x
        private bool isXAxisPlacementReversed;
        private TickVisibility xAxisTickVisibility;
        private TickAlignment xAxisTickAlignment;
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

        /************************************************************************/

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Commands = new CommandDictionary();
            InitializeCommands();
            

            //TopTitle = TryFindResource("TopTitle");
            //LeftTitle = TryFindResource("LeftTitle");
            //RightTitle = TryFindResource("LeftTitle");

            //IsXAxisPlacementReversed = true;
            XAxisTickVisibility = TickVisibility.Default;
            XAxisTickAlignment = TickAlignment.Default;
            YAxisTickVisibility = TickVisibility.Default;

            ChartOrientation = Orientation.Vertical;
            IsAxisGridVisible = true;
            IsNavigationHelpButtonVisible = true;
            IsLegendHelpButtonVisible = true;

            //barChart = new BarChart() { DisplayValues = true };
            //lineChart = new LineChart()
            //{
            //    DisplayValues = true,
            //    LineThickness = 1.0,
            //    PointSize = 10.0,
            //};

            //pieChart = new PieChart();

            //Chart = barChart;

            ChartBackground = Brushes.LightGoldenrodYellow;

            //ChartType = ActiveChartType.Bar;

            Loaded += (s, e) => ChartType = ActiveChartType.Bar;
        }
        #endregion

        /************************************************************************/

        #region Commands (property, initialization and handlers)
        /// <summary>
        /// Gets the commands
        /// </summary>
        public CommandDictionary Commands
        {
            get;
        }
        
        private void InitializeCommands()
        {
            Commands.Add("ChartBar", (p) => ChartType = ActiveChartType.Bar);
            Commands.Add("ChartLine", (p) => ChartType = ActiveChartType.Line);
            Commands.Add("ChartPie", (p) => ChartType = ActiveChartType.Pie);

            Commands.Add("OrientationVert", (p) => ChartOrientation = Orientation.Vertical);
            Commands.Add("OrientationHorz", (p) => ChartOrientation = Orientation.Horizontal);
            Commands.Add("ToggleGrid", (p) => IsAxisGridVisible = !IsAxisGridVisible);
        }
        #endregion

        /************************************************************************/

        #region Chart properties
        public ActiveChartType ChartType
        {
            get => chartType;
            private set
            {
                if (SetProperty(ref chartType, value))
                {
                    switch (chartType)
                    {
                        case ActiveChartType.None:
                            ChartControl = null;
                            break;
                        case ActiveChartType.Bar:
                            ChartControl = ChartControlBase.GetChartControl<BarChartControl>();
                            break;
                        case ActiveChartType.Line:
                            ChartControl = null;
                            break;
                        case ActiveChartType.Pie:
                            ChartControl = null;
                            break;
                    }
                    if (ChartControl != null)
                    {
                        ChartControl.CreateChartData();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the user control that has the active chart (bar, line , pie, etc)
        /// </summary>
        public ChartControlBase ChartControl
        {
            get => chartControl;
            private set => SetProperty(ref chartControl, value);
        }

        /// <summary>
        /// Gets the background color for the chart.
        /// </summary>
        public Brush ChartBackground
        {
            get => chartBackground;
            private set => SetProperty(ref chartBackground, value);
        }

        //public DataSeries Data
        //{
        //    get;
        //    private set;
        //}

        ///// <summary>
        ///// Gets the chart that is used for the chart container.
        ///// </summary>
        //public ChartBase Chart
        //{
        //    get => chart;
        //    private set => SetProperty(ref chart, value);
        //}

        /// <summary>
        /// Gets the orientation of the chart.
        /// </summary>
        public Orientation ChartOrientation
        {
            get => chartOrientation;
            private set => SetProperty(ref chartOrientation, value);
        }

        /// <summary>
        /// Gets a value that determines if the axis grid is displayed.
        /// </summary>
        public bool IsAxisGridVisible
        {
            get => isAxisGridVisible;
            private set => SetProperty(ref isAxisGridVisible, value);
        }

        /// <summary>
        /// Gets a value that determines if the chart navigation help button is displayed.
        /// If the button is not visible, you can still control the visibility
        /// of the help itself programmatically.
        /// </summary>
        public bool IsNavigationHelpButtonVisible
        {
            get => isNavigationHelpButtonVisible;
            private set => SetProperty(ref isNavigationHelpButtonVisible, value);
        }

        /// <summary>
        /// Gets a value that determines if the chart navigation help itself is displayed.
        /// You can control the display of navigation help programmatically or use
        /// the built in button if it is visible.
        /// </summary>
        public bool IsNavigationHelpVisible
        {
            get => isNavigationHelpVisible;
            set => SetProperty(ref isNavigationHelpVisible, value);
        }

        /// <summary>
        /// Gets a value that determines if the legend help button is displayed.
        /// If the button is not visible, you can still control the visibility
        /// of the legend itself programmatically.
        /// </summary>
        public bool IsLegendHelpButtonVisible
        {
            get => isLegendHelpButtonVisible;
            private set => SetProperty(ref isLegendHelpButtonVisible, value);
        }

        /// <summary>
        /// Gets a value that determines if the legend itself is displayed.
        /// You can control the display of the legend programmatically or use
        /// the built in button if it is visible.
        /// </summary>
        public bool IsLegendVisible
        {
            get => isLegendVisible;
            set => SetProperty(ref isLegendVisible, value);
        }
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
        /// Gets the tick alignment for the X axis.
        /// </summary>
        public TickAlignment XAxisTickAlignment
        {
            get => xAxisTickAlignment;
            private set => SetProperty(ref xAxisTickAlignment, value);
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
            XAxisTickVisibility = GetNextTickVisibility(XAxisTickVisibility);
        }

        private void ButtonClickXAxisTickAlignment(object sender, RoutedEventArgs e)
        {
            XAxisTickAlignment = XAxisTickAlignment == TickAlignment.Range ? TickAlignment.Values : TickAlignment.Range;
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
            else if (XAxisTextProvider is DoubleToDateConverter)
            {
                XAxisTextProvider = new DoubleToLookupConverter();
                XAxisTextFormat = null;
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
            YAxisTickVisibility = GetNextTickVisibility(YAxisTickVisibility);
        }

        // also used by ButtonClickXAxisTicks()
        private TickVisibility GetNextTickVisibility(TickVisibility currentVisibility)
        {
            switch (currentVisibility)
            {
                case TickVisibility.MajorMinorEdge:
                    return TickVisibility.MajorMinor;
                case TickVisibility.MajorMinor:
                    return TickVisibility.Major;
                case TickVisibility.Major:
                    return TickVisibility.None;
                default:
                    return TickVisibility.MajorMinorEdge;
            }
        }

        private void ButtonClickYAxisReverse(object sender, RoutedEventArgs e)
        {
            IsYAxisValueReversed = !IsYAxisValueReversed;
        }

        private void ButtonClickYAxisProvider(object sender, RoutedEventArgs e)
        {
            YAxisTextFormat = string.IsNullOrEmpty(YAxisTextFormat) ? "C0" : null;
        }
        #endregion

        #region Chart click handlers
        private void ButtonClickSwitchChartType(object sender, RoutedEventArgs e)
        {
            //if (Chart is BarChart)
            //{
            //    Chart = lineChart;
            //}
            //else if (Chart is LineChart)
            //{
            //    Chart = pieChart;
            //}
            //else
            //{
            //    Chart = barChart;
            //}

            //Dispatcher.Invoke(new Action(() =>
            //{
            //    CreateActiveData();
            //    //MainChart.RestoreSizeAndPosition();

            //}), DispatcherPriority.Loaded);
        }

        private void ButtonClickChangeChartStyle(object sender, RoutedEventArgs e)
        {
            //if (Chart is LineChart chart)
            //{
            //    switch (chart.ChartStyle)
            //    {
            //        case LineChartStyle.Standard:
            //            chart.ChartStyle = LineChartStyle.StandardCirclePoint;
            //            break;
            //        case LineChartStyle.StandardCirclePoint:
            //            chart.ChartStyle = LineChartStyle.StandardSquarePoint;
            //            break;
            //        case LineChartStyle.StandardSquarePoint:
            //            chart.ChartStyle = LineChartStyle.Filled;
            //            break;
            //        case LineChartStyle.Filled:
            //            chart.ChartStyle = LineChartStyle.Standard;
            //            break;
            //    }
            //}
        }

        private void ButtonClickChartOrientation(object sender, RoutedEventArgs e)
        {
            ChartOrientation = ChartOrientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical;
        }

        private void ButtonClickRestoreChart(object sender, RoutedEventArgs e)
        {
            //MainChart.RestoreSizeAndPosition();
        }

        private void ButtonClickToggleDisplayAxisGrid(object sender, RoutedEventArgs e)
        {
            IsAxisGridVisible = !IsAxisGridVisible;
        }

        private void ButtonClickToggleValuesDisplay(object sender, RoutedEventArgs e)
        {
           //Chart.DisplayValues = !Chart.DisplayValues;
        }

        private void ButtonClickToggleNavigationHelpButton(object sender, RoutedEventArgs e)
        {
            IsNavigationHelpButtonVisible = !IsNavigationHelpButtonVisible;
        }

        private void ButtonClickToggleNavigationHelp(object sender, RoutedEventArgs e)
        {
            IsLegendVisible = false;
            IsNavigationHelpVisible = !IsNavigationHelpVisible;
        }

        private void ButtonClickToggleLegendHelpButton(object sender, RoutedEventArgs e)
        {
            IsLegendHelpButtonVisible = !IsLegendHelpButtonVisible;
        }

        private void ButtonClickToggleLegendDisplay(object sender, RoutedEventArgs e)
        {
            IsNavigationHelpVisible = false;
            IsLegendVisible = !IsLegendVisible;
        }
        #endregion

        #region Data click handlers
        private void ButtonClickChartUseData1(object sender, RoutedEventArgs e)
        {
            CreateTestData1();
        }

        private void ButtonClickChartUseData2(object sender, RoutedEventArgs e)
        {
            CreateTestData2();
        }

        private void ButtonClickChartUseData3(object sender, RoutedEventArgs e)
        {
            CreateTestData3();
        }

        private void ButtonClickChartUseData4(object sender, RoutedEventArgs e)
        {
            CreateTestData4();
        }

        private void ButtonClickChartUserData5(object sender, RoutedEventArgs e)
        {
            CreateTestData5();
        }
        #endregion

        #region Create data
        //private void MainWindowLoaded(object sender, RoutedEventArgs e)
        //{
        //    CreateTestData2();
        //}

        /// <summary>
        /// Creates data - single series.
        /// </summary>
        private void CreateTestData1()
        {
            activeDataSet = 1;
            XAxisTextFormat = null;
            XAxisTextProvider = null;
            SetTopTitle("Data Set #1");

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

            //chart.Data = data;
        }

        /// <summary>
        /// Creates data - faker category data.
        /// </summary>
        private void CreateTestData2()
        {
            activeDataSet = 2;
            XAxisTextFormat = null;
            XAxisTextProvider = new DoubleToLookupConverter();
            SetTopTitle("Data Set #2");

            DataSeries data = DataSeries.Create();
            data.DataInfo.SetInfo(0, "Amount", Brushes.Red);
            data.PrimaryTextBrushes.SetBrush(0, Brushes.WhiteSmoke);
            data.SecondaryTextBrushes.SetBrush(0, Brushes.DarkRed);


            CategoryTable catTable = new CategoryTable();
            foreach (CategoryRow row in catTable.OrderBy((cat) => cat.Name))
            {
                data.Add(row.Id, row.Amount);
            }

            data.ExpandX(1.0);
            data.MakeYAutoZero();

            //chart.Data = data;
        }

        /// <summary>
        /// Create data - multiple series
        /// </summary>
        private void CreateTestData3()
        {
            activeDataSet = 3;
            XAxisTextFormat = null;
            XAxisTextProvider = null;
            SetTopTitle("Data Set #3");

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

            //chart.Data = data;
        }

        /// <summary>
        /// This test data represents dates and amounts.
        /// </summary>
        private void CreateTestData4()
        {
            activeDataSet = 4;
            XAxisTextFormat = "MMM-yy";
            XAxisTextProvider = new DoubleToDateConverter();
            YAxisTextFormat = "C0";
            SetTopTitle("Data Set #4");

            int minY = 10000;
            int maxY = 20000;

            DataSeries data = DataSeries.Create();
            RandomGenerator generator = new RandomGenerator(minY, maxY);

            //if (Chart is BarChart)
            //{
            //    data.DataInfo.SetInfo(0, "Balance", (Brush)TryFindResource("HeaderBrush"));
            //    data.PrimaryTextBrushes.SetBrush(0, Brushes.WhiteSmoke);
            //    data.SecondaryTextBrushes.SetBrush(0, Brushes.Black);
            //}
            //else
            //{
            //    data.DataInfo.SetInfo(0, "Balance", Brushes.Firebrick);
            //    data.PrimaryTextBrushes.SetBrush(0, Brushes.Transparent);
            //}

            DateTime now = DateTime.Now;
            DateTime start = GetMonth(now, -12);

            List<DateTime> months = new List<DateTime>();

            for (int k = 0; k < 12; k++)
            {
                months.Add(GetMonth(start, k));
            }

            foreach (DateTime x in months)
            {
                int y = generator.GetValue();
                data.Add(x.Ticks, y);
            }

            data.ExpandX(GetTicksPerDay() * 15);
            data.DataRange.Y.Include(maxY);

            data.MakeYAutoZero();

            //chart.Data = data;
        }

        /// <summary>
        /// This test data provides logarithms
        /// </summary>
        private void CreateTestData5()
        {
            activeDataSet = 5;

            XAxisTextFormat = null;
            XAxisTextProvider = null;
            YAxisTextFormat = null;
            SetTopTitle("Logarithms", "Data Set #5");

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

            //chart.Data = data;
        }

        /// <summary>
        /// Creates the last used data set.
        /// </summary>
        private void CreateActiveData()
        {
            switch (activeDataSet)
            {
                case 1:
                    CreateTestData1();
                    break;
                case 2:
                    CreateTestData2();
                    break;
                case 3:
                    CreateTestData3();
                    break;
                case 4:
                    CreateTestData4();
                    break;
                case 5:
                    CreateTestData5();
                    break;
                default:
                    CreateTestData1();
                    break;
            }
        }


        private DateTime GetMonth(DateTime date, int monthsToAdd)
        {
            return new DateTime(date.AddMonths(monthsToAdd).Year, date.AddMonths(monthsToAdd).Month, 1);
        }

        private long GetTicksPerDay()
        {
            TimeSpan span = new TimeSpan(24, 0, 0);
            return span.Ticks;
        }

        private void SetTopTitle(string extraText)
        {
            SetTopTitle("Balance History", extraText);
        }

        private void SetTopTitle(string text, string extraText)
        {
            if (TryFindResource("TopTitle") is TextBlock textBlock)
            {
                textBlock.Text = $"{text} ({extraText})";
                TopTitle = textBlock;
            }
        }
        #endregion
    }
}
