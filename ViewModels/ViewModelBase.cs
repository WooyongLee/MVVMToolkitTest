using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MVVMToolkitTest
{
    // Define ViewModel Base
    public abstract class ViewModelBase : ObservableObject, INavigationAware
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _message;

        [ObservableProperty]
        public string Message;
        //{
        //    get { return _message; }
        //    set { SetProperty(ref _message, value); }
        //}

        // Navigation 완료시
        public virtual void OnNavigated(object sender, object navigationEventArgs)
        {
        }

        // Navigation 시작시
        public virtual void OnNavigating(object sender, object navigationEventArgs)
        {
        }
    }
}
