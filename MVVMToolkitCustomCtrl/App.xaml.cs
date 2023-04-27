using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMToolkitCustomCtrl
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        // App 에서 사용할 각종 서비스, 뷰모델들을 Injection 하기 위해 미리 등록함
        public App()
        {
            Services = ConfigureServices();
        }

        // Gets the Current Instance in use
        public new static App Current => (App)Application.Current;

        // Gets the instance to resolve app. services
        public IServiceProvider Services { get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = Services.GetService<MainWindow>();
            if (mainWindow != null)
            {
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }

        // Configures the services for the App
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton(typeof(MainWindow));
            services.AddSingleton(typeof(MainWindowViewModel));

            services.AddTransient(typeof(UserConsent));
            services.AddTransient(typeof(UserConsentViewModel));

            services.AddTransient(typeof(CustomUserConsent));

            return services.BuildServiceProvider();
        }

        // Control 개발 시점에서 Visual Studio Designer에서 활성화되는지 실제 Runtime에서 활성화 된 것인지 구분
        // (Runtime 동작 코드를 Design Time에서 수행되도록 노출 시 디자이너 화면에서 오류를 발생할 수 있기 때문)
        public static bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(new DependencyObject());
        }
    }

}
