using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides static methods to assist in debug operations.
    /// </summary>
    public static class DebugHelper
    {

        /// <summary>
        /// Displays the line geometry for the specified group.
        /// </summary>
        /// <param name="text">Text to identify the caller.</param>
        /// <param name="group">The geometry group</param>
        public static void DisplayLineGeometry(string text, GeometryGroup group)
        {
#if DEBUG
            if (group != null)
            {
                Debug.WriteLine($"{text} has {group.Children.Count}");

                foreach (var item in group.Children.OfType<LineGeometry>())
                {
                    Debug.WriteLine($"Line. Start:{item.StartPoint} End:{item.EndPoint}");
                }
            }
#endif
        }
    }
}
