﻿namespace ComplexValidation.Configuration.View.Supporters
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TestListBoxCachonda"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TestListBoxCachonda;assembly=TestListBoxCachonda"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:TupleLayout/>
    ///
    /// </summary>
    public class TupleLayout : Control
    {
        static TupleLayout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TupleLayout), new FrameworkPropertyMetadata(typeof(TupleLayout)));
        }

        public static readonly DependencyProperty LeftSideProperty = DependencyProperty.Register(
            "LeftSide",
            typeof (object),
            typeof (TupleLayout),
            new PropertyMetadata(default(object)));

        public object LeftSide
        {
            get { return (object) GetValue(LeftSideProperty); }
            set { SetValue(LeftSideProperty, value); }
        }

        public static readonly DependencyProperty RightSideProperty = DependencyProperty.Register(
            "RightSide",
            typeof (object),
            typeof (TupleLayout),
            new PropertyMetadata(default(object)));

        public object RightSide
        {
            get { return (object) GetValue(RightSideProperty); }
            set { SetValue(RightSideProperty, value); }
        }
    }
}
