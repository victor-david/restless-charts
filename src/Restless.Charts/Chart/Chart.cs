using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    public class Chart : ContentControl
    {
        #region Public fields
        /// <summary>
        /// Gets the default orientation
        /// </summary>
        public const Orientation DefaultOrientation = Orientation.Vertical;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Chart"/> class.
        /// </summary>
        public Chart()
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

            AxisGrid = new AxisGrid(XAxis, YAxis);

            Padding = new Thickness(10);
            SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
        }
        #endregion

        /************************************************************************/

        #region Data / DataRange
        /// <summary>
        /// Gets or sets the data for this chart
        /// </summary>
        public DataRange DataRange
        {
            get => (DataRange)GetValue(DataRangeProperty);
            set => SetValue(DataRangeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DataRange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataRangeProperty = DependencyProperty.Register
            (
                nameof(DataRange), typeof(DataRange), typeof(Chart), new PropertyMetadata(null, OnDataRangeChanged)
            );

        private static void OnDataRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Chart c && c.DataRange != null)
            {
                c.XAxis.Range = c.DataRange.X;
                c.YAxis.Range = c.DataRange.Y;
            }
        }

        /// <summary>
        /// Gets or sets the data series
        /// </summary>
        public DataSeries Data
        {
            get => (DataSeries)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Data"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register
            (
                nameof(Data), typeof(DataSeries), typeof(Chart), new PropertyMetadata(null, OnDataChanged)
            );

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Chart c && c.Data != null)
            {
                c.XAxis.Range = c.Data.DataRange.X;
                c.YAxis.Range = c.Data.DataRange.Y;
            }
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
                nameof(Orientation), typeof(Orientation), typeof(Chart), new PropertyMetadata(DefaultOrientation, OnOrientationChanged)
            );

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Chart c)
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
                nameof(TopTitle), typeof(object), typeof(Chart), new PropertyMetadata(null)
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
                nameof(BottomTitle), typeof(object), typeof(Chart), new PropertyMetadata(null)
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
                nameof(RightTitle), typeof(object), typeof(Chart), new PropertyMetadata(null)
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
                nameof(LeftTitle), typeof(object), typeof(Chart), new PropertyMetadata(null)
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
                nameof(XAxis), typeof(Axis), typeof(Chart), new PropertyMetadata(null)
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
                nameof(XAxisPlacement), typeof(AxisPlacement), typeof(Chart), new PropertyMetadata(AxisPlacement.DefaultX, OnXAxisPropertyChanged, OnCoerceXPlacement)
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
                nameof(XAxisTextProvider), typeof(IDoubleConverter), typeof(Chart), new PropertyMetadata(null, OnXAxisPropertyChanged)
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
                nameof(XAxisTextFormat), typeof(string), typeof(Chart), new PropertyMetadata(null, OnXAxisPropertyChanged)
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
                nameof(XAxisTickVisibility), typeof(TickVisibility), typeof(Chart), new PropertyMetadata(TickVisibility.Default, OnXAxisPropertyChanged)
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
                nameof(IsXAxisReversed), typeof(bool), typeof(Chart), new PropertyMetadata(false, OnXAxisPropertyChanged)
            );

        private static void OnXAxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Chart c)
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
                nameof(YAxis), typeof(Axis), typeof(Chart), new PropertyMetadata(null)
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
                nameof(YAxisPlacement), typeof(AxisPlacement), typeof(Chart), new PropertyMetadata(AxisPlacement.DefaultY, OnYAxisPropertyChanged, OnCoerceYPlacement)
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
                nameof(YAxisTextProvider), typeof(IDoubleConverter), typeof(Chart), new PropertyMetadata(null, OnYAxisPropertyChanged)
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
                nameof(YAxisTextFormat), typeof(string), typeof(Chart), new PropertyMetadata(null, OnYAxisPropertyChanged)
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
                nameof(YAxisTickVisibility), typeof(TickVisibility), typeof(Chart), new PropertyMetadata(TickVisibility.Default, OnYAxisPropertyChanged)
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
                nameof(IsYAxisReversed), typeof(bool), typeof(Chart), new PropertyMetadata(false, OnYAxisPropertyChanged)
            );



        private static void OnYAxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Chart c)
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
                nameof(MajorTickBrush), typeof(Brush), typeof(Chart), new PropertyMetadata(Axis.MajorTickDefaultBrush, OnBrushChanged)
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
                nameof(MinorTickBrush), typeof(Brush), typeof(Chart), new PropertyMetadata(Axis.MinorTickDefaultBrush, OnBrushChanged)
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
                nameof(GridBrush), typeof(Brush), typeof(Chart), new PropertyMetadata(AxisGrid.GridDefaultBrush, OnBrushChanged)
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
                nameof(GridBorderBrush), typeof(Brush), typeof(Chart), new PropertyMetadata(AxisGrid.GridBorderDefaultBrush, OnBrushChanged)
            );

        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Chart c)
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
                    case nameof(GridBorderBrush):
                        c.AxisGrid.GridBorderBrush = c.GridBorderBrush;
                        break;
                }
            }
        }
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
                nameof(AxisGrid), typeof(AxisGrid), typeof(Chart), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="AxisGrid"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisGridProperty = AxisGridPropertyKey.DependencyProperty;
        #endregion

    }
}
