using Restless.Controls.Chart;
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
        // chart
        private ActiveChartType chartType;
        private ChartControlBase chartControl;
        private Brush chartBackground;
        private Orientation chartOrientation;
        private bool displayChartValues;
        private bool isAxisGridVisible;
        // menu
        private bool isOrientationMenuEnabled = true;
        private bool isAxisMenuEnabled = true;
        private bool isGridLineMenuEnabled = true;
        private bool isBarChartMenuVisible = false;
        private bool isLineChartMenuVisible = false;
        private bool isPieChartMenuVisible = false;

        //private bool isNavigationHelpButtonVisible;
        //private bool isNavigationHelpVisible;
        // x
        private bool isXAxisPlacementReversed;
        private TickVisibility xAxisTickVisibility;
        private TickAlignment xAxisTickAlignment;
        private string xAxisTextFormat;
        private IDoubleConverter xAxisTextProvider;
        private bool isXAxisValueReversed;
        // y
        private bool isYAxisPlacementReversed;
        private TickVisibility yAxisTickVisibility;
        private string yAxisTextFormat;
        private IDoubleConverter yAxisTextProvider;
        private bool isYAxisValueReversed;
        #endregion

        /************************************************************************/

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Commands = new CommandDictionary();
            InitializeCommands();

            XAxisTickVisibility = TickVisibility.Default;
            XAxisTickAlignment = TickAlignment.Default;
            YAxisTickVisibility = TickVisibility.Default;

            ChartOrientation = Orientation.Vertical;
            DisplayChartValues = true;
            IsAxisGridVisible = true;
            //IsNavigationHelpButtonVisible = true;

            ChartBackground = Brushes.LightGoldenrodYellow;

            ChartType = ActiveChartType.Bar;
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
            Commands.Add("ChartMultiple", (p) => ChartType = ActiveChartType.Multiple);

            Commands.Add("OrientationVert", (p) => ChartOrientation = Orientation.Vertical);
            Commands.Add("OrientationHorz", (p) => ChartOrientation = Orientation.Horizontal);
            Commands.Add("ToggleGridLines", (p) => IsAxisGridVisible = !IsAxisGridVisible);
            Commands.Add("ToggleDisplayValues", (p) => DisplayChartValues = !DisplayChartValues);

            Commands.Add("SetXTicks", (p) => { if (p is TickVisibility v) XAxisTickVisibility = v; });
            Commands.Add("ToggleXPlacement", (p) => IsXAxisPlacementReversed = !IsXAxisPlacementReversed);
            Commands.Add("ToggleXAlignment", (p) => XAxisTickAlignment = XAxisTickAlignment == TickAlignment.Range ? TickAlignment.Values : TickAlignment.Range);
            Commands.Add("ToggleXReverse", (p) => IsXAxisValueReversed = !IsXAxisValueReversed);

            Commands.Add("SetYTicks", (p) => { if (p is TickVisibility v) YAxisTickVisibility = v; });
            Commands.Add("ToggleYPlacement", (p) => IsYAxisPlacementReversed = !IsYAxisPlacementReversed);
            Commands.Add("ToggleYReverse", (p) => IsYAxisValueReversed = !IsYAxisValueReversed);
            Commands.Add("SetYTicksFormat", (p) => YAxisTextFormat = (string)p);

            Commands.Add("SetChartData", SetChartData);
            Commands.Add("SetLineStyle", SetLineStyle);
        }

        private void SetChartData(object parm)
        {
            if (parm is string str)
            {
                if (int.TryParse(str, out int dataSet))
                {
                    ChartControl.CreateChartData(dataSet);
                }
            }
        }
        private void SetLineStyle(object parm)
        {
            if (ChartControl is LineChartControl lc && parm is LineChartStyle s)
            {
                lc.ChartStyle = s;
            }
        }
        #endregion

        /************************************************************************/

        #region Chart properties
        /// <summary>
        /// Gets the active chart type.
        /// </summary>
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
                            ChartControl = ChartControlBase.GetChartControl<LineChartControl>();
                            break;
                        case ActiveChartType.Pie:
                            ChartControl = ChartControlBase.GetChartControl<PieChartControl>();
                            break;
                        case ActiveChartType.Multiple:
                            ChartControl = ChartControlBase.GetChartControl<MultipleChartControl>();
                            break;
                    }
                    if (ChartControl != null)
                    {
                        ChartControl.CreateChartData();
                    }
                    SyncMenuItemsToChartType();
                }
            }
        }

        private void SyncMenuItemsToChartType()
        {
            IsOrientationMenuEnabled = chartType != ActiveChartType.Pie && chartType != ActiveChartType.None;
            IsAxisMenuEnabled = chartType != ActiveChartType.Pie && chartType != ActiveChartType.None;
            IsGridLineMenuEnabled = chartType != ActiveChartType.Pie && chartType != ActiveChartType.None;
            IsBarChartMenuVisible = chartType == ActiveChartType.Bar;
            IsLineChartMenuVisible = chartType == ActiveChartType.Line;
            IsPieChartMenuVisible = chartType == ActiveChartType.Pie;
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

        /// <summary>
        /// Gets the orientation of the chart.
        /// </summary>
        public Orientation ChartOrientation
        {
            get => chartOrientation;
            private set => SetProperty(ref chartOrientation, value);
        }

        /// <summary>
        /// Gets a boolean value that determines if values are displayed on the chart.
        /// </summary>
        public bool DisplayChartValues
        {
            get => displayChartValues;
            private set => SetProperty(ref displayChartValues, value);
        }

        /// <summary>
        /// Gets a value that determines if the axis grid is displayed.
        /// </summary>
        public bool IsAxisGridVisible
        {
            get => isAxisGridVisible;
            private set => SetProperty(ref isAxisGridVisible, value);
        }
        #endregion

        /************************************************************************/

        #region Menu properties
        public bool IsOrientationMenuEnabled
        {
            get => isOrientationMenuEnabled;
            private set => SetProperty(ref isOrientationMenuEnabled, value);
        }

        public bool IsAxisMenuEnabled
        {
            get => isAxisMenuEnabled;
            private set => SetProperty(ref isAxisMenuEnabled, value);
        }

        public bool IsGridLineMenuEnabled
        {
            get => isGridLineMenuEnabled;
            private set => SetProperty(ref isGridLineMenuEnabled, value);
        }

        public bool IsBarChartMenuVisible
        {
            get => isBarChartMenuVisible;
            private set => SetProperty(ref isBarChartMenuVisible, value);
        }

        public bool IsLineChartMenuVisible
        {
            get => isLineChartMenuVisible;
            private set => SetProperty(ref isLineChartMenuVisible, value);
        }

        public bool IsPieChartMenuVisible
        {
            get => isPieChartMenuVisible;
            private set => SetProperty(ref isPieChartMenuVisible, value);
        }
        #endregion

        /************************************************************************/

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
        #endregion

        /************************************************************************/

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
        #endregion

        /************************************************************************/

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
    }
}
