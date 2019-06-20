using System;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Helper class for mathematical calculations.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Gets a boolean value that indicates if <paramref name="value"/> is finite, i.e not NaN and not Infinity.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is not NaN and not Infinity; otherwise, false.</returns>
        public static bool IsFinite(this double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }

        /// <summary>
        /// Gets a boolean value that indicates if <paramref name="value"/> is finite and non-zero.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>true if the value is finite and non-zero; otherwise, false.</returns>
        public static bool IsFiniteNonZero(this double value)
        {
            return IsFinite(value) && value != 0;
        }

        /// <summary>
        /// Clamps the specified long value to be within the specified range inclusive.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns><paramref name="value"/> clamped between <paramref name="min"/> and <paramref name="max"/> inclusive.</returns>
        public static long Clamp(this long value, long min, long max)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        /// <summary>
        /// Clamps the specified double value to be within the specified range inclusive.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns><paramref name="value"/> clamped between <paramref name="min"/> and <paramref name="max"/> inclusive.</returns>
        public static double Clamp(this double value, double min, double max)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        /// <summary>
        /// Clamps the specified double value to be within zero and one inclusive.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <returns><paramref name="value"/> clamped between zero and one inclusive.</returns>
        public static double Clamp(double value)
        {
            return Math.Max(0, Math.Min(value, 1));
        }

        /// <summary>
        /// Clamps the specified integer value to be within the specified range inclusive.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns><paramref name="value"/> clamped between <paramref name="min"/> and <paramref name="max"/> inclusive.</returns>
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        /// <summary>
        /// Clamps the specified byte value to be within the specified range inclusive.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns><paramref name="value"/> clamped between <paramref name="min"/> and <paramref name="max"/> inclusive.</returns>
        public static byte Clamp(this byte value, byte min, byte max)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        /// <summary>
        /// Floor
        /// </summary>
        /// <param name="number">The number</param>
        /// <param name="rem">The rem</param>
        /// <returns>The value</returns>
        public static double Floor(double number, int rem)
        {
            if (rem <= 0)
            {
                rem = Clamp(-rem, 0, 15);
            }
            double pow = Math.Pow(10, rem - 1);
            double val = pow * Math.Floor(number / Math.Pow(10, rem - 1));
            return val;
        }

        /// <summary>
        /// Round
        /// </summary>
        /// <param name="value">The value to round.</param>
        /// <param name="digits">The number of digits</param>
        /// <returns>The value</returns>
        public static double Round(double value, int digits)
        {
            digits = Math.Abs(digits).Clamp(0, 8);

            if (digits == 0)
            {
                return Math.Round(value);
            }

            double pow = Math.Pow(10, digits - 1);
            value = pow * Math.Round(value / pow);
            return value;
        }

        /// <summary>
        /// Returns <see cref="Rect"/> by four given coordinates of corner points.
        /// </summary>
        /// <param name="minX">X coordinate on a left-top corner.</param>
        /// <param name="minY">Y coordinate on a left-top corner.</param>
        /// <param name="maxX">X coordinate on a right-bottom corner.</param>
        /// <param name="maxY">Y coordinate on a right-bottom corner.</param>
        /// <returns></returns>
        public static Rect CreateRectByPoints(double minX, double minY, double maxX, double maxY)
        {
            return new Rect(new Point(minX, minY), new Point(maxX, maxY));
        }

        /// <summary>
        /// Returns <see cref="Rect"/> by given point of a center and <see cref="Size"/>.
        /// </summary>
        /// <param name="center">Point of a center of a rectangle.</param>
        /// <param name="size">Size of a rectangle.</param>
        /// <returns>Rect.</returns>
        public static Rect CreateRectFromCenterSize(Point center, Size size)
        {
            return new Rect(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
        }

        /// <summary>
        /// Converts an angle in radians to an angle in degrees.
        /// </summary>
        /// <param name="angleInRadians">Angle in radians.</param>
        /// <returns>Angle in degrees.</returns>
        public static double ToDegrees(this double angleInRadians)
        {
            return angleInRadians * 180 / Math.PI;
        }

        /// <summary>
        /// Converts an angle in degrees to an angle in radians.
        /// </summary>
        /// <param name="angleInDegrees">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        public static double ToRadians(this double angleInDegrees)
        {
            return Math.PI * angleInDegrees / 180.0;
        }

        /// <summary>
        /// Gets a Point structure with vX and Y values reversed.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns><paramref name="point"/> with its X and Y values swapped.</returns>
        public static Point SwapXY(this Point point)
        {
            return new Point(point.Y, point.X);
        }
    }
}

