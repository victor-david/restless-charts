using System;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides methods for brush generation
    /// </summary>
    public static class BrushUtility
    {
        #region Private
        private static readonly Random random = new Random();
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Creates a frozen <see cref="SolidColorBrush"/> with a random color.
        /// </summary>
        /// <returns>A solid color brush.</returns>
        public static Brush GetRandomSolidBrush()
        {
            Brush brush = new SolidColorBrush(MakeRandomColor());
            brush.Freeze();
            return brush;
        }

        /// <summary>
        /// Creates a frozen <see cref="LinearGradientBrush"/> with two colors,
        /// using a difference of 50.
        /// </summary>
        /// <returns>A linear gradient brush</returns>
        public static Brush GetRandomLinearBrush()
        {
            return GetRandomLinearBrush(50);
        }

        /// <summary>
        /// Creates a frozen <see cref="LinearGradientBrush"/> with two colors,
        /// using the specified difference.
        /// </summary>
        /// <param name="difference">
        /// The difference factor applied to generate the second color.
        /// This value is clamped between 10 and 100.
        /// </param>
        /// <returns>A linear gradient brush</returns>
        public static Brush GetRandomLinearBrush(int difference)
        {
            Color color1 = MakeRandomColor();
            Color color2 = MakeDifferenceColor(color1, difference);
            Brush brush = new LinearGradientBrush(color1, color2, 45);
            brush.Freeze();
            return brush;
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private static Color MakeRandomColor()
        {
            byte r = (byte)random.Next(0, 256);
            byte g = (byte)random.Next(0, 256);
            byte b = (byte)random.Next(0, 256);
            return Color.FromRgb(r, g, b);
        }

        private static Color MakeDifferenceColor(Color color, int difference)
        {
            difference = difference.Clamp(10, 100);
            byte r = GetNewColorByte(color.R, difference);
            byte g = GetNewColorByte(color.G, difference);
            byte b = GetNewColorByte(color.B, difference);
            return Color.FromRgb(r, g, b);
        }

        private static byte GetNewColorByte(byte value, int difference)
        {
            if (value + difference > 255)
            {
                return (byte)(value - difference);
            }
            else
            {
                return (byte)(value + difference);
            }
        }
        #endregion
    }
}
