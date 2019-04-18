using System;
using System.Windows;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a pie chart that displays slices as percentages of the overall total.
    /// </summary>
    /// <remarks>
    /// As with other charts, a PieChart uses a <see cref="DataSeries"/> object to obtains its values.
    /// However, unlike other charts, PieChart uses only the <see cref="DataSequence"/> provided by the first
    /// <see cref="DataPoint"/>. The value of <see cref="DataPoint.XValue"/> is not used. To create data for
    /// PieChart, create a <see cref="DataSeries"/> with the number of Y series set to the number of pie slices needed.
    /// Call <see cref="DataSeries.Add(double, double)"/> for each pie value, passing the same X value each time.
    /// </remarks>
    public class PieChart : ChartBase
    {
        #region Public Fields
        /// <summary>
        /// Gets the minimum value allowed for <see cref="RadiusPercentage"/>.
        /// </summary>
        public const double MinRadiusPercentage = 0.25;

        /// <summary>
        /// Gets the maximum value allowed for <see cref="RadiusPercentage"/>.
        /// </summary>
        public const double MaxRadiusPercentage = 1.0;

        /// <summary>
        /// Gets the default value for <see cref="RadiusPercentage"/>.
        /// </summary>
        public const double DefaultRadiusPercentage = 0.75;

        /// <summary>
        /// Gets the minimum allowed value for <see cref="StartAngle"/>.
        /// </summary>
        public const double MinStartAngle = -180.0;

        /// <summary>
        /// Gets the maximum allowed value for <see cref="StartAngle"/>.
        /// </summary>
        public const double MaxStartAngle = 180.0;

        /// <summary>
        /// Gets the default value for <see cref="StartAngle"/>.
        /// </summary>
        public const double DefaultStartAngle = 45.0;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChart"/> class.
        /// </summary>
        public PieChart()
        {
        }
        #endregion

        /************************************************************************/

        #region RadiusPercentage
        /// <summary>
        /// Gets or sets the percentage used to create the radius of the pie chart.
        /// </summary>
        /// <remarks>
        /// This property controls how large the pie chart is relative to its available space.
        /// A value of 1.0 fills the entire space (based on the smaller dimension between width and height),
        /// a value of 0.5 half the space, etc.
        /// This value is clamped between <see cref="MinRadiusPercentage"/> and <see cref="MaxRadiusPercentage"/> inclusive.
        /// </remarks>
        public double RadiusPercentage
        {
            get => (double)GetValue(RadiusPercentageProperty);
            set => SetValue(RadiusPercentageProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="RadiusPercentage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RadiusPercentageProperty = DependencyProperty.Register
            (
                nameof(RadiusPercentage), typeof(double), typeof(PieChart),
                new FrameworkPropertyMetadata(DefaultRadiusPercentage,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                    null, OnCoerceRadiusPercentage)
            );

        private static object OnCoerceRadiusPercentage(DependencyObject d, object value)
        {
            return ((double)value).Clamp(MinRadiusPercentage, MaxRadiusPercentage);
        }
        #endregion

        /************************************************************************/

        #region StartAngle
        /// <summary>
        /// Gets or sets a value that determines the starting angle in degrees for the first pie slice.
        /// </summary>
        /// <remarks>
        /// A value of zero indicates that the border of the first pie slice will come from the center of the pie 
        /// and go vertically towards 12 oclock. A value of 90 goes horizonally towards 3 oclock, 
        /// 180 vertically towards 6 oclock, -90 horizontally towards 9 oclock, etc.
        /// This value is clamped between <see cref="MinStartAngle"/> and <see cref="MaxStartAngle"/> inclusive.
        /// </remarks>
        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register
            (
                nameof(StartAngle), typeof(double), typeof(PieChart), 
                new FrameworkPropertyMetadata(DefaultStartAngle,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                    null, OnCoerceStartAngle)
            );

        private static object OnCoerceStartAngle(DependencyObject d, object value)
        {
            return ((double)value).Clamp(MinStartAngle, MaxStartAngle);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called by <see cref="ChartBase"/> to create child visuals.
        /// </summary>
        /// <param name="size">The size in which the chart must fit</param>
        protected override void CreateChildren(Size size)
        {
            if (Data.Count == 0)
            {
                CreateNoDataChart(size, "No Data", Brushes.Blue);
                return;
            }

            if (Data[0].YValues.Sum == 0)
            {
                CreateNoDataChart(size, "Zero", Data.DataInfo[0].DataBrush);
                return;
            }

            if (Data[0].YValues.Count == 1)
            {
                CreateSingleValueChart(size);
                return;
            }

            // most common code path.
            CreateMultiValueChart(size);

        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void CreateNoDataChart(Size size, string message, Brush brush)
        {
            double x = size.Width / 2.0;
            double y = size.Height / 2.0;
            Pen pen = new Pen(brush, 2.0);
            pen.Freeze();
            Children.Add(CreateEllipseVisual(null, pen, x, y, GetRadius(x, y)));

            FormattedText text = GetFormattedText(message, null, 16.0, brush);
            x -= text.Width / 2.0;
            y -= text.Height / 2.0;
            Children.Add(CreateTextVisual(text, x, y, 0));
        }

        private void CreateSingleValueChart(Size size)
        {
            double x = size.Width / 2.0;
            double y = size.Height / 2.0;
            Children.Add(CreateEllipseVisual(Data.DataInfo[0].DataBrush, null, x, y, GetRadius(x, y)));
        }

        private void CreateMultiValueChart(Size size)
        {
            double sum = Data[0].YValues.Sum;
            double angle = DegreeToRadian(StartAngle - 90);

            double radius = GetRadius(size);
            Point center = new Point(size.Width / 2.0, size.Height / 2.0);

            int yIdx = 0;

            // Data[0] exists or this method is not called.
            foreach (double value in Data[0].YValues)
            {
                // PathFigure and segments are created in a way that puts the chart in the upper left of the available space.
                // PathGeometry gets a transform that pushes the geometry into the center of the space.
                PathGeometry pathGeometry = new PathGeometry()
                {
                    Transform = new TranslateTransform(center.X - radius, center.Y - radius)
                };

                PathFigure figure = new PathFigure()
                {
                    IsClosed = true,
                    StartPoint = new Point(radius, radius)
                };

                double x = Math.Cos(angle) * radius + radius;
                double y = Math.Sin(angle) * radius + radius;

                // 1. Line segment from center of pie to its outer limit. 
                figure.Segments.Add(new LineSegment(new Point(x, y), true));

                // 2. Arc segment to curve around the outer limit of the pie.
                double angleShare = value / sum * 360;
                angle += DegreeToRadian(angleShare);
                x = Math.Cos(angle) * radius + radius;
                y = Math.Sin(angle) * radius + radius;
                ArcSegment arcSeg = new ArcSegment(new Point(x, y), new Size(radius, radius), angleShare, angleShare > 180, SweepDirection.Clockwise, true);
                figure.Segments.Add(arcSeg);

                // 3. Line segment back to center
                figure.Segments.Add(new LineSegment(new Point(radius, radius), true));
               
                // add the figure and create the child visual.
                pathGeometry.Figures.Add(figure);
                Children.Add(CreateGeometryVisual(Data.DataInfo[yIdx].DataBrush, null, pathGeometry));

                yIdx++;
            }
        }

        private double GetRadius(double x, double y)
        {
            return Math.Min(x, y) * RadiusPercentage;
        }

        private double GetRadius(Size size)
        {
            return GetRadius(size.Width / 2.0, size.Height / 2.0);
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        #endregion
    }
}
