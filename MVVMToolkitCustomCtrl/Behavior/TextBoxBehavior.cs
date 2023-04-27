using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMToolkitCustomCtrl
{
    public class TextBoxBehavior : Behavior<TextBox>
    {
        public bool IsSelectAll { get; set; } = true;


        public ICommand EnterCommand
        {
            get { return (ICommand)GetValue(EnterCommandProperty); }
            set { SetValue(EnterCommandProperty, value); }
        }

        // EnterCommand에 대한 DP 
        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.Register(nameof(EnterCommand), typeof(ICommand), typeof(TextBoxBehavior), new PropertyMetadata(null, EnterCommandChanged));

        private static void EnterCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (TextBoxBehavior)d;
            behavior.AssociatedObject.Text = "EnterCommandChanged";
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Enter Key 입력을 통해 EnterCommand 실행
            if ( e.Key == Key.Enter && EnterCommand != null)
            {
                EnterCommand.Execute(e);
                e.Handled = true;
            }
        }

        private void AssociatedObject_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                AssociatedObject.SelectAll();
            }));
        }

        protected override void OnAttached()
        {
            if (IsSelectAll)
            {
                AssociatedObject.GotFocus += AssociatedObject_GotFocus;
            }
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        protected override void OnDetaching()
        {
            if (IsSelectAll)
            {
                AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
            }
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
        }

    }
}
