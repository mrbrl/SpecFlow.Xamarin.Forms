using System;
using System.Collections.Generic;
using System.ComponentModel;
using SpecFlow.XForms.IViewModel;
using SpecFlow.XFormsDependency;
using Xamarin.Forms;


namespace SpecFlow.XFormsNavigation
{
    // we cannot use xlabs built in ViewFactory as it forces viewmodels to implement xlabs viewmodel base
    // and we want to allow any framework to use this testing library
    public static class TestViewFactory
    {
        private static readonly Dictionary<Type, Type> TypeDictionary = new Dictionary<Type, Type>();
        private static readonly Dictionary<string, Tuple<INotifyPropertyChanged, object>> PageCache = new Dictionary<string, Tuple<INotifyPropertyChanged, object>>();

        public static bool EnableCache { get; set; }

        public static void Register<TView, TViewModel>(Func<IResolver, TViewModel> func = null) where TView : class where TViewModel : class, INotifyPropertyChanged
        {
            TypeDictionary[typeof(TViewModel)] = typeof(TView);
        }

        public static object CreatePage(Type viewModelType, Action<object, object> initialiser = null, params object[] args)
        {
            if (!TypeDictionary.ContainsKey(viewModelType))
                throw new InvalidOperationException("Unknown View for ViewModel");

            Type type = TypeDictionary[viewModelType];

            string key = string.Format("{0}:{1}", new object[2]
            {
                (object) viewModelType.Name,
                (object) type.Name
            });

            INotifyPropertyChanged viewModel;
            object instance;

            if (EnableCache && PageCache.ContainsKey(key))
            {
                Tuple<INotifyPropertyChanged, object> tuple = PageCache[key];
                viewModel = tuple.Item1;
                instance = tuple.Item2;
            }
            else
            {
                viewModel = (Resolver.Instance.Resolve(viewModelType) ?? Activator.CreateInstance(viewModelType)) as INotifyPropertyChanged;
                instance = Activator.CreateInstance(type, args);

                if (EnableCache)
                    PageCache[key] = new Tuple<INotifyPropertyChanged, object>(viewModel, instance);
            }

            initialiser?.Invoke((object)viewModel, instance);

            BindableObject bindableObject = instance as BindableObject;

            if (bindableObject != null)
            {
                bindableObject.BindingContext = (object)null;
                bindableObject.BindingContext = (object)viewModel;
            }

            Page page = instance as Page;

            if (page != null && viewModel is IViewModel)
                ((IViewModel) viewModel).Navigation = Resolver.Instance.Resolve<INavigation>(); //page.Navigation;

            return instance;
        }

        public static object CreatePage<TViewModel, TPage>(Action<TViewModel, TPage> initialiser = null, params object[] args) where TViewModel : class, INotifyPropertyChanged
        {
            return CreatePage(typeof(TViewModel), (Action<object, object>)((o1, o2) =>
            {
                if (initialiser == null)
                    return;

                initialiser((TViewModel)o1, (TPage)o2);
            }), args);
        }

        public static void ClearCache()
        {
            PageCache?.Clear();
        }
    }
}
