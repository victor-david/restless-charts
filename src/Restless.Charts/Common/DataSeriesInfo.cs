using System;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents information for a single data series.
    /// </summary>
    public class DataSeriesInfo
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeriesInfo"/> class.
        /// </summary>
        /// <param name="name">The name of the data series.</param>
        /// <param name="dataBrush">The brush associated with the data series.</param>
        internal DataSeriesInfo(string name, Brush dataBrush)
        {
            Name = string.IsNullOrEmpty(name) ? "Unnamed" : name;
            DataBrush = dataBrush ?? throw new ArgumentNullException(nameof(dataBrush));
            if (DataBrush.CanFreeze)
            {
                DataBrush.Freeze();
            }
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the name of the data series.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Gets the brush associated with the data series.
        /// </summary>
        public Brush DataBrush
        {
            get;
        }
        #endregion
    }
}
