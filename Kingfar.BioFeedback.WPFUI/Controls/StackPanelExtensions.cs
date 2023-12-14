using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Controls
{
    public class StackPanelExtensions
    {
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.RegisterAttached(
            "ItemMargin",
            typeof(Thickness),
            typeof(StackPanelExtensions),
            new PropertyMetadata(new Thickness(), OnItemMarginChanged));

        public static Thickness GetItemMargin(UIElement element)
        {
            return (Thickness)element.GetValue(ItemMarginProperty);
        }

        public static void SetItemMargin(UIElement element, Thickness value)
        {
            element.SetValue(ItemMarginProperty, value);
        }

        private static void OnItemMarginChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;
            panel.Loaded += (ss, ee) =>
            {
                foreach (var child in panel.Children.Cast<UIElement>())
                {
                    child.SetValue(FrameworkElement.MarginProperty, e.NewValue);
                }
            };
        }
    }
}