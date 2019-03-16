using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides a container for chart controls. Provides titles, axis, axis grid and navigation.
    /// </summary>
    public class ChartContainer : ContentControl
    {
        #region Public fields
        /// <summary>
        /// Gets the default orientation
        /// </summary>
        public const Orientation DefaultOrientation = Orientation.Vertical;
        /// <summary>
        /// Gets the default grid border size.
        /// </summary>
        public const double DefaultGridBorderSize = 1.0;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartContainer"/> class.
        /// </summary>
        public ChartContainer()
        {
            XAxis = new Axis()
            {
                Name = "XAxis",
                AxisPlacement = AxisPlacement.DefaultX,
            };
            YAxis = new Axis()
            {
                Name = "YAxis",
                AxisPlacement = AxisPlacement.DefaultY,
            };

            AxisGrid = new AxisGrid(this);
            Navigation = new ChartNavigation(this);

            Padding = new Thickness(10);
            // This can cause a grid line to not display, depending on its exact coordinates
            // SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        static ChartContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartContainer), new FrameworkPropertyMetadata(typeof(ChartContainer)));
        }
        #endregion

        /************************************************************************/

        #region Orientation
        /// <summary>
        /// Gets or sets the orientation of the chart
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register
            (
                nameof(Orientation), typeof(Orientation), typeof(ChartContainer), new PropertyMetadata(DefaultOrientation, OnOrientationChanged)
            );

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
            }
        }
        #endregion

        /************************************************************************/

        #region Title
        /// <summary>
        /// Gets or sets the chart top title.
        /// Chart title is an object that is shown centered above plot area.
        /// It is used inside <see cref="ContentControl"/> and can be a <see cref="UIElement"/>.
        /// </summary>
        public object TopTitle
        {
            get => GetValue(TopTitleProperty);
            set => SetValue(TopTitleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TopTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TopTitleProperty = DependencyProperty.Register
            (
                nameof(TopTitle), typeof(object), typeof(ChartContainer), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region BottomTitle
        /// <summary>
        /// Gets or sets the bottom axis title.
        /// Bottom axis title is an object that is centered under plot area.
        /// It is used inside <see cref="ContentControl"/> and can be a <see cref="UIElement"/>.
        /// </summary>
        public object BottomTitle
        {
            get => GetValue(BottomTitleProperty);
            set => SetValue(BottomTitleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BottomTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BottomTitleProperty = DependencyProperty.Register
            (
                nameof(BottomTitle), typeof(object), typeof(ChartContainer), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region RightTitle
        /// <summary>
        /// Gets or sets right axis title.
        /// Right axis title is an object that is vertically centered and located to the right of the plot area.
        /// It is used inside <see cref="ContentControl"/> and can be a <see cref="UIElement"/>.</summary>
        public object RightTitle
        {
            get => GetValue(RightTitleProperty);
            set => SetValue(RightTitleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="RightTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RightTitleProperty = DependencyProperty.Register
            (
                nameof(RightTitle), typeof(object), typeof(ChartContainer), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region LeftTitle
        /// <summary>
        /// Gets or sets the left axis title.
        /// Left axis title is an object that is vertically centered and located to the left of the plot area.
        /// It is used inside <see cref="ContentControl"/> and can be a  <see cref="UIElement"/>.
        /// </summary>
        public object LeftTitle
        {
            get => GetValue(LeftTitleProperty);
            set => SetValue(LeftTitleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="LeftTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LeftTitleProperty = DependencyProperty.Register
            (
                nameof(LeftTitle), typeof(object), typeof(ChartContainer), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region XAxis
        /// <summary>
        /// Gets the x-axis object.
        /// </summary>
        public Axis XAxis
        {
            get => (Axis)GetValue(XAxisProperty);
            private set => SetValue(XAxisPropertyKey, value);
        }

        private static readonly DependencyPropertyKey XAxisPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(XAxis), typeof(Axis), typeof(ChartContainer), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="XAxis"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisProperty = XAxisPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the x-axis placement. This property is clamped to Bottom or Top.
        /// The default is Bottom.
        /// </summary>
        public AxisPlacement XAxisPlacement
        {
            get => (AxisPlacement)GetValue(XAxisPlacementProperty);
            set => SetValue(XAxisPlacementProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="XAxisPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisPlacementProperty = DependencyProperty.Register
            (
                nameof(XAxisPlacement), typeof(AxisPlacement), typeof(ChartContainer), new PropertyMetadata(AxisPlacement.DefaultX, OnXAxisPropertyChanged, OnCoerceXPlacement)
            );

        private static object OnCoerceXPlacement(DependencyObject d, object value)
        {
            AxisPlacement proposed = (AxisPlacement)value;
            switch(proposed)
            {
                case AxisPlacement.Bottom:
                case AxisPlacement.Top:
                    return proposed;
                default:
                    return AxisPlacement.DefaultX;
            }
        }

        /// <summary>
        /// Gets or sets a converter for tick values on the X axis.
        /// </summary>
        public IDoubleConverter XAxisTextProvider
        {
            get => (IDoubleConverter)GetValue(XAxisTextProviderProperty);
            set => SetValue(XAxisTextProviderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="XAxisTextProvider"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisTextProviderProperty = DependencyProperty.Register
            (
                nameof(XAxisTextProvider), typeof(IDoubleConverter), typeof(ChartContainer), new PropertyMetadata(null, OnXAxisPropertyChanged)
            );

        /// <summary>
        /// Gets or sets the format to use for text on the X axis.
        /// This will be passed to <see cref="XAxisTextProvider"/> if present.
        /// </summary>
        public string XAxisTextFormat
        {
            get => (string)GetValue(XAxisTextFormatProperty);
            set => SetValue(XAxisTextFormatProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="XAxisTextFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisTextFormatProperty = DependencyProperty.Register
            (
                nameof(XAxisTextFormat), typeof(string), typeof(ChartContainer), new PropertyMetadata(null, OnXAxisPropertyChanged)
            );

        /// <summary>
        /// Gets or sets tick visibility for the X axis.
        /// </summary>
        public TickVisibility XAxisTickVisibility
        {
            get => (TickVisibility)GetValue(XAxisTickVisibilityProperty);
            set => SetValue(XAxisTickVisibilityProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="XAxisTickVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisTickVisibilityProperty = DependencyProperty.Register
            (
                nameof(XAxisTickVisibility), typeof(TickVisibility), typeof(ChartContainer), new PropertyMetadata(TickVisibility.Default, OnXAxisPropertyChanged)
            );

        /// <summary>
        /// Gets or sets a value that determines if the X axis is reversed
        /// </summary>
        public bool IsXAxisReversed
        {
            get => (bool)GetValue(IsXAxisReversedProperty);
            set => SetValue(IsXAxisReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsXAxisReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsXAxisReversedProperty = DependencyProperty.Register
            (
                nameof(IsXAxisReversed), typeof(bool), typeof(ChartContainer), new PropertyMetadata(false, OnXAxisPropertyChanged)
            );

        private static void OnXAxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                switch (e.Property.Name)
                {
                    case nameof(XAxisPlacement):
                        c.XAxis.AxisPlacement = c.XAxisPlacement;
                        break;
                    case nameof(XAxisTextProvider):
                        c.XAxis.TickProvider.TextProvider = c.XAxisTextProvider;
                        c.XAxis.InvalidateMeasure();
                        break;
                    case nameof(XAxisTextFormat):
                        c.XAxis.TickProvider.TextFormat = c.XAxisTextFormat;
                        c.XAxis.InvalidateMeasure();
                        break;
                    case nameof(XAxisTickVisibility):
                        c.XAxis.TickVisibility = c.XAxisTickVisibility;
                        break;
                    case nameof(IsXAxisReversed):
                        c.XAxis.IsReversed = c.IsXAxisReversed;
                        c.InvalidateChart();
                        break;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region YAxis
        /// <summary>
        /// Gets the y-axis object.
        /// </summary>
        public Axis YAxis
        {
            get => (Axis)GetValue(YAxisProperty);
            private set => SetValue(YAxisPropertyKey, value);
        }
        
        private static readonly DependencyPropertyKey YAxisPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(YAxis), typeof(Axis), typeof(ChartContainer), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="YAxis"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YAxisProperty = YAxisPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the y-axis placement. This property is clamped to Left or Right.
        /// The default is Left.
        /// </summary>
        public AxisPlacement YAxisPlacement
        {
            get => (AxisPlacement)GetValue(YAxisPlacementProperty);
            set => SetValue(YAxisPlacementProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="YAxisPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YAxisPlacementProperty = DependencyProperty.Register
            (
                nameof(YAxisPlacement), typeof(AxisPlacement), typeof(ChartContainer), new PropertyMetadata(AxisPlacement.DefaultY, OnYAxisPropertyChanged, OnCoerceYPlacement)
            );

        private static object OnCoerceYPlacement(DependencyObject d, object value)
        {
            AxisPlacement proposed = (AxisPlacement)value;
            switch (proposed)
            {
                case AxisPlacement.Left:
                case AxisPlacement.Right:
                    return proposed;
                default:
                    return AxisPlacement.DefaultY;
            }
        }

        /// <summary>
        /// Gets or sets a converter for tick values on the Y axis.
        /// </summary>
        public IDoubleConverter YAxisTextProvider
        {
            get => (IDoubleConverter)GetValue(YAxisTextProviderProperty);
            set => SetValue(YAxisTextProviderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="YAxisTextProvider"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YAxisTextProviderProperty = DependencyProperty.Register
            (
                nameof(YAxisTextProvider), typeof(IDoubleConverter), typeof(ChartContainer), new PropertyMetadata(null, OnYAxisPropertyChanged)
            );

        /// <summary>
        /// Gets or sets the format to use for text on the Y axis.
        /// This will be passed to <see cref="YAxisTextProvider"/> if present.
        /// </summary>
        public string YAxisTextFormat
        {
            get => (string)GetValue(YAxisTextFormatProperty);
            set => SetValue(YAxisTextFormatProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="YAxisTextFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YAxisTextFormatProperty = DependencyProperty.Register
            (
                nameof(YAxisTextFormat), typeof(string), typeof(ChartContainer), new PropertyMetadata(null, OnYAxisPropertyChanged)
            );

        /// <summary>
        /// Gets or sets tick visibility for the Y axis.
        /// </summary>
        public TickVisibility YAxisTickVisibility
        {
            get => (TickVisibility)GetValue(YAxisTickVisibilityProperty);
            set => SetValue(YAxisTickVisibilityProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="YAxisTickVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YAxisTickVisibilityProperty = DependencyProperty.Register
            (
                nameof(YAxisTickVisibility), typeof(TickVisibility), typeof(ChartContainer), new PropertyMetadata(TickVisibility.Default, OnYAxisPropertyChanged)
            );

        /// <summary>
        /// Gets or sets a value that determines if the Y axis is reversed
        /// </summary>
        public bool IsYAxisReversed
        {
            get => (bool)GetValue(IsYAxisReversedProperty);
            set => SetValue(IsYAxisReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsYAxisReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsYAxisReversedProperty = DependencyProperty.Register
            (
                nameof(IsYAxisReversed), typeof(bool), typeof(ChartContainer), new PropertyMetadata(false, OnYAxisPropertyChanged)
            );

        private static void OnYAxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                switch (e.Property.Name)
                {
                    case nameof(YAxisPlacement):
                        c.YAxis.AxisPlacement = c.YAxisPlacement;
                        break;
                    case nameof(YAxisTextProvider):
                        c.YAxis.TickProvider.TextProvider = c.YAxisTextProvider;
                        c.YAxis.InvalidateMeasure();
                        break;
                    case nameof(YAxisTextFormat):
                        c.YAxis.TickProvider.TextFormat = c.YAxisTextFormat;
                        c.YAxis.InvalidateMeasure();
                        break;
                    case nameof(YAxisTickVisibility):
                        c.YAxis.TickVisibility = c.YAxisTickVisibility;
                        break;
                    case nameof(IsYAxisReversed):
                        c.YAxis.IsReversed = c.IsYAxisReversed;
                        c.InvalidateChart();
                        break;
                }
            }
        }



        #endregion

        /************************************************************************/

        #region Brushes
        /// <summary>
        /// Gets or sets the brush that is used to draw major ticks and their corresponding labels.
        /// </summary>
        public Brush MajorTickBrush
        {
            get => (Brush)GetValue(MajorTickBrushProperty);
            set => SetValue(MajorTickBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MajorTickBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MajorTickBrushProperty = DependencyProperty.Register
            (
                nameof(MajorTickBrush), typeof(Brush), typeof(ChartContainer), new PropertyMetadata(Axis.DefaultMajorTickBrush, OnBrushChanged)
            );

        /// <summary>
        /// Gets or sets the brush that is used to draw minor ticks.
        /// </summary
        public Brush MinorTickBrush
        {
            get => (Brush)GetValue(MinorTickBrushProperty);
            set => SetValue(MinorTickBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MinorTickBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorTickBrushProperty = DependencyProperty.Register
            (
                nameof(MinorTickBrush), typeof(Brush), typeof(ChartContainer), new PropertyMetadata(Axis.DefaultMinorTickBrush, OnBrushChanged)
            );

        /// <summary>
        /// Gets or sets a brush that determines how the grid lines of <see cref="AxisGrid"/> are drawn.
        /// </summary>
        public Brush GridBrush
        {
            get => (Brush)GetValue(AxisGridBrushProperty);
            set => SetValue(AxisGridBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="GridBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisGridBrushProperty = DependencyProperty.Register
            (
                nameof(GridBrush), typeof(Brush), typeof(ChartContainer), new PropertyMetadata(AxisGrid.DefaultGridBrush, OnBrushChanged)
            );

        /// <summary>
        /// Gets or sets a brush that determines how the border of <see cref="AxisGrid"/> is drawn.
        /// </summary>
        public Brush GridBorderBrush
        {
            get => (Brush)GetValue(GridBorderBrushProperty);
            set => SetValue(GridBorderBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="GridBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GridBorderBrushProperty = DependencyProperty.Register
            (
                nameof(GridBorderBrush), typeof(Brush), typeof(ChartContainer), new PropertyMetadata(AxisGrid.DefaultBorderBrush)
            );

        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                switch (e.Property.Name)
                {
                    case nameof(MajorTickBrush):
                        c.XAxis.MajorTickBrush = c.MajorTickBrush;
                        c.YAxis.MajorTickBrush = c.MajorTickBrush;
                        break;
                    case nameof(MinorTickBrush):
                        c.XAxis.MinorTickBrush = c.MinorTickBrush;
                        c.YAxis.MinorTickBrush = c.MinorTickBrush;
                        break;
                    case nameof(GridBrush):
                        c.AxisGrid.GridBrush = c.GridBrush;
                        break;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region GridBorderSize
        /// <summary>
        /// Gets or sets the thickness of the border that surrounds the chart grid lines.
        /// This value is clamped between 0 and 4 inclusive. The default is 1.0
        /// </summary>
        public double GridBorderSize
        {
            get => (double)GetValue(GridBorderSizeProperty);
            set => SetValue(GridBorderSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="GridBorderSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GridBorderSizeProperty = DependencyProperty.Register
            (
                nameof(GridBorderSize), typeof(double), typeof(ChartContainer), new PropertyMetadata(DefaultGridBorderSize, OnGridBorderSizeChanged, OnCoerceGridBorderSize)
            );

        private static object OnCoerceGridBorderSize(DependencyObject d, object value)
        {
            return ((double)value).Clamp(0, 4);
        }

        private static void OnGridBorderSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                c.GridBorderThickness = new Thickness(c.GridBorderSize);
            }
        }

        /// <summary>
        /// Gets the grid border thickness
        /// </summary>
        public Thickness GridBorderThickness
        {
            get => (Thickness)GetValue(GridBorderThicknessProperty);
            private set => SetValue(GridBorderThicknessPropertyKey, value);
        }
        
        private static readonly DependencyPropertyKey GridBorderThicknessPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(GridBorderThickness), typeof(Thickness), typeof(ChartContainer), new PropertyMetadata(new Thickness(DefaultGridBorderSize))
            );

        /// <summary>
        /// Identifies the <see cref="GridBorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GridBorderThicknessProperty = GridBorderThicknessPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region AxisGrid (read only)
        /// <summary>
        /// Gets the AxisGrid for this chart
        /// </summary>
        public AxisGrid AxisGrid
        {
            get => (AxisGrid)GetValue(AxisGridProperty);
            private set => SetValue(AxisGridPropertyKey, value);
        }

        private static readonly DependencyPropertyKey AxisGridPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(AxisGrid), typeof(AxisGrid), typeof(ChartContainer), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="AxisGrid"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisGridProperty = AxisGridPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Navigation (read only)
        /// <summary>
        /// Gets the chart navigation object.
        /// </summary>
        public ChartNavigation Navigation
        {
            get => (ChartNavigation)GetValue(NavigationProperty);
            private set => SetValue(NavigationPropertyKey, value);
        }
        
        private static readonly DependencyPropertyKey NavigationPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(Navigation), typeof(ChartNavigation), typeof(ChartContainer), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="Navigation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NavigationProperty = NavigationPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Causes the chart (if one is inside the container) to redraw itself.
        /// This method is called from property change handlers to allow the 
        /// embedded chart to update itself.
        /// </summary>
        private void InvalidateChart()
        {
            if (Content is UIElement element)
            {
                element.InvalidateMeasure();
            }
        }
        #endregion
    }
}
