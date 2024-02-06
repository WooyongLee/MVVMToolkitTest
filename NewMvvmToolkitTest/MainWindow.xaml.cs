using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace NewMvvmToolkitTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.SendPanel.DataContext = App.Current.Services.GetService(typeof(MainViewModel));
            this.ReceivePanel.DataContext = App.Current.Services.GetService(typeof(AnotherViewModel));
        }
    }

    public class ObservableValidatorViewModel : ObservableValidator
    {
        private string _numbersOnly;

        [RegularExpression(@"^[0-9]*$")]
        public string NumbersOnly
        {
            get => _numbersOnly;
            set => TrySetProperty(ref _numbersOnly, value, out var errors);
        }
    }

}
