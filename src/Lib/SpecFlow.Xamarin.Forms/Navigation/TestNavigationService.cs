using SpecFlow.XFormsDependency;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecFlow.XFormsExtensions;
using Xamarin.Forms;

namespace SpecFlow.XFormsNavigation
{
    public class TestNavigationService : INavigationService
    {
        /// <summary>
        /// The _navigation root.
        /// </summary>
        protected INavigation _navigationRoot;

        /// <summary>
        /// The _navigation page.
        /// </summary>
        private NavigationPage _navigationPage;

        private bool _isBusy = false;
        private object _currentViewModel;

        /// <summary>
        ///     The _view model mapping.
        /// </summary>
        public ViewModelMapping ViewModelMapping { get; set; }

        public ContentPage CurrentPage { get; set; }

        /// <summary>
        ///     Gets or sets the current view model.
        /// </summary>
        public Type CurrentViewModelType { get; set; }

        /// <summary>
        ///     Gets or sets the previous view model.
        /// </summary>
        public Type PreviousViewModelType { get; set; }
     
        public object CurrentViewModel
        {
            get
            {
                return NavigationRoot?.NavigationStack?.FirstOrDefault(x => x.BindingContext.GetType() == CurrentViewModelType)?.BindingContext ?? _currentViewModel;
            }
            set { _currentViewModel = value; }
        }
        
        /// <summary>
        ///     Sets the current navigation.
        /// </summary>
        public Stack<Type> ViewModelStack
        {
            get
            {
                if (_navigationRoot == null)
                    return new Stack<Type>();

                Stack<Type> stack = new Stack<Type>();

                foreach (var view in _navigationRoot.NavigationStack.ToList())
                    stack.Push(ViewModelMapping.GetViewModelType(view.GetType()));

                return stack;
            }
        }

        /// <summary>
        ///     Gets or sets the navigation page.
        /// </summary>
        public NavigationPage NavigationPage
        {
            get
            {
                return _navigationPage;
            }
            set
            {
                _navigationPage = value;
                _navigationRoot = _navigationPage.Navigation;
            }
        }

