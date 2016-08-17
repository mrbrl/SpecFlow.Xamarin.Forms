using SpecFlow.XFormsDependency;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SpecFlow.XFormsNavigation;
using TechTalk.SpecFlow;


namespace SpecFlow.XForms
{
    public class TestStepBase
    {
        protected readonly ScenarioContext _scenarioContext;

        public TestStepBase(ScenarioContext scenarioContext)
        {
            this._scenarioContext = scenarioContext;
        }
      
        #region Public Properties

        /// <summary>
        /// Gets the current view model.
        /// </summary>
        public INotifyPropertyChanged CurrentViewModelBase
        {
            get
            {
                if (this.ViewModelStack == null || this.ViewModelStack.Count == 0)
                {
                    return null;
                }

                var viewModelType = this.ViewModelStack.Peek();
                return (INotifyPropertyChanged)Resolver.Instance.Resolve(viewModelType);
            }
        }

        /// <summary>
        /// Gets the view model stack.
        /// </summary>
        public Stack<Type> ViewModelStack
        {
            get
            {
                return this.GetService<INavigationService>().ViewModelStack;
            }
        }

        #endregion

        #region Properties



        protected T GetService<T>() where T : class
        {
            return Resolver.Instance.Resolve<T>();
        }

        #endregion

        /// <summary>
        /// The get current view model.
        /// </summary>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TViewModel"/>.
        /// </returns>
        public TViewModel GetCurrentViewModel<TViewModel>() where TViewModel : class, INotifyPropertyChanged
        {
            if (this.ViewModelStack == null || this.ViewModelStack.Count == 0)
            {
                return null;
            }
        
            return (TViewModel)this.GetService<INavigationService>().CurrentViewModel;
        }

        /// <summary>
        /// The get current view model.
        /// </summary>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TViewModel"/>.
        /// </returns>
        public Type GetCurrentViewModelType()
        {
            if (this.ViewModelStack == null || this.ViewModelStack.Count == 0)
            {
                return null;
            }

            return this.ViewModelStack.Peek();
        }
    }
}
