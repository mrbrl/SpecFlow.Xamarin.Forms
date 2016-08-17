using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Specflow.XForms.Helpers;
using SpecFlow.XFormsNavigation;
using Xamarin.Forms;

namespace Specflow.XForms.Navigation
{
    public class TestNavigation : INavigation
    {
        private readonly INavigationService _navigationService;

        public TestNavigation(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void InsertPageBefore(Page page, Page before)
        {
            _navigationService.NavigationRoot.InsertPageBefore(page, before);
        }

        public Task<Page> PopAsync()
        {
            var vmType = _navigationService.PreviousViewModelType;
            return (Task<Page>)ReflectionHelpers.ExecuteGenericMethod(_navigationService, _navigationService.GetType(), vmType, nameof(PopAsync), false, null);
        }

        public Task<Page> PopAsync(bool animated)
        {
            return PopAsync();
        }

        public Task<Page> PopModalAsync()
        {
            var vmType = _navigationService.PreviousViewModelType;
            return (Task<Page>)ReflectionHelpers.ExecuteGenericMethod(_navigationService, _navigationService.GetType(), vmType, nameof(PopAsync), true, null);
        }

        public Task<Page> PopModalAsync(bool animated)
        {
            return PopModalAsync();
        }

        public Task PopToRootAsync()
        {
            var vmType = _navigationService.ViewModelStack.AsEnumerable().Last();
            return (Task<Page>)ReflectionHelpers.ExecuteGenericMethod(_navigationService, _navigationService.GetType(), vmType, nameof(PopAsync), false, null);
        }

        public Task PopToRootAsync(bool animated)
        {
            return PopToRootAsync();
        }

        public Task PushAsync(Page page)
        {
            var vmType = _navigationService.ViewModelMapping.GetViewModelType(page.GetType());
            return (Task<Page>)ReflectionHelpers.ExecuteGenericMethod(_navigationService, _navigationService.GetType(), vmType, nameof(PushAsync), false, null, false);
        }
 

        public Task PushAsync(Page page, bool animated)
        {
            return PushAsync(page);
        }

        public Task PushModalAsync(Page page)
        {
            var vmType = _navigationService.ViewModelMapping.GetViewModelType(page.GetType());
            return (Task<Page>)ReflectionHelpers.ExecuteGenericMethod(_navigationService, _navigationService.GetType(), vmType, nameof(PushAsync), true, null, false);
        }

        public Task PushModalAsync(Page page, bool animated)
        {
            return PushModalAsync(page);
        }

        public void RemovePage(Page page)
        {
            _navigationService.NavigationRoot.RemovePage(page);
        }

        public IReadOnlyList<Page> ModalStack => _navigationService.NavigationRoot.ModalStack;
        public IReadOnlyList<Page> NavigationStack => _navigationService.NavigationRoot.NavigationStack;
    }
}
