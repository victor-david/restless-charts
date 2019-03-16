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

        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //    return base.ArrangeOverride(finalSize);
        //}

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            double factor = e.Delta < 0 ? 1.2 : 1 / 1.2;

            if (xAxisRange == null) xAxisRange = owner.XAxis.Range;
            if (yAxisRange == null) yAxisRange = owner.YAxis.Range;

            ZoomToRange(owner.XAxis.Range.Zoom(factor), owner.YAxis.Range.Zoom(factor));
            //owner.XAxis.Range = owner.XAxis.Range.Zoom(factor);
            //owner.YAxis.Range = owner.YAxis.Range.Zoom(factor);
            
            //if (owner.Content is UIElement element)
            //{
            //    element.InvalidateMeasure();
            //}

            e.Handled = true;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                ZoomToRange(xAxisRange, yAxisRange);
                //if (xAxisRange != null && yAxisRange != null)
                //{
                //    owner.XAxis.Range = xAxisRange;
                //    owner.YAxis.Range = yAxisRange;
                //}
            }
        }

        private void ZoomToRange(Range xRange, Range yRange)
        {
            if (xRange != null && yRange != null)
            {
                owner.XAxis.Range = xRange;
                owner.YAxis.Range = yRange;

                if (owner.Content is UIElement element)
                {
                    element.InvalidateMeasure();
                }
            }

        }
    }
}
