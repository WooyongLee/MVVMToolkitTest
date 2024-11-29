using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NewMvvmToolkitTest.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMvvmToolkitTest.ViewModel
{
    public class OverlayControlViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private string _overlayText;

        public RelayCommand HideUserControlCommand { get; }


        public OverlayControlViewModel()
        {
            HideUserControlCommand = new RelayCommand(HideUserControl);

        }


        private void HideUserControl()
        {
            Messenger.Send(new MessageToHideUserControl());
        }
    }
}
