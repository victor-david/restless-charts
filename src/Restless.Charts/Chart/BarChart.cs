using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
        public const double DefaultBarThickness = 30.0;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BarChart"/> class.
        /// </summary>
        public BarChart()
        {
            //geometryPath = new Path()
            //{
            //    Stroke = DefaultBarBrush,
            //    StrokeThickness = DefaultBarThickness,
            //};
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
        /// Positions children of this element.
        /// </summary>
        /// <param name="finalSize">The final area within the parent this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }
            return finalSize;
        }

        /// <summary>
        /// Called during the measuring phase to create the geometry for this control.
        /// </summary>
        /// <param name="desiredSize">The desired size.</param>
        protected override void CreateChildGeometry(Size desiredSize)
        {
            GeometryGroup group = new GeometryGroup();

            double barThickness = BarThickness > 0 ? BarThickness : GetAutoBarThickness(Data, desiredSize);

            foreach (DataSeries series in Data)
            {
                Path path = new Path()
                {
                    Stroke = series.Brush,
                    StrokeThickness = barThickness,
                };

                foreach (DataPoint point in series)
                {
                    LineGeometry line = new LineGeometry();
                    if (Owner.Orientation == Orientation.Vertical)
                    {
                        SetLineGeometryVertical(point, line, desiredSize);
                    }
                    else
                    {
                        SetLineGeometryHorizontal(point, line, desiredSize);

                    }
                    group.Children.Add(line);
                }

                path.Data = group;
                Children.Add(path);
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods

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
            //Debug.WriteLine("-----------------------------------");
            //Debug.WriteLine($"S1 {s1} SX {sx} Distance {distance} Count {series.Count} Result {result} DesiredSize {desiredSize}");
            return result;
        }

        /// <summary>
        /// Sets line geometry when the chart container is in vertical orientation.
        /// </summary>
        /// <param name="point">The data point</param>
        /// <param name="line">The line object</param>
        /// <param name="desiredSize">The desired size</param>
        private void SetLineGeometryVertical(DataPoint point, LineGeometry line, Size desiredSize)
        {
            double xc = Owner.XAxis.GetCoordinateFromTick(point.XValue, desiredSize);
            double yc = Owner.YAxis.GetCoordinateFromTick(point.YValue, desiredSize);
            double ycz = Owner.YAxis.GetCoordinateFromTick(0, desiredSize);

            line.StartPoint = new Point(xc, ycz);
            line.EndPoint = new Point(xc, yc);
        }

        /// <summary>
        /// Sets line geometry when the chart container is in horizontal orientation.
        /// </summary>
        /// <param name="point">The data point</param>
        /// <param name="line">The line object</param>
        /// <param name="desiredSize">The desired size</param>
        private void SetLineGeometryHorizontal(DataPoint point, LineGeometry line, Size desiredSize)
        {
            double xc = Owner.XAxis.GetCoordinateFromTick(point.XValue, desiredSize);
            double yc = Owner.YAxis.GetCoordinateFromTick(point.YValue, desiredSize);
            double ycz = Owner.YAxis.GetCoordinateFromTick(0, desiredSize);

            line.StartPoint = new Point(ycz, xc);
            line.EndPoint = new Point(yc, xc);
        }
        #endregion
    }
}
