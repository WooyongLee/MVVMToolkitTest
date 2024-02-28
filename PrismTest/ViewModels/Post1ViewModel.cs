using Prism;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismTest.ViewModels
{
    public class Post1ViewModel : BindableBase, INavigationAware, IActiveAware
    {
        public string Header { get; set; }
        private IList<string> _messages = new ObservableCollection<string>();

        public event EventHandler IsActiveChanged;

        private bool _isActive;
        /// <summary>
        /// 현재 View에 대한 IsActive 활성화 여부 프로퍼티 - IActiveAware
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, () => Messages.Add($"{GetType().Name} IsActive : {IsActive}")); }
        }

        public IList<string> Messages
        {
            get => _messages; 
            set => SetProperty(ref _messages, value);
        }


        public Post1ViewModel()
        {
            Header = GetType().Name;
            Messages.Add(Header);
        }

        // Navigate를 통한 화면전환 From -> To
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Messages.Add($"{GetType().Name} OnNavigatedTo - " + navigationContext.NavigationService.Journal.CurrentEntry.Uri.ToString());
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (Messages.Count > 10)
            {
                Messages.Clear();
            }

            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Messages.Add($"{GetType().Name} OnNavigatedFrom - " + navigationContext.NavigationService.Journal.CurrentEntry.Uri.ToString());
        }
    }
}
