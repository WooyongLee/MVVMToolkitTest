using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace PrismTest.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IContainerProvider _containerProvider;

        private readonly IRegionManager _regionManager;

        private IList<string> _navigationNames;

        private string _title = "Prism Application";

        private string _selectedNavigationName;

        /// <summary>
        /// 선택된 네비게이션 할 뷰이름
        /// </summary>
        public string SelectedNavigationName
        {
            get { return _selectedNavigationName; }
            set { SetProperty(ref _selectedNavigationName, value); }
        }

        public IList<string> NavigationNames
        {
            get { return _navigationNames; }
            set { SetProperty(ref _navigationNames, value); }   
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// NavigateCommand
        /// </summary>
        public ICommand NavigateCommand { get; set; }

        public ICommand CheckObjectCommand { get; set; }

        public ICommand CreateObjectCommand { get; set; }

        public ICommand NavigationCommand { get; set; }

        public MainWindowViewModel(IContainerProvider containerProvider, IRegionManager regionManager)
        {
            // Prism에서 사용하고 있는 IoC Container를 Constructor Injection 후 local variable로 등록하여 사용
            _containerProvider = containerProvider;
            _regionManager = regionManager;

            // 미리 예약함
            _regionManager.RegisterViewWithRegion("ContentRegion1", "Post1View");

            NavigateCommand = new DelegateCommand(OnNavigate, CanNavigate).ObservesProperty(() => SelectedNavigationName);

            CheckObjectCommand = new DelegateCommand(OnCheckObject);

            CreateObjectCommand = new DelegateCommand(OnCreateObject);

            NavigationCommand = new DelegateCommand<string>(OnNavigation);

            NavigationNames = new List<string>
            {
                "Sample1View",
                "Sample2View"
            };
        }

        private void OnNavigation(string obj)
        {
            switch (obj)
            {
                //Back이란 문자열이 들어오면..
                case "Back":
                    {
                        //Back을 구현하기 위해서 ContentRegion의 Journal을 가져오고, 뒤로가기가 가능한지 확인 후 실행
                        IRegionNavigationJournal journal = _regionManager.Regions["ContentRegion"]
                                                                .NavigationService.Journal;
                        if (journal.CanGoBack)
                        {
                            journal.GoBack();
                        }
                    }
                    break;
                //그외 일반 문자열이 들어오면    
                default:
                    // 지정 화면으로 Navigating
                    _regionManager.RequestNavigate("ContentRegion", obj);
                    break;
            }
        }

        private void OnCreateObject()
        {
            // 식별자를 이용하여 하나의 인스턴스를 여러 식별자로 등록하여 사용 가능
            // 하나의 유형을 이용하여 다수의 인스턴스를 만들고, 각각 등록해서 사용 가능

            var person = new Person
            {
                Id = 100,
                Name = "abcd",
                Address = "Ori"
            };

            var p1 = _containerProvider.Resolve<Person>("name1");
            var p2 = _containerProvider.Resolve<Person>("name2");

            // p1 == p2 is true, single object but only different id
            Console.WriteLine("is p1 == p2 : " + p1.Equals(p2).ToString());
        }

        private void OnCheckObject()
        {
            // app.xaml.cs 에서 containerRegistry.RegisterSingleton 부분에 해당 Container 를 SingleTon으로 등록 시에 same 반환
            var person1 = _containerProvider.Resolve<Person>();
            var person2 = _containerProvider.Resolve<Person>();

            if (person1.Equals(person2))
            {
                MessageBox.Show("Same Instance");
            }
            else
            {
                MessageBox.Show("Different Instance");
            }
        }

        // NavigateCommand를 View에서 호출했을 때 실행되는 method
        private void OnNavigate()
        {
            // Combobox 에서 설정된 Item을 Console에 Print
            Console.WriteLine($"OnNavigate : {SelectedNavigationName}");
        }

        // Navigation 사용 여부
        private bool CanNavigate()
        {
            return string.IsNullOrEmpty(SelectedNavigationName) == false;
        }

    }
}
