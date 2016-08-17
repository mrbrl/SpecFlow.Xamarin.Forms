using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpecFlow.XForms.IViewModel;
using global::Xamarin.Forms;
using global::Xamarin.Forms.Xaml;


namespace SpecFlow.XForms.DemoApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
		    try
		    {
		        InitializeComponent();
		    }
            catch (InvalidOperationException soe)
            {
                // you need this catch otherwise xamarin.forms will not allow a test host to run the app
                if (!soe.Message.Contains("MUST"))
                    throw;
            }

		    SetViewModel();
		}

	    private void SetViewModel()
	    {
            var vm = new MainViewModel();

            // sets navigation on viewmodel when running app (for demo purpose)
            // this is all done internally for you when running tests
            // (a nice mvvm framework would handle that for you)
            if (vm is IViewModel.IViewModel && ((IViewModel.IViewModel)vm).Navigation == null)
                ((IViewModel.IViewModel)vm).Navigation = Navigation;

            // set binding (for demo purpose)
            // again, a mvvm framework would do that for you
            BindingContext = vm;
        }

	    protected async void GoForwardCB(object sender, EventArgs e)
	    {
            // handling navigation from Page is not testable, as we don't have a hook on the navigation
            // you will be able to test you viewmodels in isolation,
            // but navigating within the app will be ignored by the testing framweork
            await Navigation.PushAsync(new AnotherPage());
        }
	}
}