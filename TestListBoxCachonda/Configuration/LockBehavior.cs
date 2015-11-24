namespace TestListBoxCachonda.Configuration
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
                mouseButtonEventArgs.Handled = true;
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
    }
}