using System.Windows;
using System.Windows.Controls;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents the base class for all charts. This class must be inherited.
    /// </summary>
    public abstract class ChartBase : Panel
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
            Loaded += (s, e) => TreeHelper.TrySetParent(this, ref owner);
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
                    c.Owner.XAxis.Range = c.Data.DataRange.X;
                    c.Owner.YAxis.Range = c.Data.DataRange.Y;
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
                CreateChildGeometry(desiredSize);
            }

            foreach (UIElement child in Children)
            {
                child.Measure(desiredSize);
            }

            return desiredSize;
        }

        /// <summary>
        /// Called during the measure pass to create child geometry objects.
        /// Implementors can use <see cref="Data"/> and <see cref="Owner"/> without a null check.
        /// This method is called only if these properties are non null.
        /// </summary>
        /// <param name="desiredSize"></param>
        protected abstract void CreateChildGeometry(Size desiredSize);

        #endregion

        /************************************************************************/

        #region Private methods
        #endregion

    }
}
