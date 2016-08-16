using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecFlow.Xamarin.Forms.Navigation
{
    /// <summary>
    /// The view model mapping.
    /// </summary>
    public class ViewModelMapping
    {
        #region Fields

        /// <summary>
        /// The _view model mapping.
        /// </summary>
        private readonly Dictionary<Type, Type> _viewModelMapping = new Dictionary<Type, Type>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <typeparam name="TView">
        /// </typeparam>
        public void Add<TViewModel, TView>()
        {
            _viewModelMapping.Add(typeof(TViewModel), typeof(TView));
        }

        /// <summary>
        /// The get view type.
        /// </summary>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public Type GetViewType<TViewModel>()
        {
            Type type = typeof(TViewModel);
            if (!_viewModelMapping.ContainsKey(type))
            {
                throw new Exception($"ViewModel type {type} is not mapped to any view");
            }

            return _viewModelMapping[typeof(TViewModel)];
        }

        /// <summary>
        /// The get view type.
        /// </summary>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public Type GetViewModelType<TView>()
        {
            Type type = typeof(TView);
            return GetViewModelType(type);
        }

        /// <summary>
        /// The get view type.
        /// </summary>
        /// <typeparam name="TViewModel">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public Type GetViewModelType(Type viewType)
        {
            if (!_viewModelMapping.ContainsValue(viewType))
            {
                throw new Exception($"View type {viewType} is not mapped to any viewmodel");
            }

            return _viewModelMapping.Single(x => x.Value == viewType).Key;
        }

        #endregion
    }
}
