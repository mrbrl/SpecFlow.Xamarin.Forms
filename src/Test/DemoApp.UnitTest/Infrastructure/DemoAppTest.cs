using SpecFlow.Xamarin.Forms.Navigation;

namespace SpecFlow.Xamarin.Forms.DemoApp.UnitTest.Infrastructure
{
    public class DemoAppTest : TestApp
    {
        protected override void SetViewModelMapping()
        {
            TestViewFactory.EnableCache = false;

            // register your views / viewmodels below
            RegisterView<MainPage, MainViewModel>();
        }

        protected override void InitialiseContainer()
        {
            // add any di registration here
            // Resolver.Instance.Register<TInterface, TType>();
            base.InitialiseContainer();
        }
    }
}
