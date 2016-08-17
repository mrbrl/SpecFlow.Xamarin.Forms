using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SpecFlow.XForms.IViewModel;
using Xamarin.Forms;

namespace SpecFlow.XForms.DemoApp
{
    public class AnotherViewModel : INotifyPropertyChanged, IViewModel.IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand BackCommand => new Command(() => GoBack());
        
        private void GoBack()
        {
            // implement your own INavigation or use an existing framework
            // to populate the INavigation on your ViewModel (populated on ManView codebehind for demo purposes)
            Navigation.PopToRootAsync();
        }
    }
}
