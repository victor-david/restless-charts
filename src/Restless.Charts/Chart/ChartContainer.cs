using System.Diagnostics;
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
            ClipToBounds = true;

            // For best results, these two should be used together.
            UseLayoutRounding = true;
            SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        static ChartContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartContainer), new FrameworkPropertyMetadata(typeof(ChartContainer)));
        }
        #endregion

        /************************************************************************/

        #region Orientation
        /// <summary>
        /// Gets or sets the orientation of the chart. This is a dependency property
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
                switch (c.Orientation)
                {
                    case Orientation.Vertical:
                        c.XAxisPlacement = c.IsXAxisPlacementReversed ? AxisPlacement.Top : AxisPlacement.Bottom;
                        c.YAxisPlacement = c.IsYAxisPlacementReversed ? AxisPlacement.Right : AxisPlacement.Left;
                        break;
                    case Orientation.Horizontal:
                        c.XAxisPlacement = c.IsXAxisPlacementReversed ? AxisPlacement.Right : AxisPlacement.Left;
                        c.YAxisPlacement = c.IsYAxisPlacementReversed ? AxisPlacement.Top : AxisPlacement.Bottom;
                        break;
                }
                c.InvalidateMeasure();
            }
        }
        #endregion

        /************************************************************************/

        #region Title
        /// <summary>
        /// Gets or sets the chart top title.
        /// Chart title is an object that is shown centered above plot area.
        /// It is used inside <see cref="ContentControl"/> and can be a <see cref="UIElement"/>.
        /// This is a dependency property.
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
        /// This is a dependency property.
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
        /// It is used inside <see cref="ContentControl"/> and can be a <see cref="UIElement"/>.
        /// This is a dependency property.
        /// </summary>
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
        /// This is a dependency property.
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
        /// This is a read only dependency property.
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
        /// Gets or sets a value that determines if the placement of the X axis is reversed
        /// from its default placement position. When <see cref="Orientation"/> is vertical
        /// (the default), toggling this property changes the X axis from top to bottom.
        /// When <see cref="Orientation"/> is horizontal, this property changes the X axis
        /// from left to right.
        /// </summary>
        public bool IsXAxisPlacementReversed
        {
            get => (bool)GetValue(IsXAxisPlacementReversedProperty);
            set => SetValue(IsXAxisPlacementReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsXAxisPlacementReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsXAxisPlacementReversedProperty = DependencyProperty.Register
            (
                nameof(IsXAxisPlacementReversed), typeof(bool), typeof(ChartContainer), new PropertyMetadata(false, OnXAxisPropertyChanged)
            );

        /// <summary>
        /// Gets the X axis placement. The value of this property depends on the value of <see cref="Orientation"/>.
        /// When <see cref="Orientation"/> is vertical (the default), this property can be Top or Bottom.
        /// When <see cref="Orientation"/> is horizontal, this property can be Left or Right.
        /// </summary>
        public AxisPlacement XAxisPlacement
        {
            get => (AxisPlacement)GetValue(XAxisPlacementProperty);
            private set => SetValue(XAxisPlacementPropertyKey, value);
        }

        /// <summary>
        /// Identifies the <see cref="XAxisPlacement"/> dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey XAxisPlacementPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(XAxisPlacement), typeof(AxisPlacement), typeof(ChartContainer), new PropertyMetadata(AxisPlacement.DefaultX, OnXAxisPropertyChanged)
            );

        /// <summary>
        /// Identifies the <see cref="XAxisPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisPlacementProperty = XAxisPlacementPropertyKey.DependencyProperty;

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
        /// Gets or sets a value that determines if the values on the X axis are reversed.
        /// </summary>
        public bool IsXAxisValueReversed
        {
            get => (bool)GetValue(IsXAxisValueReversedProperty);
            set => SetValue(IsXAxisValueReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsXAxisValueReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsXAxisValueReversedProperty = DependencyProperty.Register
            (
                nameof(IsXAxisValueReversed), typeof(bool), typeof(ChartContainer), new PropertyMetadata(false, OnXAxisPropertyChanged)
            );

        private static void OnXAxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                switch (e.Property.Name)
                {
                    case nameof(IsXAxisPlacementReversed):
                        if (c.Orientation == Orientation.Vertical)
                        {
                            c.XAxisPlacement = c.IsXAxisPlacementReversed ? AxisPlacement.Top : AxisPlacement.Bottom;
                        }
                        else
                        {
                            c.XAxisPlacement = c.IsXAxisPlacementReversed ? AxisPlacement.Right : AxisPlacement.Left;
                        }
                        break;
                    case nameof(XAxisPlacement):
                        c.XAxis.AxisPlacement = c.XAxisPlacement;
                        // c.XAxis.InvalidateMeasure();
                        break;
                    case nameof(XAxisTextProvider):
                        c.XAxis.TickProvider.TextProvider = c.XAxisTextProvider;
                        c.InvalidateMeasure();
                        break;
                    case nameof(XAxisTextFormat):
                        c.XAxis.TickProvider.TextFormat = c.XAxisTextFormat;
                        c.InvalidateMeasure();
                        break;
                    case nameof(XAxisTickVisibility):
                        c.XAxis.TickVisibility = c.XAxisTickVisibility;
                        break;
                    case nameof(IsXAxisValueReversed):
                        c.XAxis.IsValueReversed = c.IsXAxisValueReversed;
                        c.InvalidateMeasure();
                        break;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region YAxis
        /// <summary>
        /// Gets the y-axis object.
        /// This is a read only dependency property.
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
        /// Gets or sets a value that determines if the placement of the Y axis is reversed
        /// from its default placement position. When <see cref="Orientation"/> is vertical
        /// (the default), toggling this property changes the Y axis from left to right.
        /// When <see cref="Orientation"/> is horizontal, this property changes the Y axis
        /// from top to bottom.
        /// </summary>
        public bool IsYAxisPlacementReversed
        {
            get => (bool)GetValue(IsYAxisPlacementReversedProperty);
            set => SetValue(IsYAxisPlacementReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsYAxisPlacementReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsYAxisPlacementReversedProperty = DependencyProperty.Register
            (
                nameof(IsYAxisPlacementReversed), typeof(bool), typeof(ChartContainer), new PropertyMetadata(false, OnYAxisPropertyChanged)
            );

        /// <summary>
        /// Gets the Y axis placement. The value of this property depends on the value of <see cref="Orientation"/>.
        /// When <see cref="Orientation"/> is vertical (the default), this property can be Left or Right.
        /// When <see cref="Orientation"/> is horizontal, this property can be Top or Bottom.
        /// </summary>
        public AxisPlacement YAxisPlacement
        {
            get => (AxisPlacement)GetValue(YAxisPlacementProperty);
            private set => SetValue(YAxisPlacementPropertyKey, value);
        }

        private static readonly DependencyPropertyKey YAxisPlacementPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(YAxisPlacement), typeof(AxisPlacement), typeof(ChartContainer), new PropertyMetadata(AxisPlacement.DefaultY, OnYAxisPropertyChanged)
            );

        /// <summary>
        /// Identifies the <see cref="YAxisPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YAxisPlacementProperty = YAxisPlacementPropertyKey.DependencyProperty;

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
        /// Gets or sets a value that determines if the values on the Y axis are reversed.
        /// </summary>
        public bool IsYAxisValueReversed
        {
            get => (bool)GetValue(IsYAxisValueReversedProperty);
            set => SetValue(IsYAxisValueReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsYAxisValueReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsYAxisValueReversedProperty = DependencyProperty.Register
            (
                nameof(IsYAxisValueReversed), typeof(bool), typeof(ChartContainer), new PropertyMetadata(false, OnYAxisPropertyChanged)
            );

        private static void OnYAxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                switch (e.Property.Name)
                {
                    case nameof(IsYAxisPlacementReversed):
                        if (c.Orientation == Orientation.Vertical)
                        {
                            c.YAxisPlacement = c.IsYAxisPlacementReversed ? AxisPlacement.Right : AxisPlacement.Left;
                        }
                        else
                        {
                            c.YAxisPlacement = c.IsYAxisPlacementReversed ? AxisPlacement.Top : AxisPlacement.Bottom;
                        }
                        break;
                    case nameof(YAxisPlacement):
                        c.YAxis.AxisPlacement = c.YAxisPlacement;
                        break;
                    case nameof(YAxisTextProvider):
                        c.YAxis.TickProvider.TextProvider = c.YAxisTextProvider;
                        c.InvalidateMeasure();
                        break;
                    case nameof(YAxisTextFormat):
                        c.YAxis.TickProvider.TextFormat = c.YAxisTextFormat;
                        c.InvalidateMeasure();
                        break;
                    case nameof(YAxisTickVisibility):
                        c.YAxis.TickVisibility = c.YAxisTickVisibility;
                        break;
                    case nameof(IsYAxisValueReversed):
                        c.YAxis.IsValueReversed = c.IsYAxisValueReversed;
                        c.InvalidateMeasure();
                        break;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Brushes
        /// <summary>
        /// Gets or sets the brush that is used to draw major ticks and their corresponding labels.
        /// This is a dependency property.
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
        /// This is a dependency property.
        /// </summary>
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
        /// This is a dependency property.
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
        /// This is a dependency property.
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

        #region IsKeyboardNavigationEnabled
        /// <summary>
        /// Gets or sets a value that determines if chart navigation can be performed using the keyboard.
        /// This is a dependency property.
        /// </summary>
        public bool IsKeyboardNavigationEnabled
        {
            get => (bool)GetValue(IsKeyboardNavigationEnabledProperty);
            set => SetValue(IsKeyboardNavigationEnabledProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsKeyboardNavigationEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsKeyboardNavigationEnabledProperty = DependencyProperty.Register
            (
                nameof(IsKeyboardNavigationEnabled), typeof(bool), typeof(ChartContainer), new PropertyMetadata
                    (
                        ChartNavigation.IsKeyboardNavigationEnabledDefault, OnIsKeyboardNavigationEnabledChanged
                    )
            );

        private static void OnIsKeyboardNavigationEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                c.Navigation.IsKeyboardNavigationEnabled = c.IsKeyboardNavigationEnabled;
            }
        }
        #endregion

        /************************************************************************/

        #region GridBorderSize
        /// <summary>
        /// Gets or sets the thickness of the border that surrounds the chart grid lines.
        /// This value is clamped between 0 and 4 inclusive. The default is 1.0.
        /// This is a dependency property.
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
        /// Gets the grid border thickness.
        /// This is a read only dependency property.
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
        /// Gets the AxisGrid for this chart container.
        /// This is a read only dependency property.
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
        /// Gets the chart container navigation object.
        /// This is a read only dependency property.
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

        #region Protected methods
        /// <summary>
        /// Measures the size in layout required for child elements and determines a size this element.
        /// </summary>
        /// <param name="availableSize">
        /// The available size that this element can give to child elements.
        /// Infinity can be specified as a value to indicate that the element will size to whatever content is available.
        /// </param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            XAxis.Measure(availableSize);
            YAxis.Measure(availableSize);
            AxisGrid.Measure(availableSize);
            if (Content is UIElement element)
            {
                element.Measure(availableSize);
            }
            return base.MeasureOverride(availableSize);
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Zooms in (both X and Y) by a specified factor (1.0 / 1.2)
        /// </summary>
        public void ZoomIn()
        {
            Zoom(1.0 / 1.2);
        }

        /// <summary>
        /// Zooms out (both X and Y) by a specified factor (1.2)
        /// </summary>
        public void ZoomOut()
        {
            Zoom(1.2);
        }

        /// <summary>
        /// Zooms the chart (both X and Y) by the specified factor.
        /// </summary>
        /// <param name="factor">The factor</param>
        public void Zoom(double factor)
        {
            XAxis.Range.MakeZoomed(factor);
            YAxis.Range.MakeZoomed(factor);
            InvalidateMeasure();
        }

        /// <summary>
        /// Restores the starting size and position of the chart.
        /// </summary>
        public void RestoreSizeAndPosition()
        {
            XAxis.Range.RestoreFromSnapshot();
            YAxis.Range.RestoreFromSnapshot();
            InvalidateMeasure();
        }

        /// <summary>
        /// Pans the chart according to the specified percentages.
        /// </summary>
        /// <param name="xPercentage">The percentage to pan the X axis.</param>
        /// <param name="yPercentage">The percentage to pan the Y axis.</param>
        public void Pan(double xPercentage, double yPercentage)
        {
            if (Orientation == Orientation.Vertical)
            {
                XAxis.Shift(xPercentage);
                YAxis.Shift(yPercentage);
            }
            else
            {
                XAxis.Shift(yPercentage);
                YAxis.Shift(xPercentage);
            }
            InvalidateMeasure();
        }
        #endregion
    }
}
