using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using global::Xamarin.Forms.Xaml;
using global::Xamarin.Forms;


namespace SpecFlow.XForms.DemoApp
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
		}
	}
}
