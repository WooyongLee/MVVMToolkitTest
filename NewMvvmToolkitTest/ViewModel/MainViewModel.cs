using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using NewMvvmToolkitTest.Messenger;
using NewMvvmToolkitTest.Model;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace NewMvvmToolkitTest
{
    public partial class MainViewModel : ObservableRecipient, INotifyPropertyChanged
    {
        private bool _state;

        [RelayCommand]
        private void OnStateChanged()
        {
            ValueChangedMessage<bool> message = new ValueChangedMessage<bool>(_state);
            WeakReferenceMessenger.Default.Send(message);
        }
        
        public RelayCommand SendMessageCommand { get; }

        public RelayCommand ShowUserControlCommand { get; }


        [NotifyDataErrorInfo]
        private string msg;

        [MinLength(30)]
        public string Msg
        {
            get => msg;
            set => SetProperty(ref msg, value);
        }
        // in view, <TextBox Text="{Binding Msg, UpdateSourceTrigger=PropertyChanged, ValidatesOnDataErrors=True}"/>

        // INotifyPropertyChanged, ObservableObject, ObservableRecipient

        private ICommand _cmdMsg;
        public ICommand SendMsg
        {
            get
            {
                return _cmdMsg;
            }
        }

        public MainViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessage);
            ShowUserControlCommand = new RelayCommand(ShowOverlay);

        }

        private void ShowOverlay()
        {
            MyMessenger.Instance.Publish(new MessageEvent(AnotherViewModel.StrShowOverlay));
        }

        private void SendMessage()
        {
            var message = new MyMessage { Content = "Hello from SenderViewModel!" };
            Messenger.Send(message);

        }
    }
}
