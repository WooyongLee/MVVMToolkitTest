using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMToolkitCustomCtrl
{
    [TemplatePart(Name = _partSubmit, Type = typeof(Button))]
    [TemplatePart(Name = _partExit, Type = typeof(Button))]
    public class CustomUserConsent : Control, IDisposable
    {
        private const string _partSubmit = "PART_Submit";
        private const string _partExit = "PART_Exit";

        private Button _submitButton;
        private Button _exitButton;
        private bool disposedValue;

        static CustomUserConsent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomUserConsent), new FrameworkPropertyMetadata(typeof(CustomUserConsent)));
        }

        // Check Isnt User Consent
        public bool IsUserConsent
        {
            get => (bool)GetValue(IsUserConsentProperty);
            set => SetValue(IsUserConsentProperty, value);
        }

        public static readonly DependencyProperty IsUserConsentProperty = 
            DependencyProperty.Register(nameof(IsUserConsent), typeof(bool), typeof(CustomUserConsent), new PropertyMetadata(false));

        // Command To Popup Close
        public ICommand ClosePopupCommand { get; set; }

        public override void OnApplyTemplate()
        {
            _submitButton = GetTemplateChild(_partSubmit) as Button;
            _exitButton = GetTemplateChild(_partExit) as Button;

            // All Buttons null
            if (_submitButton == null || _exitButton == null)
            {
                throw new NullReferenceException($"{_partSubmit} and {_partExit} button cannot be null");
            }

            _submitButton.Click += _submitButton_Click;
            _exitButton.Click += _exitButton_Click;
        }

        private void _exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClosePopupCommand != null)
            {
                ClosePopupCommand.Execute(false);
            }
        }

        private void _submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClosePopupCommand != null)
            {
                ClosePopupCommand.Execute(IsUserConsent);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_submitButton != null)
                    {
                        _submitButton.Click -= _submitButton_Click;
                        _submitButton = null;
                    }
                    if (_exitButton != null)
                    {
                        _exitButton.Click -= _exitButton_Click;
                        _exitButton = null;
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
