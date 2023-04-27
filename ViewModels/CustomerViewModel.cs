using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMToolkitTest
{
    public class CustomerViewModel : ViewModelBase
    {
        public ICommand BackCommand { get; set; }
        public RelayCommand RunCommand { get; }

        public CustomerViewModel()
        {
            // RunCommand 활용법
            RunCommand = new RelayCommand(() =>
            {

            }, () => true /* CanExcute? */);

            Init();
        }

        private void Init()
        {
            Title = "Customer";
            BackCommand = new RelayCommand(OnBack);
        }

        private void OnBack()
        {
            WeakReferenceMessenger.Default.Send(new NavigationMessage("GoBack"));
        }

        public override void OnNavigated(object sender, object navigationEventArgs)
        {
            Message = "Navigated";
        }
    }
}
