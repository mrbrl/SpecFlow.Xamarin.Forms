using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;


namespace SpecFlow.XamarinForms.DemoApp
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

            MainPage = new MainPage();
		}
	}
}
