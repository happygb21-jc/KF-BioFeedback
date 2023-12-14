using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Controls
{
    public class PlaceholderComboBox : ComboBox
    {
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(PlaceholderComboBox), new PropertyMetadata(string.Empty));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        static PlaceholderComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderComboBox), new FrameworkPropertyMetadata(typeof(PlaceholderComboBox)));
        }
    }
}