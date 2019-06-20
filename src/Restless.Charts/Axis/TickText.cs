using System.Windows;
using System.Windows.Controls;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents the label used on an axis.
    /// </summary>
    public class TickText : TextBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickText"/> class.
        /// </summary>
        public TickText()
        {
            TextAlignment = TextAlignment.Center;
        }
    }
}
