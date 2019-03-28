using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents the base class for all charts. This class must be inherited.
    /// </summary>
    public abstract class ChartBase : FrameworkElement
    {
        #region Private
        private ChartContainer owner;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default value font family.
        /// </summary>
        public const string DefaultValueFontFamily = "Verdana";

        /// <summary>
        /// Gets the minimum allowed value for <see cref="ValuesFontSize"/>.
        /// </summary>
        public const double MinValuesFontSize = 8.0;

        /// <summary>
        /// Gets the maximum allowed value for <see cref="ValuesFontSize"/>.
        /// </summary>
        public const double MaxValuesFontSize = 32.0;

        /// <summary>
        /// Gets the default value for <see cref="ValuesFontSize"/>.
        /// </summary>
        public const double DefaultValuesFontSize = 13.0;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="ChartBase"/> class
        /// </summary>
        protected ChartBase()
        {
            RenderTransformOrigin = new Point(0.5, 0.5);
            Children = new VisualCollection(this);
            Loaded += (s, e) => TreeHelper.TrySetParent(this, ref owner);
        }
        #endregion

        /************************************************************************/

        #region Children
        /// <summary>
        /// Gets the collection of child visuals.
        /// </summary>
        public VisualCollection Children
        {
            get;
        }

        /// <summary>
        /// Gets the count of visuals in <see cref="Children"/>
        /// </summary>
        protected override int VisualChildrenCount
        {
            get => Children.Count;
        }
        #endregion

        /************************************************************************/

        #region Owner
        /// <summary>
        /// Gets the <see cref="ChartContainer"/> that owns this plot.
        /// </summary>
        protected ChartContainer Owner
        {
            get => owner;
        }
        #endregion

        /************************************************************************/

        #region Data
        /// <summary>
        /// Gets or sets the data series
        /// </summary>
        public DataSeries Data
        {
            get => (DataSeries)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Data"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register
            (
                nameof(Data), typeof(DataSeries), typeof(ChartBase), new PropertyMetadata(null, OnDataChanged)
            );

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartBase c && c.Data != null)
            {
                if (TreeHelper.TrySetParent(c, ref c.owner))
                {
                    c.Owner.XAxis.SetRange(c.Data.DataRange.X);
                    c.Owner.YAxis.SetRange(c.Data.DataRange.Y);
                    c.Owner.XAxis.Range.CreateSnapshot();
                    c.Owner.YAxis.Range.CreateSnapshot();
                    c.Owner.InvalidateMeasure();
                }
            }
        }
        #endregion

        /************************************************************************/

        #region DisplayValues
        /// <summary>
        /// Gets or sets a value that determines if Y axis values are displayed inside the chart.
        /// </summary>
        public bool DisplayValues
        {
            get => (bool)GetValue(DisplayValuesProperty);
            set => SetValue(DisplayValuesProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DisplayValues"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayValuesProperty = DependencyProperty.Register
            (
                nameof(DisplayValues), typeof(bool), typeof(ChartBase), new PropertyMetadata(false, OnDisplayValuesPropertyChanged)
            );

        /// <summary>
        /// Gets or sets the name of the font family to use when <see cref="DisplayValues"/> is true.
        /// </summary>
        public string ValuesFontFamily
        {
            get => (string)GetValue(ValuesFontFamilyProperty);
            set => SetValue(ValuesFontFamilyProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ValuesFontFamily"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValuesFontFamilyProperty = DependencyProperty.Register
            (
                nameof(ValuesFontFamily), typeof(string), typeof(ChartBase), new PropertyMetadata(DefaultValueFontFamily, OnDisplayValuesPropertyChanged)
            );

        /// <summary>
        /// Gets or sets the size of the font when <see cref="DisplayValues"/> is true.
        /// </summary>
        /// <remarks>
        /// This value is clamped between <see cref="MinValuesFontSize"/> and <see cref="MaxValuesFontSize"/>.
        /// </remarks>
        public double ValuesFontSize
        {
            get => (double)GetValue(ValuesFontSizeProperty);
            set => SetValue(ValuesFontSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ValuesFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValuesFontSizeProperty = DependencyProperty.Register
            (
                nameof(ValuesFontSize), typeof(double), typeof(ChartBase), new PropertyMetadata(DefaultValuesFontSize, OnDisplayValuesPropertyChanged, OnCoerceValuesFontSize)
            );

        private static object OnCoerceValuesFontSize(DependencyObject d, object value)
        {
            double dval = (double)value;
            return dval.Clamp(MinValuesFontSize, MaxValuesFontSize);
        }

        private static void OnDisplayValuesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartBase c)
            {
                c.InvalidateMeasure();
            }
        }
        #endregion

        /************************************************************************/

        #region Public methods
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Gets the visual child at the specified index.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The visual at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of bounds.</exception>
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= Children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            return Children[index];
        }

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

            Children.Clear();
            if (Data != null && Owner != null)
            {
                CreateChildren(desiredSize);
            }

            return desiredSize;
        }

        /// <summary>
        /// Called during the measure pass to create child geometry objects.
        /// Implementors can use <see cref="Data"/> and <see cref="Owner"/> without a null check.
        /// This method is called only if these properties are non null.
        /// </summary>
        /// <param name="desiredSize">The desired size</param>
        protected abstract void CreateChildren(Size desiredSize);

        /// <summary>
        /// Creates a line visual.
        /// </summary>
        /// <param name="pen">The pen to use.</param>
        /// <param name="startX">The X start coordinate.</param>
        /// <param name="startY">The Y start coordinate.</param>
        /// <param name="endX">The X end coordinate.</param>
        /// <param name="endY">The Y end coordinate.</param>
        /// <returns>The visual</returns>
        protected DrawingVisual CreateLineVisual(Pen pen, double startX, double startY, double endX, double endY)
        {
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawLine(pen, new Point(startX, startY), new Point(endX, endY));
            }
            return visual;
        }

        /// <summary>
        /// Creates an ellispe visual.
        /// </summary>
        /// <param name="brush">The fill brush. May be null for no fill.</param>
        /// <param name="pen">The outline pen. May be null for no outline.</param>
        /// <param name="x">The X center of the ellipse.</param>
        /// <param name="y">The Y center of the ellipse.</param>
        /// <param name="radius">The radius</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateEllipseVisual(Brush brush, Pen pen, double x, double y, double radius)
        {
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                double halfPenWidth = 0;
                if (pen != null) halfPenWidth = pen.Thickness / 2.0;

                GuidelineSet guidelines = new GuidelineSet();
                guidelines.GuidelinesX.Add(x + radius + halfPenWidth);
                guidelines.GuidelinesX.Add(x - radius + halfPenWidth);
                guidelines.GuidelinesY.Add(y + radius + halfPenWidth);
                guidelines.GuidelinesY.Add(y - radius + halfPenWidth);
                dc.PushGuidelineSet(guidelines);
                dc.DrawEllipse(brush, pen, new Point(x, y), radius, radius);
                dc.Pop();
            }
            return visual;
        }

        /// <summary>
        /// Creates a rectangle visual.
        /// </summary>
        /// <param name="brush">The fill brush. May be null for no fill.</param>
        /// <param name="pen">The outline pen. May be null for no outline.</param>
        /// <param name="x">The X coordinate of the rectangle. The rectange will be centered on this value.</param>
        /// <param name="y">The Y coordinate of the rectangle. The rectange will be centered on this value.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectange.</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateRectangleVisual(Brush brush, Pen pen, double x, double y, double width, double height)
        {
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                double halfPenWidth = 0;
                if (pen != null) halfPenWidth = pen.Thickness / 2.0;

                Rect rect = new Rect(x - (width / 2), y - (height / 2), width, height);

                GuidelineSet guidelines = new GuidelineSet();
                guidelines.GuidelinesX.Add(rect.Left + halfPenWidth);
                guidelines.GuidelinesX.Add(rect.Right + halfPenWidth);
                guidelines.GuidelinesY.Add(rect.Top + halfPenWidth);
                guidelines.GuidelinesY.Add(rect.Bottom + halfPenWidth);
                dc.PushGuidelineSet(guidelines);
                dc.DrawRectangle(brush, pen, rect);
                dc.Pop();
            }
            return visual;
        }

        /// <summary>
        /// Creates a rectangle visual of equal width and height.
        /// </summary>
        /// <param name="brush">The fill brush. May be null for no fill.</param>
        /// <param name="pen">The outline pen. May be null for no outline.</param>
        /// <param name="x">The X coordinate of the rectangle. The rectange will be centered on this value.</param>
        /// <param name="y">The Y coordinate of the rectangle. The rectange will be centered on this value.</param>
        /// <param name="size">The width and height of the rectangle.</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateRectangleVisual(Brush brush, Pen pen, double x, double y, double size)
        {
            return CreateRectangleVisual(brush, pen, x, y, size, size);
        }

        /// <summary>
        /// Creates a text visual at the specified location and rotation.
        /// </summary>
        /// <param name="text">The formatted text object.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateTextVisual(FormattedText text, double x, double y, double rotation)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            DrawingVisual visual = new DrawingVisual();

            double rx = x + text.Width / 2.0;
            double ry = y + text.Height / 2.0;

            RotateTransform rotateTransform = new RotateTransform(rotation, rx, ry);

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.PushTransform(rotateTransform);
                //dc.DrawEllipse(Brushes.Red, null, new Point(rx, ry), 3, 3);
                dc.DrawText(text, new Point(x, y));
                dc.Pop();
            }
            return visual;
        }

        /// <summary>
        /// Gets a formatted text object.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamilyName">The font family name. Can be null to use the default.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="brush">The brush for the text.</param>
        /// <returns>A formatted text object.</returns>
        protected FormattedText GetFormattedText(string text, string fontFamilyName, double fontSize, Brush brush)
        {
            if (string.IsNullOrEmpty(fontFamilyName)) fontFamilyName = DefaultValueFontFamily;
            FormattedText ftext = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(fontFamilyName), fontSize, brush);
            return ftext;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        #endregion
    }
}
