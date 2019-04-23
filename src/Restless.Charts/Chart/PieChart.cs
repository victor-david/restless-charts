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

        /// <summary>
        /// Gets the minimum allowed value for <see cref="HoleSize"/>.
        /// </summary>
        public const double MinHoleSize = 0.0;

        /// <summary>
        /// Gets the maximum allowed value for <see cref="HoleSize"/>.
        /// </summary>
        public const double MaxHoleSize = 0.9;

        /// <summary>
        /// Gets the default value for <see cref="HoleSize"/>.
        /// </summary>
        public const double DefaultHoleSize = 0.10;

        /// <summary>
        /// Gets the minimum allowed value for <see cref="SelectedOffset"/>.
        /// </summary>
        public const double MinSelectedOffset = 10.0;

        /// <summary>
        /// Gets the maximum allowed value for <see cref="SelectedOffset"/>.
        /// </summary>
        public const double MaxSelectedOffset = 50.0;

        /// <summary>
        /// Gets the default value for <see cref="SelectedOffset"/>.
        /// </summary>
        public const double DefaultSelectedOffset = 35.0;
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
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                    null, OnCoerceStartAngle)
            );

        private static object OnCoerceStartAngle(DependencyObject d, object value)
        {
            return ((double)value).Clamp(MinStartAngle, MaxStartAngle);
        }
        #endregion

        /************************************************************************/

        #region HoleSize
        /// <summary>
        /// Gets or sets the inner hole size expressed as a percentage.
        /// This value is clamped between <see cref="MinHoleSize"/> and <see cref="MaxHoleSize"/>.
        /// </summary>
        public double HoleSize
        {
            get => (double)GetValue(HoleSizeProperty);
            set => SetValue(HoleSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HoleSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HoleSizeProperty = DependencyProperty.Register
            (
                nameof(HoleSize), typeof(double), typeof(PieChart), 
                new FrameworkPropertyMetadata(DefaultHoleSize,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, 
                    null, OnCoerceHoleSize)
            );

        private static object OnCoerceHoleSize(DependencyObject d, object value)
        {
            return ((double)value).Clamp(MinHoleSize, MaxHoleSize);
        }
        #endregion

        /************************************************************************/

        #region SelectedOffset
        /// <summary>
        /// Gets the amount that a pie piece is offset when selected.
        /// See remarks for more. This is a dependency property.
        /// </summary>
        /// <remarks>
        /// This property controls how far pushed out from the center a selected pie piece is.
        /// A pie piece may be selected by using a <see cref="ChartLegend"/> with its <see cref="ChartLegend.IsInteractive"/>
        /// property set to true, and appropiate handlers established that react to a legend selection
        /// by setting the <see cref="ChartBase.SelectedSeriesIndex"/> property.
        /// This value is clamped between <see cref="MinSelectedOffset"/> and <see cref="MaxSelectedOffset"/>.
        /// </remarks>
        public double SelectedOffset
        {
            get => (double)GetValue(SelectedOffsetProperty);
            set => SetValue(SelectedOffsetProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedOffsetProperty = DependencyProperty.Register
            (
                nameof(SelectedOffset), typeof(double), typeof(PieChart), 
                new FrameworkPropertyMetadata(DefaultSelectedOffset,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                    null, OnCoerceSelectedOffset)
            );

        private static object OnCoerceSelectedOffset(DependencyObject d, object value)
        {
            return ((double)value).Clamp(MinSelectedOffset, MaxSelectedOffset);
        }
        #endregion

        /************************************************************************/

        #region LabelDisplay
        /// <summary>
        /// Gets or sets a value that determines how labels are placed on the chart.
        /// </summary>
        public LabelDisplay LabelDisplay
        {
            get => (LabelDisplay)GetValue(LabelDisplayProperty);
            set => SetValue(LabelDisplayProperty, value);
        }
        
        public static readonly DependencyProperty LabelDisplayProperty = DependencyProperty.Register
            (
                nameof(LabelDisplay), typeof(LabelDisplay), typeof(PieChart), 
                new FrameworkPropertyMetadata(LabelDisplay.NameValue,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
            );
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
                CreateNoDataChart(size, "Zero", Data.DataInfo[0].Visual.Data);
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
            Children.Add(CreateEllipseVisual(Data.DataInfo[0].Visual.Data, null, x, y, GetRadius(x, y)));
        }

        private void CreateMultiValueChart(Size size)
        {
            double sum = Data[0].YValues.Sum;
            double radius = GetRadius(size);
            Point center = new Point(size.Width / 2.0, size.Height / 2.0);

            double innerRadius = radius * HoleSize;
            double rotationAngle = StartAngle - 90;

            int yIdx = 0;

            foreach (double value in Data[0].YValues)
            {
                double wedgeAngle = value * 360 / sum;

                double pushOffset = yIdx == SelectedSeriesIndex ? SelectedOffset : 0;
                MakeOneSlice(yIdx, center, radius, innerRadius, wedgeAngle, rotationAngle, pushOffset);
                if (LabelDisplay != LabelDisplay.None)
                {
                    double lineAngle = GetNormalizedAngle(rotationAngle + wedgeAngle / 2.0);
                    MakeOneSliceLabel(sum, value, Data.DataInfo[yIdx], center, radius + pushOffset, lineAngle);
                }

                rotationAngle += wedgeAngle;
                yIdx++;
            }
        }

        private void MakeOneSlice(int yIdx, Point center, double radius, double innerRadius, double wedgeAngle, double rotationAngle, double pushOffset)
        {
            Point innerArcStartPoint = GetCartesianCoordinate(rotationAngle, innerRadius);
            innerArcStartPoint.Offset(center.X, center.Y);

            Point innerArcEndPoint = GetCartesianCoordinate(rotationAngle + wedgeAngle, innerRadius);
            innerArcEndPoint.Offset(center.X, center.Y);

            Point outerArcStartPoint = GetCartesianCoordinate(rotationAngle, radius);
            outerArcStartPoint.Offset(center.X, center.Y);

            Point outerArcEndPoint = GetCartesianCoordinate(rotationAngle + wedgeAngle, radius);
            outerArcEndPoint.Offset(center.X, center.Y);

            bool largeArc = wedgeAngle > 180.0;

            if (pushOffset > 0)
            {
                Point offset = GetCartesianCoordinate(rotationAngle + wedgeAngle / 2.0, pushOffset);
                innerArcStartPoint.Offset(offset.X, offset.Y);
                innerArcEndPoint.Offset(offset.X, offset.Y);
                outerArcStartPoint.Offset(offset.X, offset.Y);
                outerArcEndPoint.Offset(offset.X, offset.Y);
            }

            Size outerArcSize = new Size(radius, radius);
            Size innerArcSize = new Size(innerRadius, innerRadius);

            StreamGeometry streamGeometry = new StreamGeometry();

            using (var context = streamGeometry.Open())
            {
                context.BeginFigure(innerArcStartPoint, true, true);
                context.LineTo(outerArcStartPoint, true, true);
                context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcEndPoint, true, true);
                context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArc, SweepDirection.Counterclockwise, true, true);
            }

            Pen pen = Data.DataInfo[yIdx].Visual.GetBorderPen();
            Children.Add(CreateGeometryVisual(Data.DataInfo[yIdx].Visual.Data, pen, streamGeometry));
        }

        private void MakeOneSliceLabel(double sum, double value, DataSeriesInfo info, Point center, double radius, double lineAngle)
        {
            Point startPoint = GetCartesianCoordinate(lineAngle, radius + (info.Visual.BorderThickness / 2.0) + 2.0);
            startPoint.Offset(center.X, center.Y);
            Point endPoint = GetCartesianCoordinate(lineAngle, radius * 1.075);
            endPoint.Offset(center.X, center.Y);

            StreamGeometry streamGeometry = new StreamGeometry();
            using (var context = streamGeometry.Open())
            {
                context.BeginFigure(startPoint, false, false);
                context.LineTo(endPoint, true, true);
            }

            Pen pen = info.Visual.GetPrimaryTextPen(2.0);
            Children.Add(CreateGeometryVisual(null, pen, streamGeometry));

            string labelText = GetLabelText(info.Name, sum, value);
            FormattedText text = GetFormattedText(labelText, FontFamily, FontSize, info.Visual.PrimaryText);
            text.LineHeight = text.Baseline / 2.0;
            Point offset = GetTextLabelOffset(text, lineAngle);
            endPoint.Offset(offset.X, offset.Y);
            Children.Add(CreateTextVisual(text, endPoint.X, endPoint.Y, 0));
        }

        private string GetLabelText(string seriesName, double sum, double value)
        {
            switch (LabelDisplay)
            {
                case LabelDisplay.NameValue:
                    return $"{seriesName} ({Owner.YAxis.GetFormattedValue(value)})";
                case LabelDisplay.NamePercentage:
                    return $"{seriesName} ({Owner.YAxis.GetFormattedValue(value / sum)})";
                default:
                    return seriesName;
            }
        }

        private Point GetCartesianCoordinate(double angle, double radius)
        {
            double angleRad = angle.ToRadians();
            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);
            return new Point(x, y);
        }

        private double GetRadius(double x, double y)
        {
            return Math.Min(x, y) * RadiusPercentage;
        }

        private double GetRadius(Size size)
        {
            return GetRadius(size.Width / 2.0, size.Height / 2.0);
        }

        private Point GetTextLabelOffset(FormattedText text, double lineAngle)
        {
            Point point = new Point();
            if (IsWithin(lineAngle, 0, 90)) point.Offset(4, 0);
            if (IsWithin(lineAngle, 90, 180)) point.Offset(-4 - text.Width, 0);
            if (IsWithin(lineAngle, 180, 270)) point.Offset(-4 - text.Width, -text.Height / 2.0);
            if (IsWithin(lineAngle, 270, 360)) point.Offset(4, -text.Height / 2.0);
            return point;
        }

        private bool IsWithin(double value, double min, double max)
        {
            return value > min && value <= max;
        }

        /// <summary>
        /// Gets a normalized angle. Although it doesn't matter to geometry (10 and 370 are the same)
        /// this enables us to identify the quadrant that a label is in, as that affects how we place that label.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>Angle between 0 and 360.</returns>
        private double GetNormalizedAngle(double angle)
        {
            while (angle < 0.0) angle += 360.0;
            while (angle > 360.0) angle -= 360.0;
            return angle;
        }
        #endregion
    }
}
