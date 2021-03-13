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
        private bool dataChangedEventInProgress;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default value font family.
        /// </summary>
        public const string DefaultValueFontFamily = "Verdana";

        /// <summary>
        /// Gets the minimum allowed value for <see cref="FontSize"/>.
        /// </summary>
        public const double MinValuesFontSize = 8.0;

        /// <summary>
        /// Gets the maximum allowed value for <see cref="FontSize"/>.
        /// </summary>
        public const double MaxValuesFontSize = 32.0;

        /// <summary>
        /// Gets the default value for <see cref="FontSize"/>.
        /// </summary>
        public const double DefaultValuesFontSize = 13.0;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="ChartBase"/> class
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
                nameof(Data), typeof(DataSeries), typeof(ChartBase), new FrameworkPropertyMetadata(null, OnDataChanged)
            );


        /// <summary>
        /// Provides notification when the <see cref="Data"/> property changes.
        /// </summary>
        public event RoutedEventHandler DataChanged
        {
            add => AddHandler(DataChangedEvent, value);
            remove => RemoveHandler(DataChangedEvent, value);
        }

        /// <summary>
        /// Identifies the <see cref="DataChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent DataChangedEvent = EventManager.RegisterRoutedEvent
            (
                nameof(DataChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ChartBase)
            );

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartBase c)
            {
                // avoid reentrancy
                if (!c.dataChangedEventInProgress)
                {
                    c.dataChangedEventInProgress = true;
                    c.RaiseEvent(new RoutedEventArgs(DataChangedEvent));
                    c.dataChangedEventInProgress = false;
                }

                if (c.Data != null && TreeHelper.TrySetParent(c, ref c.owner))
                {
                    c.Owner.XAxis.SetData(c.Data);
                    c.Owner.YAxis.SetData(c.Data);
                    c.Owner.InvalidateMeasure();
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Font
        /// <summary>
        /// Gets or sets the name of the font family to use.
        /// </summary>
        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register
            (
                nameof(FontFamily), typeof(string), typeof(ChartBase), 
                new FrameworkPropertyMetadata(DefaultValueFontFamily, FrameworkPropertyMetadataOptions.AffectsMeasure)
            );

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <remarks>
        /// This value is clamped between <see cref="MinValuesFontSize"/> and <see cref="MaxValuesFontSize"/>.
        /// </remarks>
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="FontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
            (
                nameof(FontSize), typeof(double), typeof(ChartBase), 
                new FrameworkPropertyMetadata(DefaultValuesFontSize, FrameworkPropertyMetadataOptions.AffectsMeasure, null, OnCoerceFontSize)
            );

        private static object OnCoerceFontSize(DependencyObject d, object value)
        {
            double dval = (double)value;
            return dval.Clamp(MinValuesFontSize, MaxValuesFontSize);
        }
        #endregion

        /************************************************************************/

        #region SelectedSeriesIndex
        /// <summary>
        /// Gets or sets the selected series index.
        /// </summary>
        public int SelectedSeriesIndex
        {
            get => (int)GetValue(SelectedSeriesIndexProperty);
            set => SetValue(SelectedSeriesIndexProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedSeriesIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedSeriesIndexProperty = DependencyProperty.Register
            (
                nameof(SelectedSeriesIndex), typeof(int), typeof(ChartBase), 
                new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsMeasure)
            );
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
        /// <param name="constraint">
        /// The available size that this element can give to child elements.
        /// Infinity can be specified as a value to indicate that the element will size to whatever content is available.
        /// </param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            Size desiredSize = new Size
                (
                    constraint.Width.IsFinite() ? constraint.Width : 512,
                    constraint.Height.IsFinite() ? constraint.Height : 512
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
        /// <param name="opacity">The opacity to apply to the visual.</param>
        /// <returns>The visual</returns>
        protected DrawingVisual CreateLineVisual(Pen pen, double startX, double startY, double endX, double endY, double opacity = 1.0)
        {
            DrawingVisual visual = new DrawingVisual()
            {
                Opacity = opacity
            };

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
        /// <param name="opacity">The opacity to apply to the visual.</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateEllipseVisual(Brush brush, Pen pen, double x, double y, double radius, double opacity = 1.0)
        {
            DrawingVisual visual = new DrawingVisual()
            {
                Opacity = opacity
            };

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
        /// <param name="opacity">The opacity to apply to the visual.</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateRectangleVisual(Brush brush, Pen pen, double x, double y, double width, double height, double opacity = 1.0)
        {
            DrawingVisual visual = new DrawingVisual()
            {
                Opacity = opacity
            };

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
        /// <param name="opacity">The opacity to apply to the visual.</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateRectangleVisual(Brush brush, Pen pen, double x, double y, double size, double opacity = 1.0)
        {
            return CreateRectangleVisual(brush, pen, x, y, size, size, opacity);
        }

        /// <summary>
        /// Creates a text visual at the specified location and rotation.
        /// </summary>
        /// <param name="text">The formatted text object.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="opacity">The opacity to apply to the visual.</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateTextVisual(FormattedText text, double x, double y, double rotation, double opacity = 1.0)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            DrawingVisual visual = new DrawingVisual()
            {
                Opacity = opacity
            };

            double rx = x + text.Width / 2.0;
            double ry = y + text.Height / 2.0;

            RotateTransform rotateTransform = new RotateTransform(rotation, rx, ry);

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.PushTransform(rotateTransform);
                // visual debugging aides
                //dc.DrawEllipse(Brushes.Red, null, new Point(rx, ry), 3, 3);
                //dc.DrawEllipse(Brushes.Red, null, new Point(x, y), 1, 1);
                dc.DrawText(text, new Point(x, y));
                dc.Pop();
            }
            return visual;
        }

        /// <summary>
        /// Creates a <see cref="DrawingVisual"/> from the specified geometry
        /// </summary>
        /// <param name="brush">The brush to fill the geometry</param>
        /// <param name="pen">A pen to outline the geometry, or null</param>
        /// <param name="geometry">The geometry to use to create the visual.</param>
        /// <param name="opacity">The opacity to apply to the visual.</param>
        /// <returns>A drawing visual.</returns>
        protected DrawingVisual CreateGeometryVisual(Brush brush, Pen pen, Geometry geometry, double opacity = 1.0)
        {
            DrawingVisual visual = new DrawingVisual()
            {
                Opacity = opacity
            };
            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawGeometry(brush, pen, geometry);
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
#pragma warning disable CS0618 // Type or member is obsolete
            FormattedText ftext = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(fontFamilyName), fontSize, brush);
#pragma warning restore CS0618 // Type or member is obsolete
            return ftext;
        }
        #endregion
    }
}
