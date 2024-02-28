using Prism.DryIoc;
using Prism.Ioc;
using PrismTest.ViewModels;
using PrismTest.Views;
using System;
using System.Reflection;
using System.Windows;

namespace PrismTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        ///// <summary>
        ///// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        ///// </summary>
        //public IServiceProvider Services { get; }


        ///// <summary>
        ///// Configures the services for the application.
        ///// </summary>
        //private static IServiceProvider ConfigureServices()
        //{
        //    var services = new ServiceCollection();

        //    services.AddSingleton(typeof(MainWindow));
        //    services.AddSingleton(typeof(MainWindowViewModel));

        //    return services.BuildServiceProvider();
        //}

        /// <summary>
        /// 뷰모델 로케이터 이름 설정
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            Prism.Mvvm.ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                string viewName = viewType.FullName;
                if (viewName == null)
                {
                    return null;
                }

                if (viewName.EndsWith("View"))
                {
                    viewName = viewName.Substring(0, viewName.Length - 4);
                }

                viewName = viewName.Replace(".Views.", ".ViewModels.");
                string viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                string viewModelName = $"{viewName}ViewModel, {viewAssemblyName}";
                return Type.GetType(viewModelName);
            });
        }

        protected override Window CreateShell()
        {
            // return base.CreateShell();

            // shell :: Prism 애플리케이션의 UI, 초기 창을 정의하는데 사용
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register Singleton object (specific class)
            containerRegistry.RegisterSingleton<Person>(() => new Person { Id = 100, Name = "abcd", Address = "Ori"});

            var person = new Person
            {
                Id = 100,
                Name = "abcd",
                Address = "Ori"
            };

            // Register some class
            containerRegistry.RegisterInstance(person, "name1");
            containerRegistry.RegisterInstance(person, "name2");

            // System.Windows 의 Window를 등록할 때 사용한다고 하였지만, 오류가 발생함
            // containerRegistry.RegisterDialogWindow<AnotherWIndow>();

            // Register Navigation UI
            containerRegistry.RegisterForNavigation<Post1View>();
            containerRegistry.RegisterForNavigation<Post2View>();

        }
    }
}
