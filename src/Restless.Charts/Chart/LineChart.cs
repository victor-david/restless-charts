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
                nameof(LineThickness), typeof(double), typeof(LineChart), new PropertyMetadata(DefaultLineThickness, OnLinePropertyChanged, OnCoerceLineThickness)
            );

        private static object OnCoerceLineThickness(DependencyObject d, object value)
        {
            double dval = (double)value;
            return dval.Clamp(MinLineThickness, MaxLineThickness);
        }
        #endregion

        /************************************************************************/

        #region ChartStyle
        /// <summary>
        /// Gets or sets the style for the line chart.
        /// </summary>
        public LineChartStyle ChartStyle
        {
            get => (LineChartStyle)GetValue(ChartStyleProperty);
            set => SetValue(ChartStyleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ChartStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChartStyleProperty = DependencyProperty.Register
            (
                nameof(ChartStyle), typeof(LineChartStyle), typeof(LineChart), new PropertyMetadata(LineChartStyle.StandardCirclePoint, OnLinePropertyChanged)
            );

        private static void OnLinePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

            switch (ChartStyle)
            {
                case LineChartStyle.Standard:
                    CreateLines(xMax, yMax, desiredSize);
                    break;
                case LineChartStyle.StandardCirclePoint:
                case LineChartStyle.StandardSquarePoint:
                    CreatePoints(xMax, yMax, desiredSize);
                    CreateLines(xMax, yMax, desiredSize);
                    break;
                case LineChartStyle.Filled:
                    CreateFill(xMax, yMax, desiredSize);
                    break;
            }

            // CreateTextDisplayIf() adds its visuals to textVisuals. Now add them to Children.
            // This enables us to make sure text is above the lines when using multiple Y values.
            // TODO
            textVisuals.ForEach((v) => Children.Add(v));
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void CreatePoints(double xMax, double yMax, Size desiredSize)
        {
            double pointWidth = LineThickness * 8.0;

            foreach (DataPoint point in Data)
            {
                double x = Owner.XAxis.GetCoordinateFromTick(point.XValue, desiredSize);
                double yZero = Owner.YAxis.GetCoordinateFromTick(0, desiredSize);

                for (int yIdx = 0; yIdx < point.YValues.Count; yIdx++)
                {
                    Pen pen = new Pen(Data.PrimaryTextBrushes[yIdx], 1.0);
                   
                    double y = Owner.YAxis.GetCoordinateFromTick(point.YValues[yIdx], desiredSize);

                    if (IsVisualCreatable(x, y, yZero, xMax, yMax, pointWidth))
                    {
                        double xc = Owner.Orientation == Orientation.Vertical ? x : y;
                        double yc = Owner.Orientation == Orientation.Vertical ? y : x;

                        if (ChartStyle == LineChartStyle.StandardSquarePoint)
                        {
                            Children.Add(CreateRectangleVisual(Data.DataInfo[yIdx].DataBrush, pen, xc, yc, pointWidth));
                        }
                        else
                        {
                            Children.Add(CreateEllipseVisual(Data.DataInfo[yIdx].DataBrush, pen, xc, yc, pointWidth / 2.0));
                        }
                    }
                }
            }
        }

        private void CreateLines(double xMax, double yMax, Size desiredSize)
        {
            for (int yIdx = 0; yIdx < Data.MaxYSeries; yIdx++)
            {
                CreateLines(yIdx, xMax, yMax, desiredSize);
            }
        }

        private void CreateLines(int yIdx, double xMax, double yMax, Size desiredSize)
        {
            Pen pen = new Pen(Data.DataInfo[yIdx].DataBrush, LineThickness);
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
                        if (Owner.Orientation == Orientation.Vertical)
                        {
                            Children.Add(CreateLineVisual(pen, xStart, yStart, xEnd, yEnd));
                        }
                        else
                        {
                            Children.Add(CreateLineVisual(pen, yStart, xStart, yEnd, xEnd));
                        }
                    }
                }
            }
        }

        private void CreateFill(double xMax, double yMax, Size desiredSize)
        {
            for (int yIdx = 0; yIdx < Data.MaxYSeries; yIdx++)
            {
                CreateFill(yIdx, xMax, yMax, desiredSize);
            }
        }

        private void CreateFill(int yIdx, double xMax, double yMax, Size desiredSize)
        {
            if (Data.Count == 0) return;

            double yZero = Owner.YAxis.GetCoordinateFromTick(0, desiredSize);

            StreamGeometry geo = new StreamGeometry();

            using (StreamGeometryContext gc = geo.Open())
            {
                double x = Owner.XAxis.GetCoordinateFromTick(Data[0].XValue, desiredSize);
                double y = 0;

                Point pt = new Point(x, yZero);
                if (Owner.Orientation == Orientation.Horizontal)
                {
                    pt = pt.SwapXY();
                }

                gc.BeginFigure(pt, true, true);

                foreach (DataPoint point in Data)
                {
                    x = Owner.XAxis.GetCoordinateFromTick(point.XValue, desiredSize);
                    y = Owner.YAxis.GetCoordinateFromTick(point.YValues[yIdx], desiredSize);

                    pt.X = x;
                    pt.Y = y;

                    if (Owner.Orientation == Orientation.Horizontal)
                    {
                        pt = pt.SwapXY();
                    }
                    gc.LineTo(pt, true, true);
                }

                if (Owner.Orientation == Orientation.Vertical)
                    pt.Y = yZero;
                else
                    pt.X = yZero;

                gc.LineTo(pt, true, true);
            }

            geo.Freeze();

            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawGeometry(Data.DataInfo[yIdx].DataBrush, null, geo);
            }

            Children.Add(visual);
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