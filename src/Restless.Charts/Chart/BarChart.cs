using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a bar chart.
    /// </summary>
    public class BarChart : ChartBase
    {
        #region Private
        /// <summary>
        /// Tolerance for bar text. Text plus this amount must fit inside bar.
        /// </summary>
        private const double TextTolerance = 10.0;
        /// <summary>
        /// Text cushion. Text is placed this distance from edge of bar.
        /// </summary>
        private const double TextEdgeCushion = TextTolerance / 2.0;

        private readonly List<DrawingVisual> textVisuals;

        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default bar thickness. This value (zero) signifies that bars will be auto sized.
        /// </summary>
        public const double DefaultBarThickness = 0;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BarChart"/> class.
        /// </summary>
        public BarChart()
        {
            UseLayoutRounding = false;
            textVisuals = new List<DrawingVisual>();
        }
        #endregion
        
        /************************************************************************/

        #region BarThickness
        /// <summary>
        /// Gets or sets the thickness of the bars. Zero signifies auto size. This is a dependency property.
        /// </summary>
        public double BarThickness
        {
            get => (double)GetValue(BarThicknessProperty);
            set => SetValue(BarThicknessProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BarThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BarThicknessProperty = DependencyProperty.Register
            (
                nameof(BarThickness), typeof(double), typeof(BarChart), new PropertyMetadata(DefaultBarThickness, OnBarPropertyChanged, OnCoerceBarThickness)
            );

        private static object OnCoerceBarThickness(DependencyObject d, object value)
        {
            double dval = (double)value;
            return dval.Clamp(0, 100);
        }

        private static void OnBarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BarChart c)
            {
                c.InvalidateMeasure();
            }
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called by <see cref="ChartBase"/> to create child visuals.
        /// </summary>
        /// <param name="desiredSize">The desired size</param>
        protected override void CreateChildren(Size desiredSize)
        {
            double barWidth = BarThickness > 0 ? BarThickness : GetAutoBarThickness(Data, desiredSize);
            double xMax = Owner.Orientation == Orientation.Vertical ? desiredSize.Width : desiredSize.Height;
            double yMax = Owner.Orientation == Orientation.Vertical ? desiredSize.Height : desiredSize.Width;

            textVisuals.Clear();

            foreach (DataPoint point in Data)
            {
                int yIndex = 0;
                foreach (double yValue in point.YValues.OrderByDescending((v) => Math.Abs(v)))
                {
                    Pen pen = new Pen(Data.DataBrushes[yIndex], barWidth);

                    double x = Owner.XAxis.GetCoordinateFromTick(point.XValue, desiredSize);
                    double y = Owner.YAxis.GetCoordinateFromTick(yValue, desiredSize);
                    double yZero = Owner.YAxis.GetCoordinateFromTick(0, desiredSize);
                    double barLength = Math.Abs(y - yZero);

                    if (IsVisualCreatable(x, y, yZero, xMax, yMax, barWidth))
                    {
                        if (Owner.Orientation == Orientation.Vertical)
                        {
                            Children.Add(CreateLineVisual(pen, x, yZero, x, y));
                            CreateTextDisplayIf(yIndex, yValue, x, y, yZero, barWidth, barLength);
                        }
                        else
                        {
                            Children.Add(CreateLineVisual(pen, yZero, x, y, x));
                            CreateTextDisplayIf(yIndex, yValue, y, x, yZero, barWidth, barLength);
                        }
                    }
                    yIndex++;
                }
            }
            // CreateTextDisplayIf() adds its visuals to textVisuals. Now add them to Children.
            // This enables us to make sure text is above the bars when using multiple Y values.
            textVisuals.ForEach((v) => Children.Add(v));
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void CreateTextDisplayIf(int yIndex, double yValue, double x, double y, double yZero, double barWidth, double barLength)
        {
            if (DisplayValues && yIndex == 0)
            {
                FormattedText text = GetFormattedText(Owner.GetFormattedYValue(yValue), ValuesFontFamily, ValuesFontSize, Data.PrimaryTextBrushes.GetBrush(yIndex));

                if (TextFitsInWidth(text, barWidth))
                {
                    if (!TextFitsInLength(text, barLength))
                    {
                        text.SetForegroundBrush(Data.SecondaryTextBrushes.GetBrush(yIndex));
                    }
                    bool isNegative = Owner.Orientation == Orientation.Vertical ? y > yZero : x < yZero;
                    x = GetAdjustedTextX(x, barLength, isNegative, text);
                    y = GetAdjustedTextY(y, barLength, isNegative, text);
                    double rotation = Owner.Orientation == Orientation.Vertical ? -90.0 : 0.0;
                    textVisuals.Add(CreateTextVisual(text, x, y, rotation));
                }
            }
        }

        private bool TextFitsInWidth(FormattedText text, double width)
        {
            return width > text.Height + TextTolerance;
        }

        private bool TextFitsInLength(FormattedText text, double length)
        {
            return text.Width + TextTolerance < length;
        }

        private double GetAdjustedTextX(double x, double barLen, bool isNegative, FormattedText text)
        {

            if (Owner.Orientation == Orientation.Vertical)
            {
                x -= text.Width / 2.0;
            }
            else
            {
                if (text.Width + TextTolerance > barLen)
                {
                    if (isNegative) x -= text.Width + TextEdgeCushion;
                    else x += TextEdgeCushion;
                }
                else
                {
                    if (isNegative) x += TextEdgeCushion;
                    else x -= text.Width + TextEdgeCushion;
                }

            }
            return x;
        }

        private double GetAdjustedTextY(double y, double barLen, bool isNegative, FormattedText text)
        {
            if (Owner.Orientation == Orientation.Vertical)
            {
                y -= text.Height / 2.0;
                if (text.Width + TextTolerance > barLen)
                {
                    if (isNegative) y += (text.Width / 2.0) + TextEdgeCushion;
                    else y -= (text.Width / 2.0) + TextEdgeCushion;
                }
                else
                {
                    if (isNegative) y -= (text.Width / 2.0) + TextEdgeCushion;
                    else y += (text.Width / 2.0) + TextEdgeCushion;
                }
            }
            else
            {
                y -= text.Height / 2.0;
            }
            return y;
        }

        private bool IsVisualCreatable(double xc, double yc, double ycz, double xMax, double yMax, double lineWidth)
        {
            double lineTolerance = lineWidth / 2.0;
            if (xc < -lineTolerance) return false;
            if (xc > xMax + lineTolerance) return false;
            if (ycz < 0.0 && yc < 0.0) return false;
            if (ycz > yMax && yc > ycz) return false;
            if (ycz > yMax && yc > yMax) return false;
            return true;
        }

        private double GetAutoBarThickness(DataSeries series, Size desiredSize)
        {
            double result = double.PositiveInfinity;

            if (series.Count > 0)
            {
                double s1 = Owner.XAxis.GetCoordinateFromTick(series[0].XValue, desiredSize);
                double sx = Owner.XAxis.GetCoordinateFromTick(series[series.Count - 1].XValue, desiredSize);
                double distance = sx - s1;
                if (distance == 0.0) return Owner.XAxis.IsHorizontal ? desiredSize.Width / 2.0 : desiredSize.Height / 2.0;
                result = Math.Abs(distance / series.Count);
            }
            return result.IsFinite() ? result : 10.0;
        }
        #endregion
    }
}
