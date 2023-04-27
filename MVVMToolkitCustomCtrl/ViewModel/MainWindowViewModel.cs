using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMToolkitCustomCtrl
{
    public class MainWindowViewModel : ObservableObject
    {
        private bool _isPopupOpen;

        // Open PopUp
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => SetProperty(ref _isPopupOpen, value);
        }

        public object _popupContent;

        // Popup Content
        public object PopupContent
        {
            get => _popupContent;
            set => SetProperty(ref _popupContent, value);
        }

        private string _message;
        
        // Express Result Message
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        // Service Provider
        private readonly IServiceProvider _serviceProvider;

        // Command To PopUp
        public ICommand ShowPopupCommand { get; set; }

        // Command To Close Popup 
        public ICommand ClosePopupCommand { get; set; }

        // Command To Enter at TextBox
        public ICommand EnterCommand { get; set; }

        // Window Loaded Command
        public ICommand WindowLoadedCommand { get; set; }

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(IServiceProvider serviceProvider) : this()
        {
            // App.Current.Service.GetService가 아니라 외부에서 생성자 Call 하면서 Injection을 받아옴
            _serviceProvider = serviceProvider;

            ShowPopupCommand = new RelayCommand<string>(OnShowPopup);
            ClosePopupCommand = new RelayCommand<bool>(b =>
            {
                // Express Result
                Message = b ? "동의 했습니다" : "동의가 완료되지 않았습니다";
                IsPopupOpen = false;
                PopupContent = null;
            });
            EnterCommand = new RelayCommand(OnEnter);
            WindowLoadedCommand = new RelayCommand(OnLoaded);
        }

        private void OnLoaded()
        {
            MessageBox.Show("Window Loaded Success");
        }

        private void OnEnter()
        {
            // MessageBox.Show(Name);
        }

        // Open PopUp
        private void OnShowPopup(string para)
        {
            IsPopupOpen = true;
            if ( para == "UserControl")
            {
                var consent = _serviceProvider.GetService<UserConsent>();
                if (consent == null)
                {
                    return;
                }
                consent.Width = 300;
                consent.Height = 200;
                if (consent.DataContext is UserConsentViewModel viewModel)
                {
                    viewModel.ClosePopupCommand = ClosePopupCommand;
                }
                PopupContent = consent;
            }

            else
            {
                var consent = _serviceProvider.GetService<CustomUserConsent>();
                if(consent == null)
                {
                    return;
                }
                consent.Width = 300;
                consent.Height = 200;
                consent.ClosePopupCommand = ClosePopupCommand;
                PopupContent = consent;
            }
        }
    }
}
