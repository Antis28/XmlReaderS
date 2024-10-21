using System.Windows.Controls;
using System.Windows;

/*
 *
 *и добавьте это в свой xaml:

   <local:ExtendedTreeView ItemsSource="{Binding Items}" SelectedItem_="{Binding Item, Mode=TwoWay}">
   .....
   </local:ExtendedTreeView>
 *
 *
 */

namespace WpfParser.Views.Extended
{
    public class ExtendedTreeView : TreeView
    {
        public ExtendedTreeView()
            : base()
        {
            this.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(___ICH);
        }

        void ___ICH(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedItem != null)
            {
                SetValue(SelectedItem_Property, SelectedItem);
            }
        }

        public object SelectedItem_
        {
            get { return (object)GetValue(SelectedItem_Property); }
            set { SetValue(SelectedItem_Property, value); }
        }
        public static readonly DependencyProperty SelectedItem_Property = DependencyProperty.Register("SelectedItem_", typeof(object), typeof(ExtendedTreeView), new UIPropertyMetadata(null));
    }
}
