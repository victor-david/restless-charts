using System;
using System.Collections.Generic;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a collection of <see cref="MajorTick"/> objects.
    /// </summary>
    public class MajorTickCollection : List<MajorTick>
    {


        public bool Contains(double value)
        {
            foreach (MajorTick tick in this)
            {
                if (tick.Value == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a size struct that represents the maximum width and maximum height of all major tick labels.
        /// </summary>
        /// <returns>A size structure.</returns>
        public Size GetMaxTextSize()
        {
            Size size = new Size();
            foreach (MajorTick tick in this)
            {
                size.Width = Math.Max(size.Width, tick.TextWidth);
                size.Height = Math.Max(size.Height, tick.TextHeight);
            }
            return size;
        }

        public double GetTotalTextSpace()
        {
            return 0;
        }
    }
}
