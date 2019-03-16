using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Describes a width, a height and location of a rectangle.
    /// This type is very similar to <see cref="Rect"/>, but has one important difference:
    /// while <see cref="Rect"/> describes a rectangle in screen coordinates, where y axis
    /// points to bottom (that's why <see cref="Rect"/>'s Bottom property is greater than Top).
    /// This type describes rectange in usual coordinates, and y axis point to top.
    /// </summary>
    public class DataRange
    {
        /// <summary>
        /// Gets an empty <see cref="DataRange"/>.
        /// </summary>
        public static DataRange EmptyDataRange()
        {
            return new DataRange(Range.EmptyRange(), Range.EmptyRange());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRange"/> struct.
        /// </summary>
        /// <param name="minX">Left value of DataRect.</param>
        /// <param name="minY">Bottom value of DataRect.</param>
        /// <param name="maxX">Right value of DataRect.</param>
        /// <param name="maxY">Top value of DataRect.</param>
        public DataRange(double minX, double minY, double maxX, double maxY)
        {
            X = new Range(minX, maxX);
            Y = new Range(minY, maxY);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRange"/> struct.
        /// </summary>
        /// <param name="x">Horizontal range</param>
        /// <param name="y">Vertical range</param>
        public DataRange(Range x, Range y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the X range.
        /// </summary>
        public Range X
        {
            get;
            //set;
        }

        /// <summary>
        /// Gets the Y range.
        /// </summary>
        public Range Y
        {
            get;
        }

        /// <summary>
        /// Gets the width of <see cref="DataRange"/>,
        /// i.e. the difference between X.Max and X.Min
        /// </summary>
        public double Width
        {
            get => X.Max - X.Min;
        }

        /// <summary>
        /// Gets the height of <see cref="DataRange"/>,
        /// i.e. the difference between Y.Max and Y.Min
        /// </summary>
        public double Height
        {
            get => Y.Max - Y.Min;
        }

        /// <summary>
        /// Gets a center point of a rectangle.
        /// </summary>
        public Point Center
        {
            get => new Point((X.Max + X.Min) / 2.0, (Y.Max + Y.Min) / 2.0);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get => X.IsEmpty && Y.IsEmpty;
        }

        /// <summary>
        /// Expands <see cref="X"/>  and <see cref="Y"/> by increasing their <see cref="Range.Max"/>
        /// or descreasing their <see cref="Range.Min"/> so that the ranges include the specified values.
        /// </summary>
        /// <param name="x">The value to include in <see cref="X"/>.</param>
        /// <param name="y">The value to include in <see cref="Y"/>.</param>
        public void Include(double x, double y)
        {
            X.Include(x);
            Y.Include(y);
        }

        /// <summary>
        /// Expands <see cref="X"/> and <see cref="Y"/> by increasing their <see cref="Range.Max"/>
        /// or descreasing their <see cref="Range.Min"/> so that the ranges include the minimum
        /// and maximum of the specified range.
        /// </summary>
        /// <param name="range">The range from which to get values to expand.</param>
        public void Include(DataRange range)
        {
            X.Include(range.X);
            Y.Include(range.Y);
        }

        /// <summary>
        /// Returns a string that represents the current instance of <see cref="DataRange"/>.
        /// </summary>
        /// <returns>String that represents the current instance of <see cref="DataRange"/></returns>
        public override string ToString()
        {
            return "{" + X.ToString() + " " + Y.ToString() + "}";
        }
    }
}