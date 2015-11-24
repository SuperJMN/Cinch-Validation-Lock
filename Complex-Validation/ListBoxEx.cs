namespace TestListBoxCachonda
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class ListBoxEx : ListBox
    {
        public ListBoxEx()
        {
            ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorOnItemsChanged;
            ItemContainerGenerator.StatusChanged += ItemContainerGeneratorOnStatusChanged;
        }

        private void ItemContainerGeneratorOnStatusChanged(object sender, EventArgs eventArgs)
        {
            var status = ItemContainerGenerator.Status;
            if (status == GeneratorStatus.ContainersGenerated)
            {
                foreach (var item in ItemContainerGenerator.Items)
                {                    
                    var container = (UIElement) ItemContainerGenerator.ContainerFromItem(item);
                    container.PreviewMouseDown += ContainerOnPreviewMouseDown;
                    container.PreviewKeyDown += ContainerPreviewKeyDown;
                }
            }
        }

        private void ContainerPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = ShouldHandleEvent(sender);
        }

        private bool ShouldHandleEvent(object sender)
        {
            if (!CanChangeSelection)
            {
                return true;
            }

            if (PromptCommand != null && PromptCommand.CanExecute(null))
            {                
                PromptCommand.Execute(null);
                if (CanChangeSelection)
                {
                    SelectedItem = ItemContainerGenerator.ItemFromContainer((DependencyObject) sender);
                }
                return true;
            }

            return false;
        }

        private void ContainerOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = ShouldHandleEvent(sender);
        }

        private void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs itemsChangedEventArgs)
        {
            if (itemsChangedEventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var item in ItemContainerGenerator.Items)
                {
                    var container = (UIElement)ItemContainerGenerator.ContainerFromItem(item);
                    if (container!=null)
                    {
                        container.PreviewMouseDown -= ContainerOnPreviewMouseDown;
                    }
                }
            }            
        }

        public static readonly DependencyProperty PromptCommandProperty = DependencyProperty.Register(
            "PromptCommand",
            typeof (ICommand),
            typeof (ListBoxEx),
            new PropertyMetadata(default(ICommand)));

        public ICommand PromptCommand
        {
            get { return (ICommand) GetValue(PromptCommandProperty); }
            set { SetValue(PromptCommandProperty, value); }
        }
        
        public static readonly DependencyProperty CanChangeSelectionProperty = DependencyProperty.Register(
            "CanChangeSelection",
            typeof (bool),
            typeof (ListBoxEx),
            new PropertyMetadata(true));

        public bool CanChangeSelection
        {
            get { return (bool) GetValue(CanChangeSelectionProperty); }
            set { SetValue(CanChangeSelectionProperty, value); }
        }
    }
}