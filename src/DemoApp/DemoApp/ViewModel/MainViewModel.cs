using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SpecFlow.XForms.IViewModel;
using Xamarin.Forms;

namespace SpecFlow.XForms.DemoApp
{
    public class MainViewModel : INotifyPropertyChanged, IViewModel.IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _text = "";

        public ICommand GetTextCommand => new Command(() => GetText());
        public ICommand GoForwardVMCommand => new Command(GoForwardVM);

        public INavigation Navigation { get; set; }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private string GetText()
        {
            Text = "TestValue";
            return Text;
        }

        // to allow viewmodel navigation and its testing, your viewmodel needs to implement IViewModel
        // and you need to use any navigation services available from any xamarin.forms framework offering
        // so the below will only work when running the test
        // but not when running the app, as you need to implement your MVVM as you please
        private void GoForwardVM()
        {
            // implement your own INavigation or use an existing framework
            // to populate the INavigation on your ViewModel (populated on ManView codebehind for demo purposes)
            Navigation?.PushAsync(new AnotherPage());
        }
    }
}