        /// <summary>
        ///     Sets the current navigation.
        /// </summary>
        public INavigation NavigationRoot
        {
            get
            {
                return _navigationRoot;
            }
            set
            {
                _navigationRoot = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="viewStateService">
        /// The view state service.
        /// </param>
        /// <param name="initialiserService">
        /// </param>
        public TestNavigationService()

        {
            ViewModelMapping = new ViewModelMapping();
        }

        public virtual async void Navigate<TViewModel>(bool isModal = false, 
        Action<TViewModel, Page> postInitialiser = null) where TViewModel : class, INotifyPropertyChanged
        {
            var page = Resolver.Instance.Resolve<INavigationService>().PushAsync<TViewModel>(isModal, postInitialiser);
        }

        public async Task<Page> PushAsync<TViewModel>(bool isModal = false, Action<TViewModel, Page> postInitialiser = null,
            bool isAnimated = true) where TViewModel : class, INotifyPropertyChanged
        {
            if (this.ViewModelStack.Contains(typeof(TViewModel)))
                return default(Page);

            var previousViewModelType = PreviousViewModelType;
            var currentViewModelType = CurrentViewModelType;
            var currentViewModel = CurrentViewModel;

            PreviousViewModelType = CurrentViewModelType;
            CurrentViewModelType = typeof(TViewModel);

            try
            {
                Page view = null;

                try
                {
                    view = TestViewFactory.CreatePage(postInitialiser) as Page;

                }
                catch (Exception ex)
                {
                    // this forces the page to create out of the UI thread
                    if (ex.GetType().Name != "UIKitThreadAccessException")
                        throw;

                    view = TestViewFactory.CreatePage(postInitialiser) as Page;
                    CurrentViewModel = (TViewModel)view?.BindingContext;
                }

                if (view == null)
                    throw new Exception($"view for {typeof(TViewModel)} failed to create");

                if (isModal)
                {
                    if (view.Parent == null && !NavigationRoot.ModalStack.Contains(view))
                        await NavigationRoot.PushModalAsync(view, isAnimated);

                    if (view.Parent == null && !NavigationRoot.ModalStack.Contains(view))
                        await NavigationRoot.PushModalAsync(view, isAnimated);
                }
                else
                {
                    if (view.Parent == null && NavigationRoot != null && !NavigationRoot.NavigationStack.Contains(view))
                    {
                        await NavigationRoot.PushAsync(view, isAnimated);
                    }
                    else
                    {
                        // if NavigationRoot is null, then we are setting the first page
                        if (NavigationRoot == null)
                        {
                            NavigationPage = new NavigationPage(view);

                            NavigationPage.Popped += NavigationPageOnPopped;

                            NavigationPage.PoppedToRoot += NavigationPageOnPoppedToRoot;
                        }

                    }
                }

                CurrentViewModel = (TViewModel)view?.BindingContext;
                CurrentPage = (ContentPage)view;

                return view;
            }
            catch (Exception ex)
            {
                // nav roollback state
                PreviousViewModelType = previousViewModelType;
                CurrentViewModelType = currentViewModelType;
                CurrentViewModel = currentViewModel;
                throw;
            }
        }

        private void NavigationPageOnPoppedToRoot(object sender, NavigationEventArgs navigationEventArgs)
        {
            var page = navigationEventArgs.Page;
            PreviousViewModelType = CurrentViewModelType;
            CurrentViewModelType = ViewModelStack.Peek().GetType();
            CurrentViewModel = NavigationRoot.NavigationStack
                .FirstOrDefault(x => x.BindingContext.GetType() == CurrentViewModelType)?.BindingContext;

            CurrentPage = (ContentPage)NavigationRoot.NavigationStack.FirstOrDefault();

            // avail soon on xlabs
            //TestViewFactory.ClearCache();
        }

        private void NavigationPageOnPopped(object sender, NavigationEventArgs navigationEventArgs)
        {
            var page = navigationEventArgs.Page;
            PreviousViewModelType = CurrentViewModelType;
            CurrentViewModelType = ViewModelStack.Peek().GetUnderlyingType();
            CurrentViewModel = NavigationRoot.NavigationStack
                 .FirstOrDefault(x => x.BindingContext.GetType() == CurrentViewModelType)?.BindingContext;
            CurrentPage = CurrentPage = (ContentPage)NavigationRoot.NavigationStack.FirstOrDefault();
        }
         

        public void PopToRoot<TViewModel>(Action<TViewModel, Page> postInitialiser = null) where TViewModel : class, INotifyPropertyChanged
        {
            NavigationRoot.PopToRootAsync();

            PreviousViewModelType = CurrentViewModelType;
            CurrentViewModelType = typeof(TViewModel);
        }

        /// <summary>
        /// The pop.
        /// </summary>
        /// <param name="isModal">
        /// The is modal.
        /// </param>
        /// <param name="initialiser">
        /// The initialiser.
        /// </param>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Page"/>.
        /// </returns>
        public virtual Page Pop<TViewModel>(bool isModal = false, Action<TViewModel, Page> postInitialiser = null)
        where TViewModel : class, INotifyPropertyChanged
        {
            Page view = TestViewFactory.CreatePage(postInitialiser) as Page;

            if (isModal)
            {
                if (NavigationRoot.ModalStack.Any())
                    NavigationRoot.PopModalAsync(false);
            }
            else
            {
                NavigationRoot.PopAsync();
            }

            PreviousViewModelType = CurrentViewModelType;
            CurrentViewModelType = typeof(TViewModel);

            return view;
        }

        /// <summary>
        /// The pop async.
        /// </summary>
        /// <param name="isModal">
        /// The is modal.
        /// </param>
        /// <param name="initialiser">
        /// The initialiser.
        /// </param>
        /// <typeparam name="TViewModel">        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<Page> PopAsync<TViewModel>(
        bool isModal = false, Action<TViewModel, Page> postInitialiser = null) where TViewModel : class, INotifyPropertyChanged
        {
            return Pop(isModal, postInitialiser);
        }

        public virtual void Navigate<TViewModel>(bool isModal = false, Action<TViewModel, Page> postInitialiser = null, bool isAnimated = true) where TViewModel : class, INotifyPropertyChanged
        {
            PushAsync<TViewModel>(isModal,  postInitialiser, isAnimated);
        }

        /// <summary>
        /// The push.
        /// </summary>
        /// <param name="isModal">
        /// The is modal.
        /// </param>
        /// <param name="initialiser">
        /// The initialiser.
        /// </param>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Page"/>.
        /// </returns>
        public virtual Page Push<TViewModel>(bool isModal = false, Action<TViewModel, Page> postInitialiser = null) where TViewModel : class, INotifyPropertyChanged
        {
            return PushAsync(isModal,  postInitialiser).Result;
        }

        public void OnPagePoppedComplete()
        {
            PreviousViewModelType = CurrentViewModelType;
            CurrentViewModelType = ViewModelStack.Peek().GetUnderlyingType();
            CurrentViewModel = NavigationRoot.NavigationStack
            .FirstOrDefault(x => x.BindingContext.GetType() == CurrentViewModelType)?.BindingContext;
        }
    }
}