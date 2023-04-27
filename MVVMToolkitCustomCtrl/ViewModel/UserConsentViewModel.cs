using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMToolkitCustomCtrl
{
    public class UserConsentViewModel : ObservableObject
    {
        // Service Provider
        private readonly IServiceProvider _serviceProvider;
        private bool _isUserConsent;

        // Isn't it User Consent
        public bool IsUserConsent
        {
            get => _isUserConsent;
            set => SetProperty(_isUserConsent, value,
                callback =>
                {
                    _isUserConsent = callback;
                    ((RelayCommand)SubmitCommand).NotifyCanExecuteChanged();
                });
        }

        // Command To Submit
        public ICommand SubmitCommand { get; set; }
        
        // Command To Close Popup
        public ICommand ClosePopupCommand { get; set; }

        // Command To Exit
        public ICommand ExitCommand { get; set; }

        public UserConsentViewModel()
        {

        }

        // Runtime Constructor
        public UserConsentViewModel(IServiceProvider serviceProvider) : this()
        {
            // ServiceProvider를 주입(Injection)
            _serviceProvider = serviceProvider;
            Init();
        }

        private void Init()
        {
            SubmitCommand = new RelayCommand(() =>
            {
                ClosePopupCommand?.Execute(true);
            }, () => IsUserConsent);
            ExitCommand = new RelayCommand(() =>
            {
                ClosePopupCommand?.Execute(false);
            });
        }
    }
}
