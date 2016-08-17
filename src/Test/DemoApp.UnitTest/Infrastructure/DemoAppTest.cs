using SpecFlow.XFormsDependency;
using SpecFlow.XFormsNavigation;

namespace SpecFlow.XForms.DemoApp.UnitTest.Infrastructure
{
    public class DemoAppTest : TestApp
    {
        protected override void SetViewModelMapping()
        {
            TestViewFactory.EnableCache = false;

            // register your views / viewmodels below
            RegisterView<MainPage, MainViewModel>();
            RegisterView<AnotherPage, AnotherViewModel>();
        }

        protected override void InitialiseContainer()
        {
            // add any dependency injection registration goes  here
            //Resolver.Instance.Register<TInterface, TType>();
            base.InitialiseContainer();
        }
    }
}
