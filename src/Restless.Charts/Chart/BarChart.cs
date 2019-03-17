using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        //private Path geometryPath;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default brush for the border of the grid axis.
        /// </summary>
        public static readonly Brush DefaultBarBrush = Brushes.Gray;
        /// <summary>
        /// Gets the default bar thickness.
        /// </summary>
        public const double DefaultBarThickness = 8.0;
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
        ///// <summary>
        ///// Gets or sets the brush to use to draw the bars
        ///// </summary>
        //public Brush BarBrush
        //{
        //    get => (Brush)GetValue(BarBrushProperty);
        //    set => SetValue(BarBrushProperty, value);
        //}

        ///// <summary>
        ///// Identifies the <see cref="BarBrush"/> dependency property.
        ///// </summary>
        //public static readonly DependencyProperty BarBrushProperty = DependencyProperty.Register
        //    (
        //        nameof(BarBrush), typeof(Brush), typeof(BarChart), new PropertyMetadata(DefaultBarBrush, OnBarBrushChanged)
        //    );

        //private static void OnBarBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is BarChart c)
        //    {
        //        c.geometryPath.Stroke = c.BarBrush;
        //    }
        //}

        /// <summary>
        /// Gets or sets the thickness of the bars
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
            return dval.Clamp(1, 100);
        }

        private static void OnBarThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BarChart c)
            {
                //c.geometryPath.StrokeThickness = c.BarThickness;
            }
        }
        #endregion

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

            int seriesCount = Data.Count;

            foreach (DataSeries series in Data)
            {
                Path path = new Path()
                {
                    Stroke = series.Brush,
                    StrokeThickness = BarThickness,
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
    }
}
