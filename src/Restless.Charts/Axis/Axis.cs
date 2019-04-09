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
        private const double LabelGapHorz = 0.0;
        private const double LabelGapVert = 2.0;

        private readonly ChartContainer owner;
        private Path majorTickPath;
        private Path minorTickPath;

        private int tickLength;
        private long maxTickCount;
        private AxisPlacement axisPlacement;
        private TickVisibility tickVisibility;
        private DataTransform dataTransform;
        private bool isValueReversed;
        private TickAlignment tickAlignment;
        private DataSeries data;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default brush for major ticks and their corresponding labels.
        /// </summary>
        public static readonly Brush DefaultMajorTickBrush = Brushes.DarkSlateBlue;

        /// <summary>
        /// Gets the default brush for minor ticks.
        /// </summary>
        public static readonly Brush DefaultMinorTickBrush = Brushes.LightGray;

        /// <summary>
        /// Gets the minimum allowed value for <see cref="MaxTickCount"/>.
        /// </summary>
        public const int MinMaxTickCount = 10;

        /// <summary>
        /// Gets the maximum allowed value for <see cref="MaxTickCount"/>.
        /// </summary>
        public const int MaxMaxTickCount = 24;

        /// <summary>
        /// Gets the default value for <see cref="MaxTickCount"/>.
        /// </summary>
        public const int DefaultMaxTickCount = 20;

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

            // These are set via the backing store to avoid triggering InvalidateMeasure()
            tickLength = DefaultTickLength;
            maxTickCount = DefaultMaxTickCount;
            tickVisibility = TickVisibility.Default;
            tickAlignment = TickAlignment.Default;

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

        #region Ticks
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

        /// <summary>
        /// Gets or sets the length of the major ticks. Minor ticks are half this size.
        /// This value is clamped between <see cref="MinTickLength"/> and <see cref="MaxTickLength"/> inclusive.
        /// </summary>
        public int TickLength
        {
            get => tickLength;
            set
            {
                tickLength = value.Clamp(MinTickLength, MaxTickLength);
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of major ticks.
        /// This value is clamped between <see cref="MinMaxTickCount"/> and <see cref="MaxMaxTickCount"/> inclusive.
        /// </summary>
        public long MaxTickCount
        {
            get => maxTickCount;
            set
            {
                maxTickCount = value.Clamp(MinMaxTickCount, MaxMaxTickCount);
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or (from this assembly) sets the format used when converting a tick value to text.
        /// </summary>
        public string TextFormat
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or (from this assembly) sets an <see cref="IDoubleConverter"/> to convert tick values to text.
        /// </summary>
        public IDoubleConverter TextProvider
        {
            get;
            internal set;
        }
        #endregion

        /************************************************************************/

        #region TickProvider / TickVisibility / TickAlignment
        /// <summary>
        /// Gets or sets a value that determines which ticks are rendered
        /// </summary>
        public TickVisibility TickVisibility
        {
            get => tickVisibility;
            set
            {
                if (value != tickVisibility)
                {
                    tickVisibility = value;
                    InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Gets or (from this assembly) sets a value that determines how ticks are aligned on the axis.
        /// </summary>
        public TickAlignment TickAlignment
        {
            get => tickAlignment;
            internal set
            {
                if (value != tickAlignment)
                {
                    tickAlignment = value;
                    InvalidateMeasure();
                }
            }
        }
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

        #region AxisPlacement
        /// <summary>
        /// Gets the axis type.
        /// </summary>
        public AxisType AxisType
        {
            get;
        }

        /// <summary>
        /// Gets or (from this assembly) sets the axis placement.
        /// </summary>
        public AxisPlacement AxisPlacement
        {
            get => axisPlacement;
            internal set
            {
                if (value != axisPlacement)
                {
                    axisPlacement = value;
                    InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the axis is horizontal or not.
        /// </summary>
        public bool IsHorizontal
        {
            get => AxisPlacement == AxisPlacement.Bottom || AxisPlacement == AxisPlacement.Top;
        }
        #endregion

        /************************************************************************/

        #region IsReversed
        /// <summary>
        /// Gets or sets a flag that determines whether the axis is reversed or not.
        /// </summary>
        public bool IsValueReversed
        {
            get => isValueReversed;
            set
            {
                if (value != isValueReversed)
                {
                    isValueReversed = value;
                    InvalidateMeasure();
                }
            }
        }
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
        /// <param name="availableSize">
        /// The available size that this element can give to child elements.
        /// Infinity can be specified as a value to indicate that the element will size to whatever content is available.
        /// </param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size
                (
                    availableSize.Width.IsFinite() ? availableSize.Width : 128,
                    availableSize.Height.IsFinite() ? availableSize.Height : 128
                );

            ClearAll();
            CreateTicks(desiredSize);
            CreateGeometry(desiredSize);

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
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

            desiredSize.Width = Math.Min(desiredSize.Width, availableSize.Width);
            desiredSize.Height = Math.Min(desiredSize.Height, availableSize.Height);

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
                    double x = tick.Coordinate - tick.TextWidth / 2;
                    double y = (AxisPlacement == AxisPlacement.Top) ? finalSize.Height - TickLength - maxTextSize.Height - LabelGapHorz : TickLength + LabelGapHorz;
                    if (tick.IsCoordinateWithin(finalSize.Width))
                    {
                        tick.Text.Arrange(new Rect(x, y, tick.TextWidth, tick.TextHeight));
                    }
                }
                else
                {
                    double x = (AxisPlacement == AxisPlacement.Left) ? LabelGapVert + maxTextSize.Width - tick.TextWidth : TickLength + LabelGapVert;
                    double y = tick.Coordinate - tick.TextHeight / 2;
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

        private void CreateGeometry(Size desiredSize)
        {
            GeometryGroup majorTickGeometry = new GeometryGroup();
            GeometryGroup minorTickGeometry = new GeometryGroup();

            bool drawMajorTicks = TickVisibility != TickVisibility.None;
            bool drawMinorTicks = TickVisibility == TickVisibility.MajorAndMinor && !Range.IsPoint;

            foreach (MajorTick tick in MajorTicks)
            {
                LineGeometry line = GetLineGeometryFromTick(tick, TickLength);
                majorTickGeometry.Children.Add(line);
                if (drawMajorTicks)
                {
                    tick.Text.Foreground = MajorTickBrush;
                    Children.Add(tick.Text);
                }
            }

            if (drawMinorTicks)
            {
                foreach (MinorTick tick in MinorTicks)
                {
                    LineGeometry line = GetLineGeometryFromTick(tick, TickLength / 2.0);
                    minorTickGeometry.Children.Add(line);
                }
            }

            if (drawMajorTicks)
            {
                majorTickPath.Data = majorTickGeometry;
                minorTickPath.Data = minorTickGeometry;
            }
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
            switch (TickAlignment)
            {
                case TickAlignment.Range:
                    CreateTicksFromRange(axisSize);
                    break;
                case TickAlignment.Values:
                    CreateTicksFromValues(axisSize);
                    break;
            }
            CreateMinorTicks(axisSize, 5);
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

        private void CreateTicksFromValues(Size axisSize, IEnumerable<double> enumerator)
        {
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            foreach (double value in enumerator)
            {
            }
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

        private void CreateMinorTicks(Size size, int count)
        {
            if (MajorTicks.Count > 1)
            {
                for (int idx = 0; idx < MajorTicks.Count; idx++)
                {
                    if (idx < MajorTicks.Count - 1)
                    {
                        double tickValue = MajorTicks[idx].Value;
                        double tickCoord = MajorTicks[idx].Coordinate;

                        double valueDistance = MajorTicks[idx + 1].Value - MajorTicks[idx].Value;
                        double coordDistance = MajorTicks[idx + 1].Coordinate - MajorTicks[idx].Coordinate;

                        double valueStep = valueDistance / (count + 1);
                        double coordStep = coordDistance / (count + 1);

                        for (int step = 1; step <= count; step++)
                        {
                            MinorTicks.Add(new MinorTick(tickValue + (valueStep * step), tickCoord + (coordStep * step)));
                        }
                    }
                }
            }
        }

        private TickText GetTickText(double tickValue)
        {
            TickText tickText = new TickText()
            {
                Text = (TextProvider != null) ? TextProvider.Convert(tickValue, TextFormat) : tickValue.ToString(TextFormat)
            };
            return tickText;
        }
        #endregion
    }
}