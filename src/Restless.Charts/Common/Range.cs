using System;
using System.Globalization;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents ranges for double type.
    /// </summary>
    public class Range
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of range from given minimum and maximum values.
        /// </summary>
        /// <param name="minimum">Minimum value of the range.</param>
        /// <param name="maximum">Maximum value of the range.</param>
        public Range(double minimum, double maximum)
        {
            if (!minimum.IsFinite() || !maximum.IsFinite())
            {
                throw new ArgumentException($"{nameof(minimum)} and {nameof(maximum)} must be finite");
            }

            if (minimum < maximum)
            {
                Min = minimum;
                Max = maximum;
            }
            else
            {
                Min = maximum;
                Max = minimum;
            }
        }

        private Range()
        {

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
        public static Range FullReversedRange()
        {
            return new Range()
            {
                Min = double.MaxValue,
                Max = double.MinValue
            };
        }

        /// <summary>
        /// Creates and returns an empty range
        /// </summary>
        public static Range EmptyRange()
        {
            return new Range()
            {
                Min = double.PositiveInfinity,
                Max = double.NegativeInfinity
            };
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the minimum value of current range.
        /// </summary>
        public double Min
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the maximum value of current range.
        /// </summary>
        public double Max
        {
            get;
            set;
        }


        /// <summary>
        /// Returns true of this range is considered empty. An empty range is when
        /// Min == PositiveInfinity and Max == NegativeInfinity
        /// </summary>
        public bool IsEmpty
        {
            get => double.IsPositiveInfinity(Min) && double.IsNegativeInfinity(Max);
        }

        /// <summary>
        /// Returns true of this range is a point (e.g. Min == Max).
        /// </summary>
        public bool IsPoint
        {
            get => Max == Min;
        }
        #endregion

        /************************************************************************/

        #region Methods
        /// <summary>
        /// Updates current instance of <see cref="Range"/> with minimal range which contains current range and specified value
        /// </summary>
        /// <param name="value">Value, which will be used for for current instance of range surrond</param>
        public void Surround(double value)
        {
            if (value.IsFinite())
            {
                if (value < Min) Min = value;
                if (value > Max) Max = value;
            }
        }

        /// <summary>
        /// Updates current instance of <see cref="Range"/> with minimal range which contains current range and specified range
        /// </summary>
        /// <param name="range">Range, which will be used for current instance of range surrond</param>
        public void Surround(Range range)
        {
            if (range != null && !range.IsEmpty)
            {
                Surround(range.Min);
                Surround(range.Max);
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
        /// Calculates and returns a new range object which will have its Min and Max
        /// 
        /// 
        /// 
        /// the same center and which size will be larger in factor times
        /// </summary>
        /// <param name="factor">Zoom factor</param>
        /// <returns>Zoomed with specified factor range</returns>
        public Range Zoom(double factor)
        {
            if (!factor.IsFinite()) throw new ArgumentException($"{nameof(factor)} must be finite");
            if (IsEmpty) return EmptyRange();

            double delta = (Max - Min) / 2;
            double center = (Max + Min) / 2;

            Range zoomed = new Range(center - delta * factor, center + delta * factor);

            //if (zoomed.Min < 0) return zoomed.ZeroCentered();

            return zoomed;
        }

        /// <summary>
        /// Returns a <see cref="Range"/> from current Min and Max that is centered on zero.
        /// </summary>
        /// <returns>A new range object.</returns>
        public Range ZeroCentered()
        {
            double max = Math.Max(Math.Abs(Min), Math.Abs(Max));
            return new Range(-max, max);
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
        /// <param name="second">The second value to compare.</param>
        /// <returns>True if values are not equal, false otherwise.</returns>
        public static bool operator != (Range leftSide, Range rightSide)
        {
            return !(leftSide == rightSide);
        }
        #endregion
    }
}


