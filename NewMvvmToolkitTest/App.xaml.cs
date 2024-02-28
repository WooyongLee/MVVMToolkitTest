using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using NewMvvmToolkitTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NewMvvmToolkitTest
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private bool blnReady;

        // Current Instance in use
        public new static App Current => (App)Application.Current;

        // Gets the instance to resolve app. services
        public IServiceProvider Services { get; }

        public App()
        {
            InitializeComponent();
            Exit += (_, __) => OnClosing();
            Startup += Application_Startup;
            try
            {
                IServiceProvider serviceProvider = ConfigureServices();
                Services = serviceProvider;
                // Ioc.Default.ConfigureServices(serviceProvider);
            }
            catch (Exception ex) 
            {
                

            }
        }
        private IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();
            // 원하는 서비스를 전략에 맞는 수명 주기로 등록
            services.AddTransient<AnotherViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<OverlayControlViewModel>();

            return services.BuildServiceProvider();
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            blnReady = true;
        }

        private void OnClosing()
        {
            Console.WriteLine("OnClosing");
            Thread.Sleep(1500);
        }
    }

    // Utilize Service
    //public partial class LoginViewModel : ObservableObject
    //{
    //    private readonly ILoginService _loginService;

    //    public LoginViewModel(ILoginService loginService)
    //    {
    //        // ILoginService의 구현체로 LoginService 클래스를 등록했으므로
    //        // LoginService 객체가 주입됨
    //        _loginService = loginService;
    //        // 생성자 매개변수로 전달받는 대신 아래도 가능합니다.
    //        // _loginService = Ioc.Default.GetRequiredService<ILoginService>();
    //    }
    //}

    //public partial class LoginView : UserControl
    //{
    //    public LoginView()
    //    {
    //        DataContext = Ioc.Default.GetService<LoginViewModel>();
    //        InitializeComponent();
    //    }
    //}
}
