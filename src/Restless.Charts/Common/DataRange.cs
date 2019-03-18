using System;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a <see cref="Range"/> of data values for <see cref="X"/>
    /// and a <see cref="Range"/> of data values for <see cref="Y"/>.
    /// </summary>
    public class DataRange
    {
        /// <summary>
        /// Gets an empty <see cref="DataRange"/>.
        /// </summary>
        /// <returns>A data range with an empty X range and an empty Y range.</returns>
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
            X = Range.SpecifiedRange(minX, maxX);
            Y = Range.SpecifiedRange(minY, maxY);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRange"/> struct.
        /// </summary>
        /// <param name="x">Horizontal range</param>
        /// <param name="y">Vertical range</param>
        /// <exception cref="ArgumentNullException">Either parameter is null.</exception>
        public DataRange(Range x, Range y)
        {
            X = x ?? throw new ArgumentNullException(nameof(x));
            Y = y ?? throw new ArgumentNullException(nameof(y));
        }

        /// <summary>
        /// Gets the X range.
        /// </summary>
        public Range X
        {
            get;
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
            return $"{X} {Y}";
        }
    }
}