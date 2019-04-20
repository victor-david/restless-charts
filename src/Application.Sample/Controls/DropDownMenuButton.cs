using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Application.Sample
{
    public class DropDownMenuButton : ToggleButton
    {
        public DropDownMenuButton()
        {
            ContentTemplate = GetDataTemplate();
        }

        public ContextMenu Menu
        {
            get => (ContextMenu)GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register
            (
                nameof(Menu), typeof(ContextMenu), typeof(DropDownMenuButton), new PropertyMetadata(null, OnMenuChanged)
            );

        private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropDownMenuButton c && e.NewValue is ContextMenu menu)
            {
                menu.Closed += (s, e2) => c.IsChecked = false;
            }
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (Menu != null)
            {
                Menu.PlacementTarget = e.Source as ToggleButton;
                Menu.Placement = PlacementMode.Bottom;
                Menu.IsOpen = true;
            }
        }

        private DataTemplate GetDataTemplate()
        {
            var template = new DataTemplate(typeof(DropDownMenuButton));
            var path = new FrameworkElementFactory(typeof(Path));
            path.SetValue(Path.DataProperty, Geometry.Parse("F1 M 301.14,-189.041L 311.57,-189.041L 306.355,-182.942L 301.14,-189.041 Z"));
            path.SetValue(MarginProperty, new Thickness(4, 4, 0, 4));
            path.SetValue(WidthProperty, 5d);
            path.SetValue(Shape.FillProperty, Brushes.Black);
            path.SetValue(Shape.StretchProperty, Stretch.Uniform);
            path.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Right);
            path.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);

            var panel = new FrameworkElementFactory(typeof(StackPanel));
            panel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));

            var binding = new Binding("Content") { Source = this };
            contentPresenter.SetBinding(ContentProperty, binding);

            panel.AppendChild(contentPresenter);
            panel.AppendChild(path);

            template.VisualTree = panel;
            return template;
        }
    }
}