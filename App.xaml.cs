using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMToolkitTest
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
            this.InitializeComponent();
        }

        // App.xaml에서 StartUri를 MainWindow 로 설정하지 않는 경우에 해당 코드 주석 해제
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    var mainWindow = Services.GetService<MainWindow>();
        //    if ( mainWindow != null)
        //    {
        //        mainWindow.Show();
        //    }
        //    else
        //    {
        //        Shutdown();
        //    }
        //}

        // Current Instance in use
        public new static App Current => (App)Application.Current;

        // Gets the instance to resolve app. services
        public IServiceProvider Services { get; }

        // Configures the services for the app.
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // AddSingleton : 싱글톤으로 하나의 개체를 계속 사용할 떄,
            // AddTransient : 서비스를 요청할 떄 마다 개체를 생성하는 대상을 등록

            //services.AddSingleton<string, string>();
            //var str =  Ioc.Default.GetService<string>();

            // Main ViewModel 등록
            services.AddTransient(typeof(MainViewModel));
            services.AddTransient(typeof(HomeViewModel));
            services.AddTransient(typeof(CustomerViewModel));

            // Control 등록
            services.AddTransient(typeof(AboutControl));
            
            return services.BuildServiceProvider();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddHostedService<Worker>()
                            .AddScoped<IMessageWriter, LoggingMessageWriter>());
    }


    #region TestCode
    public interface IMessageWriter
    {
        void Write(string message);
    }

    public class MessageWriter : IMessageWriter
    {
        public void Write(string message)
        {
            Console.WriteLine($"MessageWriter.Write(message: \"{message}\")");
        }
    }

    public class Worker : BackgroundService
    {
        private readonly IMessageWriter _messageWriter;

        public Worker(IMessageWriter messageWriter)
        {
            _messageWriter = messageWriter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _messageWriter.Write($"Worker running at : {DateTimeOffset.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    public class LoggingMessageWriter : IMessageWriter
    {
        private readonly ILogger<LoggingMessageWriter> _logger;

        public LoggingMessageWriter(ILogger<LoggingMessageWriter> logger)
        {
            _logger = logger;
        }

        public void Write(string message)
        {
            _logger.LogInformation(message);
        }
    }
    #endregion

}
