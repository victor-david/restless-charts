using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a control to display help information
    /// </summary>
    public class HelpControl : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpControl"/> class.
        /// </summary>
        public HelpControl()
        {
            // width will animate out.
            Width = 0.0;
            CommandBindings.Add(new CommandBinding(CloseHelpCommand, ExecuteNavigationHelpCommand));
        }

        static HelpControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HelpControl), new FrameworkPropertyMetadata(typeof(HelpControl)));
        }
        #endregion

        /************************************************************************/

        #region IsHelpVisible
        /// <summary>
        /// Gets or sets a value that determines if the control is visible.
        /// </summary>
        public bool IsHelpVisible
        {
            get => (bool)GetValue(IsHelpVisibleProperty);
            set => SetValue(IsHelpVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsHelpVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsHelpVisibleProperty = DependencyProperty.Register
            (
                nameof(IsHelpVisible), typeof(bool), typeof(HelpControl), 
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsHelpVisibleChanged)
            );

        private static void OnIsHelpVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HelpControl c)
            {
                c.AnimateVisibility();
            }
        }
        #endregion

        /************************************************************************/

        #region Header
        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register
            (
                nameof(Header), typeof(string), typeof(HelpControl), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region HelpContent
        /// <summary>
        /// Gets or sets the help content.
        /// </summary>
        public object HelpContent
        {
            get => GetValue(HelpContentProperty);
            set => SetValue(HelpContentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HelpContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HelpContentProperty = DependencyProperty.Register
            (
                nameof(HelpContent), typeof(object), typeof(HelpControl), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region CloseHelpCommand
        /// <summary>
        /// Gets the command used to display navigation help.
        /// </summary>
        public static readonly RoutedCommand CloseHelpCommand = new RoutedCommand();

        private void ExecuteNavigationHelpCommand(object sender, ExecutedRoutedEventArgs e)
        {
            IsHelpVisible = false;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void AnimateVisibility()
        {
            double from = IsHelpVisible ? 0.0 : 380.0;
            double to = IsHelpVisible ? 380.0 : 0.0;
            DoubleAnimation wa = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(200));
            BeginAnimation(WidthProperty, wa);
        }
        #endregion

    }
}
