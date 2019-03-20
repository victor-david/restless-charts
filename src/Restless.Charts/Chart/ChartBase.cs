using System;
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
        public DataSeriesCollection Data
        {
            get => (DataSeriesCollection)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Data"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register
            (
                nameof(Data), typeof(DataSeriesCollection), typeof(ChartBase), new PropertyMetadata(null, OnDataChanged)
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
            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext dc = visual.RenderOpen();
            dc.DrawLine(pen, new Point(startX, startY), new Point(endX, endY));
            //double radius = pen.Thickness / 4.0;

            //dc.DrawEllipse(Brushes.Red, null, new Point(endX, endY - radius), radius, radius);
            // Persist the drawing content.
            dc.Close();
            return visual;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        #endregion

    }
}
