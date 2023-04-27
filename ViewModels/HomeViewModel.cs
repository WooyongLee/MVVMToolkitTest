using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMToolkitTest
{
    public class HomeViewModel : ViewModelBase
    {
        public static int Count { get; set; }

        // Busy Test Command
        public ICommand BusyTestCommand { get; set; }
        
        public ICommand LayerPopupCommand { get; set; }

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        public HomeViewModel()
        {
            Title = "Home";

            Init();
        }

        public override void OnNavigated(object sender, object navigatedEventArgs)
        {
            Count++;
            Message = $"{Count} Navigated";
        }

        public void Init()
        {
            BusyTestCommand = new AsyncRelayCommand(OnBusyTestAsync);
            LayerPopupCommand = new RelayCommand(OnLayerPopupTest);

            Price = 10000000;
            Console.WriteLine("Price : " + Price);
        }

        private void OnLayerPopupTest()
        {
            WeakReferenceMessenger.Default.Send(new LayerPopupMessage(true) { ControlName = "AboutControl" });
        }

        private async Task OnBusyTestAsync()
        {
            WeakReferenceMessenger.Default.Send(new BusyMessage(true) { BusyId = "OnBusyTestAsync" });
            await Task.Delay(5000);
            WeakReferenceMessenger.Default.Send(new BusyMessage(false) { BusyId = "OnBusyTestAsync" });
        }

        public override void OnNavigating(object sender, object navigationEventArgs)
        {
        }
    }
}
