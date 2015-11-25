namespace ComplexValidation.Configuration.View.Supporters
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    public class LockBehavior : Behavior<UIElement>
    {
        private Control element;
        private Window window;

        protected override void OnAttached()
        {          
            base.OnAttached();

            element = (Control) AssociatedObject;
            if (element.Background == null)
            {
                element.Background = Brushes.Transparent;
            }

            element.Loaded += ElementOnLoaded;
        }

        private void ElementOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            window = element.TryFindParent<Window>();            
            window.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (IsLocked && !element.IsMouseOver)
            {
                TryExecuteScapeAttemptCommand();
                mouseButtonEventArgs.Handled = true;
            }
        }

        private void TryExecuteScapeAttemptCommand()
        {
            if (ScapeAttemptCommand != null && ScapeAttemptCommand.CanExecute(ScapeAttemptCommandParameter))
            {
                ScapeAttemptCommand.Execute(ScapeAttemptCommandParameter);
            }
        }

        public static readonly DependencyProperty IsLockedProperty = DependencyProperty.Register(
            "IsLocked",
            typeof (bool),
            typeof (LockBehavior),
            new PropertyMetadata(default(bool)));

        public bool IsLocked
        {
            get { return (bool) GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        public static readonly DependencyProperty ScapeAttemptCommandProperty = DependencyProperty.Register(
            "ScapeAttemptCommand",
            typeof (ICommand),
            typeof (LockBehavior),
            new PropertyMetadata(default(ICommand)));

        public ICommand ScapeAttemptCommand
        {
            get { return (ICommand) GetValue(ScapeAttemptCommandProperty); }
            set { SetValue(ScapeAttemptCommandProperty, value); }
        }

        public static readonly DependencyProperty ScapeAttemptCommandParameterProperty = DependencyProperty.Register(
            "ScapeAttemptCommandParameter",
            typeof (object),
            typeof (LockBehavior),
            new PropertyMetadata(default(object)));

        public object ScapeAttemptCommandParameter
        {
            get { return (object) GetValue(ScapeAttemptCommandParameterProperty); }
            set { SetValue(ScapeAttemptCommandParameterProperty, value); }
        }
    }
}