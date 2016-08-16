using System;
using System.ComponentModel;
using SpecFlow.Xamarin.Forms.Dependency;

namespace SpecFlow.Xamarin.Forms
{
    public class TestAppBootstrap
    {
        public TestApp TestApp { get; set; }

        public void RunApplication<TApp, TRootViewModel>() where TApp : TestApp, new() where TRootViewModel : class, INotifyPropertyChanged
        {
            // reset resolver
            Resolver.Reset();

            //init portable app
            TestApp = new TApp();
            TestApp.Init();

            // set main page
            TestApp.SetMainPage<TRootViewModel>();
        }

        public static void Destroy()
        {
            Resolver.Instance = new AutoFacResolver();
        }
    }
}