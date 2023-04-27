
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MVVMToolkitTest
{
    public class MainViewModel : ViewModelBase
    {
        private string _navigationSource;

        // Navigation Source
        public string NavigationSource
        {
            get { return _navigationSource; }
            set { SetProperty(ref _navigationSource, value); }
        }

        // Navigation Command
        public ICommand NavigateCommand { get; set; }

        // Busy List
        private IList<BusyMessage> busys = new List<BusyMessage>();

        // Busy 여부
        private bool isBusy;
        public bool IsBusy
        {
            get {  return isBusy; }
            set {  SetProperty(ref isBusy, value); }
        }

        // Laer Popup 출력 여부
        private bool showLayerPopup;
        public bool ShowLayerPopup
        {
            get { return showLayerPopup; }
            set { SetProperty(ref showLayerPopup, value); }
        }

        private string controlName;
        public string ControlName
        {
            get { return controlName; }
            set { SetProperty(ref controlName, value); } 
        }


        public MainViewModel()
        {
            // Test ViewModel
            Title = "Main View";
            this.Init();
        }

        private void Init()
        {
            // Set Start Page
            NavigationSource = "Views/HomePage.xaml";
            NavigateCommand = new RelayCommand<string>(OnNavigate);

            // Register Navigation Message Receiver
            WeakReferenceMessenger.Default.Register<NavigationMessage>(this, OnNavigationMessage);

            // Register Busy Message Receiver
            WeakReferenceMessenger.Default.Register<BusyMessage>(this, OnBusyMessage);

            WeakReferenceMessenger.Default.Register<LayerPopupMessage>(this, OnLayerPopupMessage);
        }

        private void OnLayerPopupMessage(object recipient, LayerPopupMessage message)
        {
            ShowLayerPopup = message.Value;
            ControlName = message.ControlName;
        }

        // Handle Busy Message
        private void OnBusyMessage(object recipient, BusyMessage message)
        {
            if ( message.Value )
            {
                var existBusy = busys.FirstOrDefault(b => b.BusyId == message.BusyId);
                if (existBusy != null)
                {
                    return;
                }
                busys.Add(message);
            }
            else
            {
                var existBusy = busys.FirstOrDefault(b => b.BusyId == message.BusyId);
                if ( existBusy == null )
                {
                    return;
                }
                busys.Remove(existBusy);
            }
            // Busys에 Item 유무 확인하여 IsBusy Flag 값 결정
            IsBusy = busys.Any();
        }

        private void OnNavigationMessage(object recipient, NavigationMessage message)
        {
            NavigationSource = message.Value;
        }

        private void OnNavigate(string pageUri)
        {
            NavigationSource = pageUri;
        }

        // Navigation

    }
}
