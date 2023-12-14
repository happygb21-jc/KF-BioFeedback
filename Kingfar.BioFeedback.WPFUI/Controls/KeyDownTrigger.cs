using Microsoft.Xaml.Behaviors;
using System.Windows.Input;

namespace Kingfar.BioFeedback.WPFUI.Controls
{
    public class KeyDownTrigger : TriggerAction<UIElement>
    {
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(Key), typeof(KeyDownTrigger), new PropertyMetadata(Key.None));

        public Key Key
        {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
           DependencyProperty.Register("Command", typeof(ICommand), typeof(KeyDownTrigger), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
           DependencyProperty.Register("CommandParameter", typeof(object), typeof(KeyDownTrigger), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            var e = parameter as KeyEventArgs;
            if (e != null && e.Key == Key)
            {
                if (Command != null && Command.CanExecute(CommandParameter))
                {
                    Command.Execute(parameter);
                }
            }
        }
    }
}