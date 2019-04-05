using System;
using System.Globalization;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a range of double values.
    /// </summary>
    public class Range
    {
        #region Private
        private double snapshotMin;
        private double snapshotMax;
        private bool isSnapshotCreated;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Creates and returns a range using the specified minimum and maximum.
        /// </summary>
        /// <param name="min">The minimum value in the range.</param>
        /// <param name="max">The maximum value in the range.</param>
        /// <returns>The range object.</returns>
        /// <exception cref="ArgumentException">Either <paramref name="min"/> or <paramref name="max"/> is not finite.</exception>
        public static Range SpecifiedRange(double min, double max)
        {
            if (!min.IsFinite() || !max.IsFinite())
            {
                throw new ArgumentException($"{nameof(min)} and {nameof(max)} must be finite");
            }
            double createMin = (min < max) ? min : max;
            double createMax = (min < max) ? max : min;
            return new Range(createMin, createMax);
        }

        /// <summary>
        /// Creates and returns a range that represents a single point, i.e. min is the same as max.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The range object.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not finite.</exception>
        public static Range PointRange(double value)
        {
            return SpecifiedRange(value, value);
        }

        /// <summary>
        /// Creates and returns a range from double.MinValue to double.MaxValue
        /// </summary>
        /// <returns>A new range object</returns>
        public static Range FullRange()
        {
            return new Range(double.MinValue, double.MaxValue);
        }

        /// <summary>
        /// Creates and returns a range with <see cref="Min"/> set to double.MaxValue and <see cref="Max"/> set to double.MinValue.
        /// This can be used as a starting range to then surround with actual data values.
        /// </summary>
        /// <returns>A new range object</returns>
        public static Range FullReversedRange()
        {
            return new Range(double.MaxValue, double.MinValue);
        }

        /// <summary>
        /// Creates and returns an empty range.
        /// </summary>
        /// <returns>A new range object</returns>
        public static Range EmptyRange()
        {
            return new Range(double.PositiveInfinity, double.NegativeInfinity);
        }

        private Range(double min, double max)
        {
            Min = min;
            Max = max;
            snapshotMin = 0;
            snapshotMin = 0;
            isSnapshotCreated = false;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the minimum value of the range.
        /// </summary>
        public double Min
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the maximum value of the range.
        /// </summary>
        public double Max
        {
            get;
            private set;
        }

        public double MidPoint
        {
            get => Min + (Max - Min) / 2.0;
        }

        /// <summary>
        /// Returns true of this range is considered empty. An range is empty when
        /// Min == PositiveInfinity and Max == NegativeInfinity.
        /// </summary>
        public bool IsEmpty
        {
            get => double.IsPositiveInfinity(Min) && double.IsNegativeInfinity(Max);
        }

        /// <summary>
        /// Returns true if this range contains finite values for both <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        public bool HasValues
        {
            get => Min.IsFinite() && Max.IsFinite();
        }

        /// <summary>
        /// Returns true of this range is a point, that is: Min == Max.
        /// </summary>
        public bool IsPoint
        {
            get => Max == Min;
        }
        #endregion

        /************************************************************************/

        #region Methods
        /// <summary>
        /// Changes the range into an empty range.
        /// </summary>
        public void MakeEmpty()
        {
            Min = double.PositiveInfinity;
            Max = double.NegativeInfinity;
        }

        /// <summary>
        /// Gets a boolean value that indicates if <paramref name="value"/> is within <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        /// true if <paramref name="value"/> is greater than or equal to <see cref="Min"/> and less than or equal to <see cref="Max"/>;
        /// otherwise, false.
        /// </returns>
        public bool Includes(double value)
        {
            return !double.IsNaN(value) && value >= Min && value <= Max;
        }

        /// <summary>
        /// Expands this instance by increasing <see cref="Max"/> or descreasing <see cref="Min"/>
        /// so that the range includes the specified value.
        /// </summary>
        /// <param name="value">The value to include in range.</param>
        public void Include(double value)
        {
            if (value.IsFinite())
            {
                if (value < Min) Min = value;
                if (value > Max) Max = value;
            }
        }

        /// <summary>
        /// Expands this instance by increasing <see cref="Max"/> or descreasing <see cref="Min"/>
        /// so that the range includes the minimum and maximum of the specified range.
        /// </summary>
        /// <param name="range">The range from which to get values to expand.</param>
        public void Include(Range range)
        {
            if (range != null && !range.IsEmpty)
            {
                Include(range.Min);
                Include(range.Max);
            }
        }

        /// <summary>
        /// Decreases <see cref="Min"/> by the specified percentage.
        /// </summary>
        /// <param name="percentage">The percentage. Min will be decreased by this perecentage.</param>
        public void DecreaseMinBy(double percentage)
        {
            Include(Min - Math.Abs(Min * Math.Abs(percentage)));
        }

        /// <summary>
        /// Increases <see cref="Max"/> by the specified percentage.
        /// </summary>
        /// <param name="percentage">The percentage. Max will be increased by this perecentage.</param>
        public void IncreaseMaxBy(double percentage)
        {
            Include(Max + Max * Math.Abs(percentage));
        }

        /// <summary>
        /// Shifts <see cref="Min"/> and <see cref="Max"/> by the specified percentage.
        /// </summary>
        /// <param name="percentage">The percentage</param>
        /// <exception cref="ArgumentException"><paramref name="percentage"/> is not finite.</exception>
        public void Shift(double percentage)
        {
            if (!percentage.IsFinite()) throw new ArgumentException($"{nameof(percentage)} must be finite");
            if (IsEmpty) return;

            // double shift = (Max - Min) / 2 * percentage;
            double shift = (Max - Min) * percentage;


            Min += shift;
            Max += shift;
        }

        /// <summary>
        /// Changes this range so that it is zoomed or contracted by the specified percentage.
        /// </summary>
        /// <param name="percentage">The zoom percentage.</param>
        /// <exception cref="ArgumentException"><paramref name="percentage"/> is not finite.</exception>
        public void MakeZoomed(double percentage)
        {
            if (!percentage.IsFinite()) throw new ArgumentException($"{nameof(percentage)} must be finite");
            if (IsEmpty) return;

            double delta = (Max - Min) / 2;
            double center = (Max + Min) / 2;

            Min = center - delta * percentage; 
            Max = center + delta * percentage;
        }

        /// <summary>
        /// Modifies this range so that it is centered on zero.
        /// </summary>
        public void MakeZeroCentered()
        {
            double max = Math.Max(Math.Abs(Min), Math.Abs(Max));
            Include(-max);
            Include(max);
        }

        /// <summary>
        /// Creates a snapshot of the values in this range. 
        /// Restore with <see cref="RestoreFromSnapshot"/>.
        /// </summary>
        public void CreateSnapshot()
        {
            if (!isSnapshotCreated)
            {
                snapshotMin = Min;
                snapshotMax = Max;
                isSnapshotCreated = true;
            }
        }

        /// <summary>
        /// Restores the values in this range which were saved with <see cref="CreateSnapshot"/>.
        /// </summary>
        public void RestoreFromSnapshot()
        {
            if (isSnapshotCreated)
            {
                Min = snapshotMin;
                Max = snapshotMax;
            }
        }

        /// <summary>
        /// Returns a string that represents the current range.
        /// </summary>
        /// <returns>String that represents the current range</returns>
        public override string ToString()
        {
            return "[" + Min.ToString(CultureInfo.InvariantCulture) + "," + Max.ToString(CultureInfo.InvariantCulture) + "]";
        }

        /// <summary>
        /// Determines whether the specified <see cref="Range"/> is equal to the current range.
        /// </summary>
        /// <param name="obj">The range to compare with the current <see cref="Range"/>.</param>
        /// <returns>True if the specified range is equal to the current range, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            Range r = (Range)obj;
            return r.Min == Min && r.Max == Max;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for current instance</returns>
        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }

        /// <summary>
        /// Returns a value that indicates whether two specified range values are equal.
        /// </summary>
        /// <param name="leftSide">The first value to compare.</param>
        /// <param name="rightSide">The second value to compare.</param>
        /// <returns>True if values are equal, false otherwise.</returns>
        public static bool operator == (Range leftSide, Range rightSide)
        {
            // Check for null on left side.
            if (leftSide is null)
            {
                if (rightSide is null)
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return leftSide.Equals(rightSide);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified ranges values are not equal.
        /// </summary>
        /// <param name="leftSide">The first value to compare.</param>
        /// <param name="rightSide">The second value to compare.</param>
        /// <returns>True if values are not equal, false otherwise.</returns>
        public static bool operator != (Range leftSide, Range rightSide)
        {
            return !(leftSide == rightSide);
        }
        #endregion
    }
}


