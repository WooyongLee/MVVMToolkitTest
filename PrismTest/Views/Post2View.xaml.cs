using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrismTest.Views
{
    /// <summary>
    /// Post2View.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Post2View : UserControl
    {
        public Post2View()
        {
            InitializeComponent();

            // ViewModelLocationProvider를 통해 제공.
            // Prism 설정에 의해 View가 생성될 때 대상 ViewModel을 Injection 주입을 통해 자동으로 생성하고 그 결과를 콜백 메서드로 반환하는 기능을 제공
            // 또한 콜백 메서드는 object 타입의 View와 object 타입의 ViewModel을 반환합니다.
            // Prism에서는 ViewModelLocationProvider 클래스를 통해 한 가지 유용한 이벤트(콜백함수 호출 방식)를 제공합니다.
            // 이 기능을 통해 Prism View와 ViewModel의 생성과 흐름을 좀 더 명확히 인지하고,
            // View와 ViewModel이 모두 생성된 이 시점에서 다양한 확장 응용 작업을 할 수 있는 타이밍을 확보할 수 있습니다
            ViewModelLocationProvider.AutoWireViewModelChanged(this, AutoWireViewModelChanged);
        }

        private void AutoWireViewModelChanged(object view, object dataContext)
        {
            if (dataContext is IViewCreatable ui)
            {
                ui.OnInitialized();
            }
        }
    }
}
