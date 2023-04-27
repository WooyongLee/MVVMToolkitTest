using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace MVVMToolkitTest
{
    public class LayerPopupMessage : ValueChangedMessage<bool>
    {
        public string ControlName { get; set; }

        public object Parameter { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">True : Open Layer Popup, False : Close Layer Popup</param>
        public LayerPopupMessage(bool value) : base(value)
        {
        }
    }
}