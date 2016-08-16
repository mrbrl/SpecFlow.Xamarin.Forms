using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpecFlow.Xamarin.Forms.DemoApp
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
                if (!soe.Message.Contains("MUST"))
                    throw;
            }

            BindingContext = new MainViewModel();
		}
    }
}
