using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Range range;
        private Path majorTickPath;
        private Path minorTickPath;

        private int tickLength;
        private AxisPlacement axisPlacement;
        private TickVisibility tickVisibility;
        private DataTransform dataTransform;
        private bool isReversed;
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
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="Axis"/> class.
        /// </summary>
        public Axis()
        {
            TickProvider = new TickProvider();

            // These are set via the backing store to avoid triggering InvalidateMeasure()
            tickLength = 12;
            tickVisibility = TickVisibility.Default;

            range = Range.EmptyRange();
            dataTransform = new IdentityDataTransform();
            // dataTransform = new LinearDataTransform();

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

        public event EventHandler<GeometryGroup> MajorTickGeometryCreated;

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

        #region TickLength
        /// <summary>
        /// Gets or sets the length of the major ticks. Minor ticks are half this size.
        /// This value is clamped between 10 and 24 inclusive.
        /// </summary>
        public int TickLength
        {
            get => tickLength;
            set
            {
                tickLength = value.Clamp(10, 24);
                InvalidateMeasure();
            }
        }
        #endregion

        /************************************************************************/

        #region TickProvider / TickVisibility
        /// <summary>
        /// Gets the tick provider
        /// </summary>
        public TickProvider TickProvider
        {
            get;
        }

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
        #endregion

        /************************************************************************/

        #region Range
        /// <summary>
        /// Gets or sets the range of values on the axis.
        /// </summary>
        public Range Range
        {
            get => range;
            set
            {
                if (value != range)
                {
                    range = value;
                    InvalidateMeasure();
                }
            }
        }
        #endregion

        /************************************************************************/

        #region AxisPlacement
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
        public bool IsReversed
        {
            get => isReversed;
            set
            {
                if (value != isReversed)
                {
                    isReversed = value;
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
        /// 
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

        #region Protected methods
        /// <summary>
        /// Measures the size in layout required for child elements and determines a size for the Figure. 
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

            ClearGeometry();
            CreateTicks(desiredSize);
            CreateGeometry(desiredSize);

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            Size maxTextSize = TickProvider.GetMaxTextSize();

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
        /// Positions child elements and determines a size for a Figure.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that Figure should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Size maxTextSize = TickProvider.GetMaxTextSize();

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

            foreach (MajorTick tick in TickProvider.EnumerateMajorTicks())
            {
                double coordinate = GetCoordinateFromTick(tick.Value, finalSize);

                if (IsHorizontal)
                {
                    double x = coordinate - tick.TextWidth / 2;
                    double y = (AxisPlacement == AxisPlacement.Top) ? finalSize.Height - TickLength - maxTextSize.Height - LabelGapHorz : TickLength + LabelGapHorz;
                    tick.Text.Arrange(new Rect(x, y, tick.TextWidth, tick.TextHeight));
                }
                else
                {
                    double x = (AxisPlacement == AxisPlacement.Left) ? LabelGapVert + maxTextSize.Width - tick.TextWidth : TickLength + LabelGapVert;
                    double y = coordinate - tick.TextHeight / 2;
                    tick.Text.Arrange(new Rect(x, y, tick.TextWidth, tick.TextHeight));
                }
            }
            return finalSize;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void ClearGeometry()
        {
            foreach (var child in Children.OfType<TickText>().ToList())
            {
                Children.Remove(child);
            }

            majorTickPath.Data = null;
            minorTickPath.Data = null;
        }

        private void CreateTicks(Size desiredSize)
        {
            double min = DataTransform.PlotToData(Range.Min.IsFinite() ? Range.Min : 0);
            double max = DataTransform.PlotToData(Range.Max.IsFinite() ? Range.Max : 1);
            Range range = new Range(min, max);
            TickProvider.CreateTicks(desiredSize, range, ValidateLabelArrangement);
        }

        private TickEvaluationResult ValidateLabelArrangement(Size axisSize)
        {
            List<MajorTick> majorTicks = TickProvider.EnumerateMajorTicks().ToList();

            majorTicks.ForEach(item => item.Text.Measure(axisSize));

            var sizeInfos = majorTicks.Select(item =>
                new
                {
                    Offset = GetCoordinateFromTick(item.Value, axisSize),
                    Size = IsHorizontal ? item.TextWidth : item.TextHeight,
                    Label = item.Text.Text,
                })
                .OrderBy(item => item.Offset).ToArray();

            //if (IsHorizontal)
            //{
            //    Debug.WriteLine("--------------------------------");
            //    foreach (var info in sizeInfos)
            //    {
            //        Debug.WriteLine($"SizeInfo. Label:{info.Label} Offset: {info.Offset} Size:{info.Size}");
            //    }
            //    Debug.WriteLine("--------------------------------");
            //}

            // If distance between labels if smaller than threshold for any of the labels - decrease
            for (int idx = 0; idx < sizeInfos.Length - 1; idx++)
            {
                if ((sizeInfos[idx].Offset + sizeInfos[idx].Size * DecreaseRatio / 2) > sizeInfos[idx + 1].Offset)
                {
                    return TickEvaluationResult.Decrease;
                }
            }

            // If distance between labels if larger than threshold for all of the labels - increase
            TickEvaluationResult res = TickEvaluationResult.Increase;
            for (int idx = 0; idx < sizeInfos.Length - 1; idx++)
            {
                if ((sizeInfos[idx].Offset + sizeInfos[idx].Size * IncreaseRatio / 2) > sizeInfos[idx + 1].Offset)
                {
                    res = TickEvaluationResult.Ok;
                    break;
                }
            }

            return res;
        }

        private void CreateGeometry(Size desiredSize)
        {
            GeometryGroup majorTickGeometry = new GeometryGroup();
            GeometryGroup minorTickGeometry = new GeometryGroup();

            bool drawMajorTicks = TickVisibility != TickVisibility.None;
            bool drawMinorTicks = TickVisibility == TickVisibility.MajorAndMinor && !Range.IsPoint;

            foreach (MajorTick tick in TickProvider.EnumerateMajorTicks())
            {
                LineGeometry line = new LineGeometry();
                double coordinate = GetCoordinateFromTick(tick.Value, desiredSize);

                if (IsHorizontal)
                {
                    line.StartPoint = new Point(coordinate, 0);
                    line.EndPoint = new Point(coordinate, TickLength);
                }
                else
                {
                    line.StartPoint = new Point(0, coordinate);
                    line.EndPoint = new Point(TickLength, coordinate);
                }

                majorTickGeometry.Children.Add(line);
                if (drawMajorTicks)
                {
                    tick.Text.Foreground = MajorTickBrush;
                    Children.Add(tick.Text);
                }
            }

            if (drawMinorTicks)
            {
                foreach (double tickValue in TickProvider.EnumerateMinorTicks())
                {
                    LineGeometry line = new LineGeometry();
                    double coordinate = GetCoordinateFromTick(tickValue, desiredSize);

                    if (IsHorizontal)
                    {
                        line.StartPoint = new Point(coordinate, 0);
                        line.EndPoint = new Point(coordinate, TickLength / 2.0);
                    }
                    else
                    {
                        line.StartPoint = new Point(0, coordinate);
                        line.EndPoint = new Point(TickLength / 2.0, coordinate);
                    }

                    minorTickGeometry.Children.Add(line);
                }
            }

            if (drawMajorTicks)
            {
                majorTickPath.Data = majorTickGeometry;
                minorTickPath.Data = minorTickGeometry;
            }

            MajorTickGeometryCreated?.Invoke(this, majorTickGeometry);
        }

        private double ValueToScreen(double value, Size axisSize, Range range)
        {
            double result;
            double rangeMin = IsReversed ? range.Max : range.Min;
            double rangeMax = IsReversed ? range.Min : range.Max;

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
    }
}