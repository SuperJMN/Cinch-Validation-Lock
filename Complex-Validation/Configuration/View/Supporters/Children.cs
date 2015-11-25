namespace ComplexValidation.Configuration.View.Supporters
{
    using System.Windows;
    using System.Windows.Controls;

    public class Children
    {
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached( "Margin", typeof(Thickness), typeof(Children), new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }
        
        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }


        public static void MarginChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Make sure this is put on a panel


            var panel = sender as Panel;


            if (panel == null)
            {
                return;
            }


            panel.Loaded += panel_Loaded;
        }


        private static void panel_Loaded(object sender, RoutedEventArgs e)
        {
            var panel = sender as Panel;

            // Go over the children and set margin for them:
            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;
                if (fe == null)
                {
                    continue;
                }
                if (fe.ReadLocalValue(MarginProperty) == DependencyProperty.UnsetValue)
                {
                    fe.Margin = GetMargin(panel);
                }
                else
                {
                    
                }
            }
        }
    }
}