using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMToolkitTest
{
    public class BusyMessage : ValueChangedMessage<bool>
    {
        public string BusyId {  get; set; }

        public string BusyText { get; set; }

        public BusyMessage(bool value) : base(value)
        {
        }
    }
}
