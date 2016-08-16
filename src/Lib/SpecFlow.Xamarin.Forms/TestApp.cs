using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using SpecFlow.XamarinForms.Dependency;
using SpecFlow.XamarinForms.Navigation;

namespace SpecFlow.XamarinForms
{
    public abstract class TestApp 
    {
        private readonly List<Action> _viewMappingActions;
        
        public Page MainPage { get; set; }
      
        public TestApp()
        {
            _viewMappingActions = new List<Action>();
        }

        public void Init()
        {
            InitialiseContainer();

            SetViewModelMapping();

            Resolver.Instance.Initialise();

            ApplyViewMappings();
        }

        /// <summary>
        /// The register services and repositories.
        /// </summary>
        protected virtual void InitialiseContainer()
        {
            Resolver.Instance.Register<INavigationService, TestNavigationService>(LifetimeScopeEnum.Singleton);
        }

        /// <summary>
        /// The register main page
        /// </summary>
        public void SetMainPage<TViewModel>() where TViewModel : class, INotifyPropertyChanged
        {
            var page = Resolver.Instance.Resolve<INavigationService>().PushAsync<TViewModel>().Result;
            MainPage = Resolver.Instance.Resolve<INavigationService>().NavigationPage;
        }

        /// <summary>
        /// The register views / viewmodels
        /// </summary>
        protected void RegisterView<TView, TViewModel>()
          where TView : class
          where TViewModel : class, INotifyPropertyChanged
        {
            Resolver.Instance.Register<TViewModel>(LifetimeScopeEnum.InstancePerDependency);
            
            _viewMappingActions.Add(() =>
            {
                TestViewFactory.Register<TView, TViewModel>(x => Resolver.Instance.Resolve<TViewModel>());
                Resolver.Instance.Resolve<INavigationService>().ViewModelMapping.Add<TViewModel, TView>();
            });
        }

        protected abstract void SetViewModelMapping();

        private void ApplyViewMappings()
        {
            foreach (Action action in _viewMappingActions)
                action.Invoke();
        }
    }
}
