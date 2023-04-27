using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;
using System;

namespace MVVMToolkitTest
{
    public class FrameBehaviors : Behavior<Frame>
    {
        // NavigationSource DP 변경으로 발생하는 PropertyChanged Event를 막기위한 변수
        private bool _isWork;

        protected override void OnAttached()
        {
            // Navigation 시작
            AssociatedObject.Navigating += AssociatedObject_Navigating;

            // Navigation 종료
            AssociatedObject.Navigated += AssociatedObject_Navigated;
        }

        // 네비게이션 종료 이벤트 핸들러
        private void AssociatedObject_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            _isWork = true;

            NavigationSource = e.Uri.ToString();
            _isWork = false;

            if ( AssociatedObject.Content is Page pageContent
                && pageContent.DataContext is INavigationAware navigationAware)
            {
                navigationAware.OnNavigated(sender, e);
            }
        }

        // 네비게이션 시작 이벤트 핸들러
        private void AssociatedObject_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            // Navigation Previous Start State를 ViewModel에 Notify함
            if ( AssociatedObject.Content is Page pageContent && pageContent.DataContext is INavigationAware navigationAware)
            {
                navigationAware?.OnNavigating(sender, e);
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Navigating -= AssociatedObject_Navigating;
            AssociatedObject.Navigated -= AssociatedObject_Navigated;
        }

        public string NavigationSource
        {
            get { return (string)GetValue(NavigationSourceProperty); }
            set { SetValue(NavigationSourceProperty, value); }
        }

        // NavigationSource DP
        public static readonly DependencyProperty NavigationSourceProperty =
            DependencyProperty.Register(nameof(NavigationSource), typeof(string), typeof(FrameBehaviors), new PropertyMetadata(null, NavigationSourceChanged));

        private static void NavigationSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (FrameBehaviors)d;
            if ( behavior._isWork )
            {
                return;
            }
            behavior.Navigate();
        }

        private void Navigate()
        {
            // Navigation할 항목에 따라 분기함
            switch (NavigationSource)
            {
                case "GoBack":
                    if ( AssociatedObject.CanGoBack )
                    {
                        AssociatedObject.GoBack();
                    }
                    break;
                case null:
                case "":
                    return;
                default:
                    AssociatedObject.Navigate(new Uri(NavigationSource, UriKind.RelativeOrAbsolute));
                    break;
            }
        }
    }

    // Navigation Start-Stop 시점을 ViewModel에 알려주는 Interface
    public interface INavigationAware 
    {
        void OnNavigating(object sender, object navigationEventArgs);
        void OnNavigated(object sender, object navigationEventArgs);
    }

}
