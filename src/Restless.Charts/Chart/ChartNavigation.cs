using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides a panel that enables chart navigation.
    /// </summary>
    /// <remarks>
    /// The following navigation is supported:
    /// Mouse wheel - zoom in / zoom out
    /// Left mouse double click - restore original zoom
    /// Left mouse single click with Control key pressed - zoom out
    /// Left mouse single click with Shift key pressed - zoom in
    /// Right mouse single click - zoom in
    /// Mouse move - pan X and Y
    /// Mouse move with Control key pressed - pan Y only.
    /// Mouse move with Shift key pressed - pan X only.
    /// </remarks>
    public class ChartNavigation : Panel
    {
        #region Private
        private readonly ChartContainer owner;
        private Point mouseStartPoint;
        private bool isKeyboardNavigationEnabled;
        private bool isPanning;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default value for <see cref="IsKeyboardNavigationEnabled"/>.
        /// </summary>
        public const bool IsKeyboardNavigationEnabledDefault = true;
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
            FocusVisualStyle = null;
            IsKeyboardNavigationEnabled = IsKeyboardNavigationEnabledDefault;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets a value that determines if keyboard navigation is enabled.
        /// </summary>
        public bool IsKeyboardNavigationEnabled
        {
            get => isKeyboardNavigationEnabled;
            set
            {
                isKeyboardNavigationEnabled = value;
                Focusable = isKeyboardNavigationEnabled;
            }
        }
        #endregion

        /************************************************************************/

        #region Mouse methods
        /// <summary>
        /// Called when the mouse enters.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Keyboard.Focus(this);
            e.Handled = true;
        }

        /// <summary>
        /// Called when the mouse leaves.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            // TODO - this doesn't cause the keyboard focus to move to another element.
            Keyboard.Focus(null);
        }

        /// <summary>
        /// Called when the mouse wheel moves.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
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
        /// Called when the left mouse button goes down.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.ClickCount == 2 && Keyboard.Modifiers == ModifierKeys.None)
            {
                owner.RestoreSizeAndPosition();
            }
            mouseStartPoint = e.GetPosition(this);
            e.Handled = true;
        }

        /// <summary>
        /// Called when the left mouse button goes up.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            // If panning with a keyboard modifier, don't want an errant zoom at the end.
            if (!isPanning)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    owner.ZoomOut();
                }
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    owner.ZoomIn();
                }
            }
            isPanning = false;
            e.Handled = true;
        }

        /// <summary>
        /// Called when the mouse moves.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isPanning = true;
                Point movePoint = e.GetPosition(this);

                double xPercentage = (mouseStartPoint.X - movePoint.X) / ActualWidth;
                double yPercentage = (mouseStartPoint.Y - movePoint.Y) / ActualHeight;

                // Hold Control key to move only Y. Hold Shift to move only X
                xPercentage = Keyboard.Modifiers == ModifierKeys.Control ? 0 : xPercentage;
                yPercentage = Keyboard.Modifiers == ModifierKeys.Shift ? 0 : yPercentage;

                mouseStartPoint = movePoint;
                owner.Pan(xPercentage, yPercentage);
            }
            e.Handled = true;
        }
        #endregion

        /************************************************************************/

        #region Keyboard methods
        /// <summary>
        /// Called when a key goes down.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Key.Right:
                    owner.Pan(-0.05, 0);
                    break;
                case Key.Left:
                    owner.Pan(0.05, 0);
                    break;
                case Key.Up:
                    owner.Pan(0, 0.05);
                    break;
                case Key.Down:
                    owner.Pan(0, -0.05);
                    break;
            }

            Keyboard.Focus(this);
            e.Handled = true;
        }
        #endregion
    }
}
