using System.Windows;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides static methods to assist with visual tree traversal.
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        /// Searches for and sets the specified object in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of object to search for and set.</typeparam>
        /// <param name="origin">The element from which to originate the search.</param>
        /// <param name="parent">The value set to set.</param>
        /// <returns>true if <paramref name="parent"/> was located and set; otherwise, false.</returns>
        public static bool TrySetParent<T>(DependencyObject origin, ref T parent) where T:class
        {
            if (origin == null) return false;

            DependencyObject result = VisualTreeHelper.GetParent(origin);

            while (result != null && !(result is T))
            {
                result = VisualTreeHelper.GetParent(result);
            }
            parent = result as T;
            return parent is T;
        }
    }
}
