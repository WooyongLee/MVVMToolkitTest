using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MVVMToolkitCustomCtrl
{
    public class TextBoxExBehavior : AttachableForStyleBehavior<TextBox, TextBoxExBehavior>
    {
        public TextBoxExBehavior()
        {
            // Create TextBoxExBehavior
            if ( App.IsInDesignMode())
            {
                // DesignMode에서는 예외발생하지 않기 위한 return 처리 
                return;
            }
        }

        int count = 0;
        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                AssociatedObject.Text = "Text Initialize";
            }));
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
        }

        private void AssociatedObject_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                AssociatedObject.Text = (++count).ToString();
            }));
        }
    }
}
