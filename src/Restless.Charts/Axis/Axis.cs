using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents an axis on a chart.
    /// </summary>
    public class Axis : Panel
    {
        #region Private
        private const double IncreaseRatio = 8.0;
        private const double DecreaseRatio = 8.0;
        // LabelGap is the distance between the tick mark and the label
        // and between the label and the edge of the control.
        // LabelGapHorz is used when the axis is horizontal, LabelGapVert when vertical.
        private const double LabelGapHorz = 0.0;
        private const double LabelGapVert = 3.0;

        private readonly ChartContainer owner;
        private Path majorTickPath;
        private Path minorTickPath;
        private DataTransform dataTransform;
        private DataSeries data;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default brush for major ticks and their corresponding labels.
        /// </summary>
        public static readonly Brush DefaultMajorTickBrush = Brushes.DarkGray;

        /// <summary>
        /// Gets the default brush for minor ticks.
        /// </summary>
        public static readonly Brush DefaultMinorTickBrush = Brushes.LightGray;

        /// <summary>
        /// Gets the minimum allowed value for <see cref="MaxTickCount"/>.
        /// </summary>
        public const long MinMaxTickCount = 10;

        /// <summary>
        /// Gets the maximum allowed value for <see cref="MaxTickCount"/>.
        /// </summary>
        public const long MaxMaxTickCount = 24;

        /// <summary>
        /// Gets the default value for <see cref="MaxTickCount"/>.
        /// </summary>
        public const long DefaultMaxTickCount = 20;

        /// <summary>
        /// Gets the minimum allowed value for <see cref="TickLength"/>.
        /// </summary>
        public const int MinTickLength = 10;

        /// <summary>
        /// Gets the maximum allowed value for <see cref="TickLength"/>.
        /// </summary>
        public const int MaxTickLength = 24;

        /// <summary>
        /// Gets the default value for <see cref="TickLength"/>.
        /// </summary>
        public const int DefaultTickLength = 12;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="Axis"/> class.
        /// </summary>
        internal Axis(ChartContainer owner, AxisType axisType)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            AxisType = axisType;
            MajorTicks = new MajorTickCollection();
            MinorTicks = new MinorTickCollection();

            Range = Range.EmptyRange();

            dataTransform = new IdentityDataTransform();

            majorTickPath = new Path()
            {
                Stroke = DefaultMajorTickBrush,
                StrokeThickness = 1.0,
            };

            minorTickPath = new Path()
            {
                Stroke = DefaultMinorTickBrush,
                StrokeThickness = 1.0,
            };

            Children.Add(majorTickPath);
            Children.Add(minorTickPath);
        }
        #endregion

        /************************************************************************/

        #region Brushes
        /// <summary>
        /// Gets or sets the brush for major ticks and their corresponding labels.
        /// </summary>
        public Brush MajorTickBrush
        {
            get => majorTickPath.Stroke;
            internal set => majorTickPath.Stroke = value;
        }

        /// <summary>
        /// Gets or sets the brush for minor ticks.
        /// </summary>
        public Brush MinorTickBrush
        {
            get => minorTickPath.Stroke;
            internal set => minorTickPath.Stroke = value;
        }
        #endregion

        /************************************************************************/

        #region Tick Collections
        /// <summary>
        /// Gets the collection of <see cref="MajorTick"/> objects.
        /// </summary>
        public MajorTickCollection MajorTicks
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="MinorTick"/> objects.
        /// </summary>
        public MinorTickCollection MinorTicks
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region TickLength / MaxTickCount
        /// <summary>
        /// Gets or sets the length of the major ticks. Minor ticks are half this size.
        /// This value is clamped between <see cref="MinTickLength"/> and <see cref="MaxTickLength"/> inclusive.
        /// </summary>
        public int TickLength
        {
            get => (int)GetValue(TickLengthProperty);
            set => SetValue(TickLengthProperty, value);
        }

        /// <summary>
        /// Identifes the <see cref="TickLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TickLengthProperty = DependencyProperty.Register
            (
                nameof(TickLength), typeof(int), typeof(Axis),
                new FrameworkPropertyMetadata(DefaultTickLength,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender, null, OnCoerceTickLength)
            );

        private static object OnCoerceTickLength(DependencyObject d, object value)
        {
            return ((double)value).Clamp(MinTickLength, MaxTickLength);
        }

        /// <summary>
        /// Gets or sets the maximum number of major ticks.
        /// This value is clamped between <see cref="MinMaxTickCount"/> and <see cref="MaxMaxTickCount"/> inclusive.
        /// </summary>
        public long MaxTickCount
        {
            get => (long)GetValue(MaxTickCountProperty);
            set => SetValue(MaxTickCountProperty, value);
        }

        /// <summary>
        /// Identifes the <see cref="MaxTickCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxTickCountProperty = DependencyProperty.Register
            (
                nameof(MaxTickCount), typeof(long), typeof(Axis), 
                new FrameworkPropertyMetadata(DefaultMaxTickCount,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender, null, OnCoerceMaxTickCount)
            );

        private static object OnCoerceMaxTickCount(DependencyObject d, object value)
        {
            return ((double)value).Clamp(MinMaxTickCount, MaxMaxTickCount);
        }
        #endregion

        /************************************************************************/

        #region TextFormat / TextProvider
        /// <summary>
        /// Gets or (from this assembly) sets the format used when converting a tick value to text.
        /// </summary>
        public string TextFormat
        {
            get => (string)GetValue(TextFormatProperty);
            internal set => SetValue(TextFormatPropertyKey, value);
        }
        
        private static readonly DependencyPropertyKey TextFormatPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(TextFormat), typeof(string), typeof(Axis), 
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender)
            );

        /// <summary>
        /// Identifes the <see cref="TextFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextFormatProperty = TextFormatPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or (from this assembly) sets an <see cref="IDoubleConverter"/> to convert tick values to text.
        /// </summary>
        public IDoubleConverter TextProvider
        {
            get => (IDoubleConverter)GetValue(TextProviderProperty);
            internal set => SetValue(TextProviderPropertyKey, value);
        }

        private static readonly DependencyPropertyKey TextProviderPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(TextProvider), typeof(IDoubleConverter), typeof(Axis),
                new FrameworkPropertyMetadata(null, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender)
            );

        /// <summary>
        /// Identifes the <see cref="TextProvider"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProviderProperty = TextProviderPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region TickVisibility / TickAlignment
        /// <summary>
        /// Gets or sets a value that determines which ticks are rendered
        /// </summary>
        public TickVisibility TickVisibility
        {
            get => (TickVisibility)GetValue(TickVisibilityProperty);
            set => SetValue(TickVisibilityProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TickVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TickVisibilityProperty = DependencyProperty.Register
            (
                nameof(TickVisibility), typeof(TickVisibility), typeof(Axis), 
                new FrameworkPropertyMetadata(TickVisibility.Default,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender)
            );

        /// <summary>
        /// Gets or (from this assembly) sets a value that determines how ticks are aligned on the axis.
        /// </summary>
        public TickAlignment TickAlignment
        {
            get => (TickAlignment)GetValue(TickAlignmentProperty);
            set => SetValue(TickAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TickAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TickAlignmentProperty = DependencyProperty.Register
            (
                nameof(TickAlignment), typeof(TickAlignment), typeof(Axis), 
                new FrameworkPropertyMetadata(TickAlignment.Default,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender)
            );
        #endregion

        /************************************************************************/

        #region Range
        /// <summary>
        /// Gets the range of values on the axis.
        /// </summary>
        public Range Range
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region AxisType / AxisPlacement
        /// <summary>
        /// Gets the axis type.
        /// </summary>
        public AxisType AxisType
        {
            get;
        }

        /// <summary>
        /// Gets or (from this assembly) sets the axis placement
        /// </summary>
        public AxisPlacement AxisPlacement
        {
            get => (AxisPlacement)GetValue(AxisPlacementProperty);
            internal set => SetValue(AxisPlacementPropertyKey, value);
        }

        private static readonly DependencyPropertyKey AxisPlacementPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(AxisPlacement), typeof(AxisPlacement), typeof(Axis), 
                new FrameworkPropertyMetadata(AxisPlacement.Left,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender)
            );

        /// <summary>
        /// Identifies the <see cref="AxisPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisPlacementProperty = AxisPlacementPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value that indicates whether the axis is horizontal or not.
        /// </summary>
        public bool IsHorizontal
        {
            get => AxisPlacement == AxisPlacement.Bottom || AxisPlacement == AxisPlacement.Top;
        }
        #endregion

        /************************************************************************/

        #region IsValueReversed
        /// <summary>
        /// Gets or sets a flag that determines whether the axis is reversed or not.
        /// </summary>
        public bool IsValueReversed
        {
            get => (bool)GetValue(IsValueReversedProperty);
            set => SetValue(IsValueReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsValueReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsValueReversedProperty = DependencyProperty.Register
            (
                nameof(IsValueReversed), typeof(bool), typeof(Axis), 
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender)
            );
        #endregion

        /************************************************************************/

        #region DataTransform
        /// <summary>
        /// Gets or sets <see cref="DataTransform"/> for an axis.
        /// </summary>
        /// <remarks>
        /// The default transform is <see cref="IdentityDataTransform"/>
        /// DataTransform is used for transform plot coordinates from Range property to data values, which will be displayed on ticks
        /// </remarks>
        public DataTransform DataTransform
        {
            get => dataTransform;
            set
            {
                if (value != dataTransform)
                {
                    dataTransform = value;
                    InvalidateMeasure();
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Shifts the range of the axis by the specified percentage.
        /// </summary>
        /// <param name="perecentage">The percentage to shift.</param>
        public void Shift(double perecentage)
        {
            if (perecentage == 0) return;

            if (IsHorizontal)
            {
                Range.Shift(IsValueReversed ? -perecentage : perecentage);
            }
            else
            {
                Range.Shift(IsValueReversed ? perecentage : -perecentage);
            }
        }

        /// <summary>
        /// Gets a screen coordinate from the specified tick value.
        /// </summary>
        /// <param name="tick">The tick value.</param>
        /// <param name="layoutSize">The size of the layout used for the translation,</param>
        /// <returns>The translated value.</returns>
        public double GetCoordinateFromTick(double tick, Size layoutSize)
        {
            return ValueToScreen(DataTransform.DataToPlot(tick), layoutSize, Range);
        }

        /// <summary>
        /// Gets the specified value formatted according to <see cref="TextProvider"/> and <see cref="TextFormat"/>.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A string representation of value.</returns>
        public string GetFormattedValue(double value)
        {
            return TextProvider != null ? TextProvider.Convert(value, TextFormat) : value.ToString(TextFormat);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Sets the data series. This is used by the axis to generate its ticks.
        /// </summary>
        /// <param name="data"></param>
        internal void SetData(DataSeries data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            switch (AxisType)
            {
                case AxisType.X:
                    Range = data.DataRange.X;
                    break;
                case AxisType.Y:
                    Range = data.DataRange.Y;
                    break;
            }
            Range.CreateSnapshot();
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
            // The value 1024 is used to assign to the starting size dimension if the dimenison comes in as Infinity.
            // For reasons unknown, a size smaller than 479.5 causes the center section of the grid
            // (where ChartBase sits) to receive a size that causes that center grid cell to go out of bounds, leaving it cut off.
            // Whether it gets cut off or not depends on other factors. Orientation = Vertical, AxisAlignment = Values, and using
            // the DoubleToLookupConverter to emulate getting values for the X axis labels from a lookup table causes this problem.
            //
            // I think it has something to do with how Grid measures. An axis sits in an Auto column or row. See:
            // https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Grid.cs,f9ce1d6be154348a
            // 
            // In the comments for Grid.MeasureOverride(), they talk about certain grid topologies that create a cyclical dependency.
            // I think that's what's happening here.
            // 
            // By setting the infinite dimension to 1024, the problem goes away. It could also be set to 479.5, but I'm not sure
            // (being on the limit, didn't check more decimal places) if some other factor might introduce the problem again.
            //
            // I also tried higher values, up to one trillion. Still okay. However, double.MaxValue and double.MaxValue minus one trillion
            // don't work. The cell is cut off under all conditions and switching to Orientation = Vertical causes an exception
            // during child.Measure(constraint) below.
            //
            // UPDATE: Now setting to 512. Also in ChartBase and ChartContainer.
            Size desiredSize = new Size
                (
                    constraint.Width.IsFinite() ? constraint.Width : 512,
                    constraint.Height.IsFinite() ? constraint.Height : 512
                );

            ClearAll();
            CreateTicks(desiredSize);
            CreateGeometry();

            foreach (UIElement child in Children)
            {
                child.Measure(constraint);
            }

            Size maxTextSize = MajorTicks.GetMaxTextSize();

            // LabelGap is the distance between the tick mark and the label
            // and between the label and the edge of the control.
            if (IsHorizontal)
            {
                desiredSize.Height = majorTickPath.DesiredSize.Height + maxTextSize.Height + (LabelGapHorz * 2.0);
            }
            else
            {
                desiredSize.Width = majorTickPath.DesiredSize.Width + maxTextSize.Width + (LabelGapVert * 2.0);
            }

            desiredSize.Width = Math.Min(desiredSize.Width, constraint.Width);
            desiredSize.Height = Math.Min(desiredSize.Height, constraint.Height);

            return desiredSize;
        }

        /// <summary>
        /// Positions children of this element.
        /// </summary>
        /// <param name="finalSize">The final area within the parent this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Size maxTextSize = MajorTicks.GetMaxTextSize();

            switch (AxisPlacement)
            {
                case AxisPlacement.Top:
                    majorTickPath.Arrange(new Rect(0, finalSize.Height - TickLength, finalSize.Width, TickLength));
                    minorTickPath.Arrange(new Rect(0, finalSize.Height - TickLength / 2.0, finalSize.Width, TickLength / 2.0));
                    break;
                case AxisPlacement.Bottom:
                    majorTickPath.Arrange(new Rect(0, 0, finalSize.Width, TickLength));
                    minorTickPath.Arrange(new Rect(0, 0, finalSize.Width, TickLength / 2.0));
                    break;
                case AxisPlacement.Right:
                    majorTickPath.Arrange(new Rect(0, 0, TickLength, finalSize.Height));
                    minorTickPath.Arrange(new Rect(0, 0, TickLength / 2.0, finalSize.Height));
                    break;
                case AxisPlacement.Left:
                    majorTickPath.Arrange(new Rect(finalSize.Width - TickLength, 0, TickLength, finalSize.Height));
                    minorTickPath.Arrange(new Rect(Math.Max(0, finalSize.Width - TickLength / 2.0), 0, TickLength / 2.0, finalSize.Height));
                    break;
            }

            foreach (MajorTick tick in MajorTicks)
            {
                if (IsHorizontal)
                {
                    double x = tick.Coordinate - tick.TextWidth / 2.0;
                    double y = (AxisPlacement == AxisPlacement.Top) ? finalSize.Height - TickLength - maxTextSize.Height - LabelGapHorz : TickLength + LabelGapHorz;
                    if (tick.IsCoordinateWithin(finalSize.Width))
                    {
                        tick.Text.Arrange(new Rect(x, y, tick.TextWidth, tick.TextHeight));
                    }
                }
                else
                {
                    double x = (AxisPlacement == AxisPlacement.Left) ? LabelGapVert + maxTextSize.Width - tick.TextWidth : TickLength + LabelGapVert;
                    double y = tick.Coordinate - tick.TextHeight / 2.0;
                    if (tick.IsCoordinateWithin(finalSize.Height))
                    {
                        tick.Text.Arrange(new Rect(x, y, tick.TextWidth, tick.TextHeight));
                    }
                }
            }
            return finalSize;
        }
        #endregion

        /************************************************************************/

        #region Private methods (Geometry)
        private void ClearAll()
        {
            MajorTicks.Clear();
            MinorTicks.Clear();

            foreach (var child in Children.OfType<TickText>().ToList())
            {
                Children.Remove(child);
            }

            majorTickPath.Data = null;
            minorTickPath.Data = null;
        }

        private void CreateGeometry() // Size desiredSize)
        {
            GeometryGroup majorTickGeometry = new GeometryGroup();
            GeometryGroup minorTickGeometry = new GeometryGroup();

            foreach (MajorTick tick in MajorTicks)
            {
                LineGeometry line = GetLineGeometryFromTick(tick, TickLength);
                majorTickGeometry.Children.Add(line);
                tick.Text.Foreground = MajorTickBrush;
                Children.Add(tick.Text);
            }

            foreach (MinorTick tick in MinorTicks)
            {
                LineGeometry line = GetLineGeometryFromTick(tick, TickLength / 2.0);
                minorTickGeometry.Children.Add(line);
            }

            majorTickPath.Data = majorTickGeometry;
            minorTickPath.Data = minorTickGeometry;
        }

        private LineGeometry GetLineGeometryFromTick(MinorTick tick, double tickLength)
        {
            LineGeometry line = new LineGeometry();
            if (IsHorizontal)
            {
                line.StartPoint = new Point(tick.Coordinate, 0);
                line.EndPoint = new Point(tick.Coordinate, tickLength);
            }
            else
            {
                line.StartPoint = new Point(0, tick.Coordinate);
                line.EndPoint = new Point(tickLength, tick.Coordinate);
            }
            return line;
        }

        private double ValueToScreen(double value, Size axisSize, Range range)
        {
            double result;
            double rangeMin = IsValueReversed ? range.Max : range.Min;
            double rangeMax = IsValueReversed ? range.Min : range.Max;

            if (IsHorizontal)
            {
                result = range.IsPoint ?
                    (axisSize.Width / 2) :
                    ((value - rangeMin) * axisSize.Width / (rangeMax - rangeMin));
            }
            else
            {
                result = range.IsPoint ?
                    (axisSize.Height / 2) :
                    (axisSize.Height - (value - rangeMin) * axisSize.Height / (rangeMax - rangeMin));
            }
            return result;
        }
        #endregion

        /************************************************************************/

        #region Private methods (Tick generation)
        /// <summary>
        /// Creates ticks. This method is called from MeasureOverride().
        /// Before this method is called, everything has been cleared (MajorTicks, MinorTicks, Children, paths, etc.)
        /// </summary>
        /// <param name="axisSize">The axis size.</param>
        private void CreateTicks(Size axisSize)
        {
            if (TickVisibility != TickVisibility.None)
            {
                switch (TickAlignment)
                {
                    case TickAlignment.Range:
                        CreateTicksFromRange(axisSize);
                        break;
                    case TickAlignment.Values:
                        CreateTicksFromValues(axisSize);
                        break;
                }
                if (TickVisibility == TickVisibility.MajorMinor || TickVisibility == TickVisibility.MajorMinorEdge)
                {
                    CreateMinorTicks(axisSize, 5);
                }
            }
        }

        private void CreateTicksFromRange(Size size)
        {
            if (Range.HasValues)
            {
                var calc = new TickCalculation(Range, MaxTickCount);

                CreateMajorTicks(size, calc);

                while (!DoMajorTickLabelsFit(size))
                {
                    MajorTicks.Clear();
                    calc.DecreaseTickCount();
                    CreateMajorTicks(size, calc);
                }
            }
        }

        private void CreateTicksFromValues(Size size)
        {
            if (data != null)
            {
                IEnumerable<double> e = AxisType == AxisType.X ? data.EnumerateX() : data.EnumerateY();
                CreateTicksFromValues(size, e);
            }
        }

        private void CreateTicksFromValues(Size size, IEnumerable<double> enumerator)
        {
            int step = 1;
            CreateTicksFromValues(size, enumerator, step);
            while (!DoMajorTickLabelsHaveMinimumSpacing(size, 16))
            {
                MajorTicks.Clear();
                step++;
                CreateTicksFromValues(size, enumerator, step);
            }
        }

        private void CreateTicksFromValues(Size size, IEnumerable<double> enumerator, int step)
        {
            int idx = 1;
            foreach (double value in enumerator)
            {
                if (idx % step == 0)
                {
                    double coordinate = GetCoordinateFromTick(value, size);
                    MajorTicks.Add(new MajorTick(value, coordinate, GetTickText(value)));
                }
                idx++;
            }
        }

        private bool DoMajorTickLabelsHaveMinimumSpacing(Size size, double minSpacing)
        {
            double min = MajorTicks.GetMinimumTextSpacing(size, IsHorizontal);
            return min >= minSpacing;
        }

        private void CreateMajorTicks(Size size, TickCalculation calc)
        {
            for (int k = 0; k < calc.TickCount + 1; k++)
            {
                double value = MathHelper.Round(calc.X0 + k * calc.Multiplier, calc.Beta);
                if (Range.Includes(value))
                {
                    double coordinate = GetCoordinateFromTick(value, size);
                    MajorTicks.Add(new MajorTick(value, coordinate, GetTickText(value)));
                }
            }
        }

        private bool DoMajorTickLabelsFit(Size size)
        {
            Size totalSize = MajorTicks.GetTotalTextSize(size, 32);
            if (IsHorizontal)
            {
                return totalSize.Width < size.Width;
            }
            return totalSize.Height < size.Height;
        }

        /// <summary>
        /// Creates minor ticks that fall between major ticks.
        /// </summary>
        /// <param name="size">Size of the axis.</param>
        /// <param name="count">The max number of minor ticks.</param>
        private void CreateMinorTicks(Size size, int count)
        {
            if (MajorTicks.Count > 1)
            {
                MajorTick tick0 = MajorTicks[0];
                MajorTick tickZ = MajorTicks[MajorTicks.Count - 1];

                double valueStep = 0;
                double coordStep = 0;

                int idx = 0;
                foreach (MajorTick tick in MajorTicks)
                {
                    if (idx < MajorTicks.Count - 1)
                    {
                        MajorTick nextTick = MajorTicks[idx + 1];

                        double valueDistance = nextTick.Value - tick.Value;
                        double coordDistance = nextTick.Coordinate - tick.Coordinate;

                        valueStep = valueDistance / (count + 1);
                        coordStep = coordDistance / (count + 1);

                        for (int step = 1; step <= count; step++)
                        {
                            MinorTicks.Add(new MinorTick(tick.Value + (valueStep * step), tick.Coordinate + (coordStep * step)));
                        }
                    }
                    idx++;
                }

                if (TickVisibility == TickVisibility.MajorMinorEdge)
                {
                    CreateMinorEdgeTicks(size, valueStep, coordStep, count);
                }
            }
        }

        /// <summary>
        /// Creates minor ticks that come before the first major tick (if any)
        /// and those that come after the last major tick (if any).
        /// </summary>
        /// <param name="size">Size of the axis.</param>
        /// <param name="valueStep">The value difference that represents a minor tick step.</param>
        /// <param name="coordStep">The coordinate difference that represents a minor tick step.</param>
        /// <param name="count">The max number of minor ticks.</param>
        private void CreateMinorEdgeTicks(Size size, double valueStep, double coordStep, int count)
        {
            // Array access safe because this method is only called if MajorTicks.Count > 1.
            MajorTick tick0 = MajorTicks[0];
            MajorTick tickZ = MajorTicks[MajorTicks.Count - 1];

            // Cushion so that minor ticks aren't too close to the edge
            double cushion = 5.0;
            double max = IsHorizontal ? size.Width - cushion : size.Height - cushion;

            for (int step = 1; step <= count; step++)
            {
                MinorTick tick = new MinorTick(tick0.Value - (valueStep * step), tick0.Coordinate - (coordStep * step));

                if (tick.IsCoordinateWithin(cushion, max))
                {
                    MinorTicks.Add(tick);
                }

                tick = new MinorTick(tickZ.Value + (valueStep * step), tickZ.Coordinate + (coordStep * step));

                if (tick.IsCoordinateWithin(cushion, max))
                {
                    MinorTicks.Add(tick);
                }
            }
        }

        private TickText GetTickText(double tickValue)
        {
            TickText tickText = new TickText()
            {
                Text = GetFormattedValue(tickValue)
            };
            return tickText;
        }
        #endregion
    }
}