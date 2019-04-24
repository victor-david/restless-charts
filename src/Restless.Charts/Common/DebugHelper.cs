using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides static methods to assist in debug operations.
    /// </summary>
    public static class DebugHelper
    {

        /// <summary>
        /// Displays information about a routed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The routed event args.</param>
        public static void DisplayRoutedEvent(object sender, RoutedEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("Routed Event Handler");
            Debug.WriteLine("====================");
            Debug.WriteLine($"Event: {e.RoutedEvent.OwnerType}.{e.RoutedEvent.Name} ({e.RoutedEvent.RoutingStrategy})");
            Debug.WriteLine($"Sender: {sender}");
            Debug.WriteLine($"Source: {e.Source}");
            Debug.WriteLine($"Orig Source: {e.OriginalSource}");
#endif
        }


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
