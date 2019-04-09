using System;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents calculation values for major tick generation.
    /// </summary>
    public class TickCalculation
    {
        #region Private
        private Range range;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="TickCalculation"/> class.
        /// </summary>
        /// <param name="range">The range.</param>
        public TickCalculation(Range range)
        {
            this.range = range ?? throw new ArgumentNullException(nameof(range));
            Delta = 1;
            Beta = 0;
            Calculate();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TickCalculation"/> class,
        /// ensuring that <see cref="TickCount"/> is less than or equal to
        /// the specified value.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="maxTickCount">The maximum tick count.</param>
        /// <remarks>
        /// This constructor performs startup calculations and then calls
        /// <see cref="DecreaseTickCount(long)"/> to get <see cref="TickCount"/>
        /// to be less than or equal to <paramref name="maxTickCount"/>.
        /// </remarks>
        public TickCalculation(Range range, long maxTickCount) : this (range)
        {
            DecreaseTickCount(maxTickCount);
        }
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the delta value.
        /// </summary>
        public int Delta
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the beta value.
        /// </summary>
        public int Beta
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the multiplier
        /// </summary>
        public double Multiplier
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the tick count.
        /// </summary>
        public long TickCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets X0.
        /// </summary>
        public double X0
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Decreases the tick count and recalculates all values
        /// until <see cref="TickCount"/> is less than or equal to <paramref name="maxTickCount"/>.
        /// </summary>
        /// <param name="maxTickCount">
        /// The maximum tick count. This value is clamped between <see cref="Axis.MinMaxTickCount"/>
        /// and <see cref="Axis.MaxMaxTickCount"/> inclusive.
        /// </param>
        public void DecreaseTickCount(long maxTickCount)
        {
            maxTickCount = maxTickCount.Clamp(Axis.MinMaxTickCount, Axis.MaxMaxTickCount);
            while (TickCount > maxTickCount)
            {
                DecreaseTickCount();
            }
        }

        /// <summary>
        /// Increases the tick count and recalculates all values.
        /// </summary>
        public void IncreaseTickCount()
        {
            switch (Delta)
            {
                case 1:
                    Delta = 5;
                    Beta--;
                    break;
                case 2:
                    Delta = 1;
                    break;
                case 5:
                    Delta = 2;
                    break;
            }
            Calculate();
        }

        /// <summary>
        /// Decreases the tick count and recalculates all values.
        /// </summary>
        public void DecreaseTickCount()
        {
            switch (Delta)
            {
                case 1:
                    Delta = 2;
                    break;
                case 2:
                    Delta = 5;
                    break;
                case 5:
                    Delta = 1;
                    Beta++;
                    break;
            }
            Calculate();
        }
        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A string that displays the values of this object.</returns>
        public override string ToString()
        {
            return $"Range: {range} Delta: {Delta} Beta: {Beta} Multiplier: {Multiplier} X0: {X0} TickCount: {TickCount}";
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void Calculate()
        {
            Multiplier = Delta * Math.Pow(10, Beta);
            double min = Math.Floor(range.Min / Multiplier);
            double max = Math.Floor(range.Max / Multiplier);
            TickCount = (long)(max - min + 1);
            X0 = min * Multiplier;
        }
        #endregion
    }
}
