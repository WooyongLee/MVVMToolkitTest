using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using NewMvvmToolkitTest.Model;
using System;

namespace NewMvvmToolkitTest
{
    [INotifyPropertyChanged]
    public partial class AnotherViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private string _receivedValue;

        public string ReceivedMessage
        {
            get { return _receivedValue; }
            set { SetProperty(ref _receivedValue, value); }
        }
        public AnotherViewModel()
        {
            WeakReferenceMessenger.Default.Register<MyMessage>(this, OnMessageReceived);

            WeakReferenceMessenger.Default.Register<ValueChangedMessage<bool>>(this, (o, m) => OnStateChangedMessage(m));

            // 1) IRecipient 인터페이스를 하나만 구현하는 경우
            // 따로 Generic 타입을 지정해주지 않아도 IRecipient<T> 타입을 알아서 인식
            // WeakReferenceMessenger.Default.Register(this);

            // 2) 별도 Message Handler를 등록
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<string>>(this, HandleMessage);

        }

        private void OnMessageReceived(object recipient, MyMessage message)
        {
            ReceivedMessage = message.Content;
        }


        // 1) IRecipient 인터페이스의 Receive()를 구현하는 방식으로 등록
        public void Receive(ValueChangedMessage<string> message)
        {
            _receivedValue = message.Value;
        }

        // 2) Messenger Handler를 지정해 등록하는 방법
        private void HandleMessage(object recipient, ValueChangedMessage<string> message)
        {
            _receivedValue = message.Value;
        }

        private void OnStateChangedMessage(ValueChangedMessage<bool> message)
        {
            // Handle Message...
        }
    }

    public partial class AnotherViewModelWantedMessage : ObservableObject,
    IRecipient<ValueChangedMessage<bool>>,
    IRecipient<RequestMessage<string>>,
    IRecipient<PropertyChangedMessage<int>>
    {

        public AnotherViewModelWantedMessage()
        {
            //RegisterAll(object recipient) 메서드는 IRecipient 인터페이스로 구현한 모든 메시지의 수신을 등록하기 때문에
            //해당 클래스는 이제 세 가지 타입의 메시지를 수신할 수 있습니다
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public void Receive(ValueChangedMessage<bool> message)
        {
        }

        public void Receive(RequestMessage<string> message)
        {
        }

        public void Receive(PropertyChangedMessage<int> message)
        {
        }
    }
}
