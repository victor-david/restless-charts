using System;
using System.Diagnostics;
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
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default brush for the border of the grid axis.
        /// </summary>
        public static readonly Brush DefaultBarBrush = Brushes.Gray;
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
        }
        #endregion
        
        /************************************************************************/

        #region Brush
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
                nameof(BarThickness), typeof(double), typeof(BarChart), new PropertyMetadata(DefaultBarThickness, OnBarThicknessPropertyChanged, OnCoerceBarThickness)
            );

        private static object OnCoerceBarThickness(DependencyObject d, object value)
        {
            double dval = (double)value;
            return dval.Clamp(0, 100);
        }

        private static void OnBarThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

            foreach (DataSeries series in Data)
            {
                //EvaluateSeries(series);

                foreach (DataPoint point in series)
                {
                    Pen pen = new Pen(series.Brush, barWidth);
                    double xc = Owner.XAxis.GetCoordinateFromTick(point.XValue, desiredSize);
                    double yc = Owner.YAxis.GetCoordinateFromTick(point.YValue, desiredSize);
                    double ycz = Owner.YAxis.GetCoordinateFromTick(0, desiredSize);

                    if (IsVisualCreatable(xc, yc, ycz, xMax, yMax, barWidth))
                    {
                        if (Owner.Orientation == Orientation.Vertical)
                        {
                            Children.Add(CreateLineVisual(pen, xc, ycz, xc, yc));
                        }
                        else
                        {
                            Children.Add(CreateLineVisual(pen, ycz, xc, yc, xc));
                        }
                    }
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods

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

        private double GetAutoBarThickness(DataSeriesCollection seriesCollection, Size desiredSize)
        {
            double result = double.PositiveInfinity;
            foreach(DataSeries series in seriesCollection)
            {
                double autoSeries = Math.Abs(GetAutoBarThickness(series, desiredSize));
                result = (autoSeries > 0) ? Math.Min(result, autoSeries) : result;
            }
            return result.IsFinite() ? result : 10.0;
        }

        private double GetAutoBarThickness(DataSeries series, Size desiredSize)
        {
            if (series.Count == 0) return 0;
            double s1 = Owner.XAxis.GetCoordinateFromTick(series[0].XValue, desiredSize);
            double sx = Owner.XAxis.GetCoordinateFromTick(series[series.Count - 1].XValue, desiredSize);
            double distance = sx - s1;

            if (distance == 0.0) return Owner.XAxis.IsHorizontal ? desiredSize.Width / 2.0 : desiredSize.Height / 2.0;

            double result = distance / series.Count;
            return result;
        }

        private void EvaluateSeries(DataSeries series)
        {
            double minDiff = double.MaxValue;
            double maxDiff = double.MinValue;
            double lastX = double.NaN;

            foreach (DataPoint point in series)
            {
                if (!double.IsNaN(lastX))
                {
                    double diff = point.XValue - lastX;
                    minDiff = Math.Min(minDiff, diff);
                    maxDiff = Math.Max(maxDiff, diff);
                }
                lastX = point.XValue;
            }
            Debug.WriteLine($"Series has {series.Count} values. Min Diff: {minDiff} MaxDiff: {maxDiff}");
        }
        #endregion
    }
}
