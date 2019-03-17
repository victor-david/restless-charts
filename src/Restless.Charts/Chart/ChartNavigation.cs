using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides a panel that enables chart navigation.
    /// </summary>
    public class ChartNavigation : Panel
    {
        #region Private
        private readonly ChartContainer owner;
        private Range xAxisRange;
        private Range yAxisRange;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes new instance of the <see cref="ChartNavigation"/> class.
        /// </summary>
        /// <param name="owner">The chart that owns this navigation panel</param>
        internal ChartNavigation(ChartContainer owner)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            // need a background to intercept mouse wheel and clicks.
            Background = Brushes.Transparent;
        }
        #endregion

        /// <summary>
        /// Called when the mouse wheel moves.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (xAxisRange == null) xAxisRange = owner.XAxis.Range;
            if (yAxisRange == null) yAxisRange = owner.YAxis.Range;

            if (e.Delta < 0)
            {
                owner.ZoomOut();
            }
            else
            {
                owner.ZoomIn();
            }
            e.Handled = true;
        }

        /// <summary>
        /// Called when a mouse button goes down.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                owner.ZoomToRange(xAxisRange, yAxisRange);
            }
        }
    }
}
