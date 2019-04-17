using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides a container for chart controls. Provides titles, axis, axis grid and navigation.
    /// </summary>
    public class ChartContainer : ContentControl
    {
        #region Private
        #endregion

        /************************************************************************/

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
            XAxis = new Axis(this, AxisType.X)
            {
                Name = "XAxis",
                AxisPlacement = AxisPlacement.DefaultX,
            };

            YAxis = new Axis(this, AxisType.Y)
            {
                Name = "YAxis",
                AxisPlacement = AxisPlacement.DefaultY,
            };

            AxisGrid = new AxisGrid(this);
            Navigation = new ChartNavigation(this);

            Padding = new Thickness(10);
            ClipToBounds = true;
            SnapsToDevicePixels = true;

            CommandBindings.Add(new CommandBinding(NavigationHelpCommand, ExecuteNavigationHelpCommand));
            CommandBindings.Add(new CommandBinding(LegendHelpCommand, ExecuteLegendHelpCommand));
        }

        static ChartContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartContainer), new FrameworkPropertyMetadata(typeof(ChartContainer)));
            PaddingProperty.OverrideMetadata(typeof(ChartContainer), new FrameworkPropertyMetadata(new Thickness(), FrameworkPropertyMetadataOptions.AffectsParentMeasure, OnPaddingPropertyChanged));
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
                nameof(Orientation), typeof(Orientation), typeof(ChartContainer),
                new FrameworkPropertyMetadata(DefaultOrientation,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnOrientationChanged)
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
                nameof(TopTitle), typeof(object), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange)
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
                nameof(BottomTitle), typeof(object), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange)
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
                nameof(RightTitle), typeof(object), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange)
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
                nameof(LeftTitle), typeof(object), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange)
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
                nameof(IsXAxisPlacementReversed), typeof(bool), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(false, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnXAxisPropertyChanged)
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
                nameof(XAxisPlacement), typeof(AxisPlacement), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(AxisPlacement.DefaultX, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnXAxisPropertyChanged)
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
                nameof(XAxisTextProvider), typeof(IDoubleConverter), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnXAxisPropertyChanged)
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
                nameof(XAxisTextFormat), typeof(string), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(null, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnXAxisPropertyChanged)
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
                nameof(XAxisTickVisibility), typeof(TickVisibility), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(TickVisibility.Default, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnXAxisPropertyChanged)
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
                nameof(IsXAxisValueReversed), typeof(bool), typeof(ChartContainer),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnXAxisPropertyChanged)
            );

        /// <summary>
        /// Gets or sets a value that determines how ticks are aligned on the X axis.
        /// </summary>
        public TickAlignment XAxisTickAlignment
        {
            get => (TickAlignment)GetValue(XAxisTickAlignmentProperty);
            set => SetValue(XAxisTickAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="XAxisTickAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisTickAlignmentProperty = DependencyProperty.Register
            (
                nameof(XAxisTickAlignment), typeof(TickAlignment), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(TickAlignment.Default, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnXAxisPropertyChanged)
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
                        break;
                    case nameof(XAxisTextProvider):
                        c.XAxis.TextProvider = c.XAxisTextProvider;
                        break;
                    case nameof(XAxisTextFormat):
                        c.XAxis.TextFormat = c.XAxisTextFormat;
                        break;
                    case nameof(XAxisTickVisibility):
                        c.XAxis.TickVisibility = c.XAxisTickVisibility;
                        break;
                    case nameof(IsXAxisValueReversed):
                        c.XAxis.IsValueReversed = c.IsXAxisValueReversed;
                        break;
                    case nameof(XAxisTickAlignment):
                        c.XAxis.TickAlignment = c.XAxisTickAlignment;
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
                nameof(IsYAxisPlacementReversed), typeof(bool), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnYAxisPropertyChanged)
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
                nameof(YAxisPlacement), typeof(AxisPlacement), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(AxisPlacement.DefaultY,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnYAxisPropertyChanged)
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
                nameof(YAxisTextProvider), typeof(IDoubleConverter), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnYAxisPropertyChanged)
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
                nameof(YAxisTextFormat), typeof(string), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnYAxisPropertyChanged)
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
                nameof(YAxisTickVisibility), typeof(TickVisibility), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(TickVisibility.Default,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, 
                    OnYAxisPropertyChanged)
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
                nameof(IsYAxisValueReversed), typeof(bool), typeof(ChartContainer),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnYAxisPropertyChanged)
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
                        c.YAxis.TextProvider = c.YAxisTextProvider;
                        break;
                    case nameof(YAxisTextFormat):
                        c.YAxis.TextFormat = c.YAxisTextFormat;
                        break;
                    case nameof(YAxisTickVisibility):
                        c.YAxis.TickVisibility = c.YAxisTickVisibility;
                        break;
                    case nameof(IsYAxisValueReversed):
                        c.YAxis.IsValueReversed = c.IsYAxisValueReversed;
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

        /// <summary>
        /// Gets or sets a value that determines if the axis grid is displayed.
        /// This is a dependency property.
        /// </summary>
        public bool IsAxisGridVisible
        {
            get => (bool)GetValue(IsAxisGridVisibleProperty);
            set => SetValue(IsAxisGridVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsAxisGridVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsAxisGridVisibleProperty = DependencyProperty.Register
            (
                nameof(IsAxisGridVisible), typeof(bool), typeof(ChartContainer), new PropertyMetadata(true, OnIsAxisGridVisibleChanged)
            );

        private static void OnIsAxisGridVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                c.AxisGrid.IsGridVisible = c.IsAxisGridVisible;
            }
        }
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

        #region IconMargin (read only)
        /// <summary>
        /// Gets the margin to be applied to the icon container.
        /// </summary>
        public Thickness IconMargin
        {
            get => (Thickness)GetValue(IconMarginProperty);
            private set => SetValue(IconMarginPropertyKey, value);
        }

        private static readonly DependencyPropertyKey IconMarginPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(IconMargin), typeof(Thickness), typeof(ChartContainer), new PropertyMetadata(new Thickness())
            );

        /// <summary>
        /// Identifies the <see cref="IconMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconMarginProperty = IconMarginPropertyKey.DependencyProperty;

        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartContainer c)
            {
                c.IconMargin = new Thickness(0, -c.Padding.Top + 3, -c.Padding.Right + 3, 0);
            }
        }
        #endregion

        /************************************************************************/

        #region Navigation Help
        /// <summary>
        /// Gets the command used to display navigation help.
        /// </summary>
        public static readonly RoutedCommand NavigationHelpCommand = new RoutedCommand();

        private void ExecuteNavigationHelpCommand(object sender, ExecutedRoutedEventArgs e)
        {
            IsLegendVisible = false;
            IsNavigationHelpVisible = !IsNavigationHelpVisible;
        }

        /// <summary>
        /// Gets or sets a value that determines if the navigation help button is visible.
        /// </summary>
        public bool IsNavigationHelpButtonVisible
        {
            get => (bool)GetValue(IsNavigationHelpButtonVisibleProperty);
            set => SetValue(IsNavigationHelpButtonVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsNavigationHelpButtonVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNavigationHelpButtonVisibleProperty = DependencyProperty.Register
            (
                nameof(IsNavigationHelpButtonVisible), typeof(bool), typeof(ChartContainer), new PropertyMetadata(true)
            );

        /// <summary>
        /// Gets or sets a value that determines if navigation help is visible.
        /// </summary>
        public bool IsNavigationHelpVisible
        {
            get => (bool)GetValue(IsNavigationHelpVisibleProperty);
            set => SetValue(IsNavigationHelpVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsNavigationHelpVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNavigationHelpVisibleProperty = DependencyProperty.Register
            (
                nameof(IsNavigationHelpVisible), typeof(bool), typeof(ChartContainer), 
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        #endregion

        /************************************************************************/

        #region Legend Help
        /// <summary>
        /// Gets the command used to display the legend.
        /// </summary>
        public static readonly RoutedCommand LegendHelpCommand = new RoutedCommand();

        private void ExecuteLegendHelpCommand(object sender, ExecutedRoutedEventArgs e)
        {
            IsNavigationHelpVisible = false;
            IsLegendVisible = !IsLegendVisible;
        }

        /// <summary>
        /// Gets or sets a value that determines if the legend help button is visible.
        /// </summary>
        public bool IsLegendHelpButtonVisible
        {
            get => (bool)GetValue(IsLegendHelpButtonVisibleProperty);
            set => SetValue(IsLegendHelpButtonVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsLegendHelpButtonVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLegendHelpButtonVisibleProperty = DependencyProperty.Register
            (
                nameof(IsLegendHelpButtonVisible), typeof(bool), typeof(ChartContainer), new PropertyMetadata(true)
            );

        /// <summary>
        /// Gets or sets a value that determines if the legend is visible.
        /// </summary>
        public bool IsLegendVisible
        {
            get => (bool)GetValue(IsLegendVisibleProperty);
            set => SetValue(IsLegendVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsLegendVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLegendVisibleProperty = DependencyProperty.Register
            (
                nameof(IsLegendVisible), typeof(bool), typeof(ChartContainer),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        #endregion

        /************************************************************************/

        #region LegendContent (read only)
        /// <summary>
        /// Gets the legend content.
        /// </summary>
        public UIElement LegendContent
        {
            get => (UIElement)GetValue(LegendContentProperty);
            private set => SetValue(LegendContentPropertyKey, value);
        }
        
        private static readonly DependencyPropertyKey LegendContentPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(LegendContent), typeof(UIElement), typeof(ChartContainer), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="LegendContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendContentProperty = LegendContentPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Chart
        /// <summary>
        /// Gets the chart that is currently assigned to the content of this container, or null.
        /// </summary>
        public ChartBase Chart
        {
            get => Content as ChartBase;
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Measures the size in layout required for child elements and determines a size this element.
        /// </summary>
        /// <param name="constraint">
        /// The available size that this element can give to child elements.
        /// Infinity can be specified as a value to indicate that the element will size to whatever content is available.
        /// </param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            XAxis.Measure(constraint);
            YAxis.Measure(constraint);
            AxisGrid.Measure(constraint);
            Navigation.Measure(constraint);

            if (Content is UIElement element)
            {
                element.Measure(constraint);
            }

            return base.MeasureOverride(constraint);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Creates the legend element. This method is called by <see cref="ChartBase"/>
        /// when it receives data.
        /// </summary>
        /// <param name="data">The data.</param>
        internal void CreateLegend(DataSeries data)
        {
            double size = 24;

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(size, GridUnitType.Pixel)
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            int rowIdx = 0;
            foreach (DataSeriesInfo info in data.DataInfo)
            {
                grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(size, GridUnitType.Pixel)
                });
                Border b = new Border()
                {
                    Background = info.DataBrush,
                    Margin = new Thickness(2),
                    CornerRadius = new CornerRadius(2)
                };
                b.SetValue(Grid.RowProperty, rowIdx);
                grid.Children.Add(b);

                TextBlock text = new TextBlock()
                {
                    Text = info.Name,
                    Margin = new Thickness(2),
                    VerticalAlignment = VerticalAlignment.Center,
                };
                text.SetValue(Grid.RowProperty, rowIdx);
                text.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(text);
                rowIdx++;
            }
            LegendContent = grid;
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

        /// <summary>
        /// Gets the specified value formatted according to the settings of <see cref="XAxisTextProvider"/> abd <see cref="XAxisTextFormat"/>.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted value.</returns>
        public string GetFormattedXValue(double value)
        {
            if (XAxisTextProvider != null)
            {
                return XAxisTextProvider.Convert(value, XAxisTextFormat);
            }
            return value.ToString(XAxisTextFormat);
        }

        /// <summary>
        /// Gets the specified value formatted according to the settings of <see cref="YAxisTextProvider"/> abd <see cref="YAxisTextFormat"/>.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted value.</returns>
        public string GetFormattedYValue(double value)
        {
            if (YAxisTextProvider != null)
            {
                return YAxisTextProvider.Convert(value, YAxisTextFormat);
            }
            return value.ToString(YAxisTextFormat);
        }
        #endregion

    }
}
