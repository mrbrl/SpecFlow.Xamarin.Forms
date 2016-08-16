using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpecFlow.XamarinForms.Navigation
{
    /// <summary>
    /// The NavigationService interface.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Gets or sets the current view model type.
        /// </summary>
        Type CurrentViewModelType { get; set; }

        /// <summary>
        /// Gets or sets the current view model.
        /// </summary>
        object CurrentViewModel { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        ContentPage CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the previous view model type.
        /// </summary>
        Type PreviousViewModelType { get; set; }

        /// <summary>
        /// Gets or sets the navigation page.
        /// </summary>
        NavigationPage NavigationPage { get; set; }

        /// <summary>
        /// Gets or sets the navigation root.
        /// </summary>
        INavigation NavigationRoot { get; set; }

        ViewModelMapping ViewModelMapping { get; set; }

        Stack<Type> ViewModelStack { get; }
        

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
        Page Pop<TViewModel>(bool isModal = false, Action<TViewModel, Page> postInitialiser = null)
            where TViewModel : class, INotifyPropertyChanged;

        /// <summary>
        /// The pop async.
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
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Page> PopAsync<TViewModel>(bool isModal = false, Action<TViewModel, Page> postInitialiser = null)
            where TViewModel : class, INotifyPropertyChanged;

        /// <summary>
        /// The pop to root.
        /// </summary>
        /// <param name="initialiser">
        /// The initialiser.
        /// </param>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Page"/>.
        /// </returns>
        void PopToRoot<TViewModel>(Action<TViewModel, Page> postInitialiser = null) where TViewModel : class, INotifyPropertyChanged;

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
        Page Push<TViewModel>(bool isModal = false, Action<TViewModel, Page> postInitialiser = null) where TViewModel : class, INotifyPropertyChanged;

      
        /// <summary>
        /// The push async.
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
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Page> PushAsync<TViewModel>(bool isModal = false, Action<TViewModel, Page> postInitialiser = null, bool isAnimated = true)
            where TViewModel : class, INotifyPropertyChanged;

        void Navigate<TViewModel>(bool isModal = false, 
            Action<TViewModel, Page> postInitialiser = null, bool isAnimated = true) where TViewModel : class, INotifyPropertyChanged;

        void OnPagePoppedComplete();
    }
}
