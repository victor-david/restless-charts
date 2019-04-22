using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents visual aspects that are applied to a data series.
    /// </summary>
    public class DataSeriesVisual
    {
        #region Private
        private Brush data;
        private Brush primaryText;
        private Brush secondaryText;
        private Brush border;
        private double borderThickness;
        #endregion

        /************************************************************************/

        #region Constructor
        internal DataSeriesVisual()
        {
            Data = Brushes.Black;
            PrimaryText = Brushes.White;
            SecondaryText = Brushes.White;
            Border = Brushes.DarkRed;
            BorderThickness = 2.0;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the brush used for the data of the data series.
        /// Brush usage depends on chart type.
        /// </summary>
        public Brush Data
        {
            get => data;
            set => SetBrush(ref data, value);
        }

        /// <summary>
        /// Gets or sets the brush used for the primary text of the data series.
        /// Brush usage depends on chart type.
        /// </summary>
        public Brush PrimaryText
        {
            get => primaryText;
            set => SetBrush(ref primaryText, value);
        }

        /// <summary>
        /// Gets or sets the brush used for the secondary text the data series.
        /// Brush usage depends on chart type.
        /// </summary>
        public Brush SecondaryText
        {
            get => secondaryText;
            set => SetBrush(ref secondaryText, value);
        }

        /// <summary>
        /// Gets or sets the brush used for the border of the data series.
        /// Brush usage depends on chart type.
        /// </summary>
        public Brush Border
        {
            get => border;
            set => SetBrush(ref border, value);
        }

        /// <summary>
        /// Gets or sets the thickness used for <see cref="Border"/>.
        /// This value is clamped between zero and 8.0.
        /// </summary>
        public double BorderThickness
        {
            get => borderThickness;
            set => borderThickness = value.Clamp(0.0, 8.0);
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a pen based on <see cref="Border"/> and <see cref="BorderThickness"/>.
        /// If <see cref="Border"/> is null or <see cref="BorderThickness"/> is zero,
        /// returns null.
        /// </summary>
        /// <returns>A Pen object, or null.</returns>
        public Pen GetBorderPen()
        {
            if (Border != null && BorderThickness > 0.0)
            {
                return new Pen(Border, BorderThickness);
            }
            return null;
        }
        #endregion
        
        /************************************************************************/

        #region Private methods
        private void SetBrush(ref Brush brush, Brush value)
        {
            brush = value;
            if (brush != null && brush.CanFreeze)
            {
                brush.Freeze();
            }
        }
        #endregion
    }
}
