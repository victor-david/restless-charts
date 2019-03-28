using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a line chart.
    /// </summary>
    public class LineChart : ChartBase
    {
        #region Private
        private readonly List<DrawingVisual> textVisuals;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the minimum allowed line thickness.
        /// </summary>
        public const double MinLineThickness = 1.0;

        /// <summary>
        /// Gets the maximum allowed line thickness.
        /// </summary>
        public const double MaxLineThickness = 10.0;

        /// <summary>
        /// Gets the default line thickness.
        /// </summary>
        public const double DefaultLineThickness = 2.0;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BarChart"/> class.
        /// </summary>
        public LineChart()
        {
            UseLayoutRounding = false;
            textVisuals = new List<DrawingVisual>();
        }
        #endregion

        /************************************************************************/

        #region LineThickness
        /// <summary>
        /// Gets or sets the thickness of the lines. This is a dependency property.
        /// </summary>
        public double LineThickness
        {
            get => (double)GetValue(LineThicknessProperty);
            set => SetValue(LineThicknessProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="LineThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            (
                nameof(LineThickness), typeof(double), typeof(LineChart), new PropertyMetadata(DefaultLineThickness, OnLineThicknessChanged, OnCoerceLineThickness)
            );

        private static object OnCoerceLineThickness(DependencyObject d, object value)
        {
            double dval = (double)value;
            return dval.Clamp(MinLineThickness, MaxLineThickness);
        }

        private static void OnLineThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineChart c)
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
            double xMax = Owner.Orientation == Orientation.Vertical ? desiredSize.Width : desiredSize.Height;
            double yMax = Owner.Orientation == Orientation.Vertical ? desiredSize.Height : desiredSize.Width;

            textVisuals.Clear();
            CreatePoints(xMax, yMax, desiredSize);
            for (int yIdx = 0; yIdx < Data.MaxYSeries; yIdx++)
            {
                CreateLines(yIdx, xMax, yMax, desiredSize);
            }
            // CreateTextDisplayIf() adds its visuals to textVisuals. Now add them to Children.
            // This enables us to make sure text is above the bars when using multiple Y values.
            textVisuals.ForEach((v) => Children.Add(v));
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void CreatePoints(double xMax, double yMax, Size desiredSize)
        {
            double rectWidth = LineThickness * 8.0;

            foreach (DataPoint point in Data)
            {
                double x = Owner.XAxis.GetCoordinateFromTick(point.XValue, desiredSize);
                double yZero = Owner.YAxis.GetCoordinateFromTick(0, desiredSize);

                for (int yIdx = 0; yIdx < point.YValues.Count; yIdx++)
                {
                    Pen pen = new Pen(Data.PrimaryTextBrushes[yIdx], 1.0);
                   
                    double y = Owner.YAxis.GetCoordinateFromTick(point.YValues[yIdx], desiredSize);

                    if (IsVisualCreatable(x, y, yZero, xMax, yMax, rectWidth))
                    {
                        if (Owner.Orientation == Orientation.Vertical)
                        {
                            Children.Add(CreateRectangleVisual(Data.DataBrushes[yIdx], pen, x, y, rectWidth));
                        }
                        else
                        {
                            Children.Add(CreateRectangleVisual(Data.DataBrushes[yIdx], pen, y, x, rectWidth));
                        }
                    }
                }
            }
        }

        private void CreateLines(int yIdx, double xMax, double yMax, Size desiredSize)
        {
            Pen pen = new Pen(Data.DataBrushes[yIdx], LineThickness);
            double yZero = Owner.YAxis.GetCoordinateFromTick(0, desiredSize);

            for (int dpIdx = 0; dpIdx < Data.Count; dpIdx++)
            {
                double yValue = Data[dpIdx].YValues[yIdx];
                double xStart = Owner.XAxis.GetCoordinateFromTick(Data[dpIdx].XValue, desiredSize);
                double yStart = Owner.YAxis.GetCoordinateFromTick(yValue, desiredSize);

                if (dpIdx < Data.Count - 1)
                {
                    double yValueNext = Data[dpIdx + 1].YValues[yIdx];
                    double xEnd = Owner.XAxis.GetCoordinateFromTick(Data[dpIdx + 1].XValue, desiredSize);
                    double yEnd = Owner.YAxis.GetCoordinateFromTick(yValueNext, desiredSize);

                    if (IsVisualCreatable(xStart, yStart, yZero, xMax, yMax, LineThickness) || 
                        IsVisualCreatable(xEnd, yEnd, yZero, xMax, yMax, LineThickness))
                    {
                        Children.Add(CreateLineVisual(pen, xStart, yStart, xEnd, yEnd));
                    }
                }
            }
        }

        private bool IsVisualCreatable(double xc, double yc, double ycz, double xMax, double yMax, double lineWidth)
        {
            double lineTolerance = lineWidth / 2.0;
            if (xc < -lineTolerance) return false;
            if (yc < -lineTolerance) return false;
            if (xc > xMax + lineTolerance) return false;
            if (ycz < 0.0 && yc < 0.0) return false;
            if (ycz > yMax && yc > ycz) return false;
            if (ycz > yMax && yc > yMax) return false;
            return true;
        }
        #endregion
    }
}