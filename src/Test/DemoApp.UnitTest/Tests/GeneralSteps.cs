using SpecFlow.XFormsDependency;
using SpecFlow.XFormsExtensions;
using SpecFlow.XFormsNavigation;
using TechTalk.SpecFlow;
using Xamarin.Forms;

namespace SpecFlow.XForms.DemoApp.UnitTest.Tests
{
    [Binding]
    public class GeneralSteps : TestStepBase
    {
        public GeneralSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            // you need to instantiate your steps by passing the scenarioContext to the base
        }

        [Given(@"I am on the main page")]
        public void GivenIAmOnTheMainPage()
        {
            Resolver.Instance.Resolve<INavigationService>().PushAsync<MainViewModel>();           
            Resolver.Instance.Resolve<INavigationService>().CurrentViewModelType.ShouldEqualType<MainViewModel>();
        }
        
        [When(@"I click on the text button")]
        public void WhenIClickOnTheTextButton()
        {
            GetCurrentViewModel<MainViewModel>().GetTextCommand.Execute(null);
        }

        [Then(@"I can see a Label with text ""(.*)""")]
        public void ThenICanSeeALabelWithText(string text)
        {
            GetCurrentViewModel<MainViewModel>().Text.ShouldEqual(text);
        }

        [When(@"I click on the goforward button")]
        public void WhenIClickOnTheGoforwardButton()
        {
            GetCurrentViewModel<MainViewModel>().GoForwardVMCommand.Execute(null);
        }

        [Then(@"I am redirected to the page ""(.*)""")]
        public void ThenIAmRedirectedToTheView(string name)
        {
            Resolver.Instance.Resolve<INavigationService>().CurrentPage.GetType().Name.ShouldEqual($"{name}{nameof(Page)}");
            Resolver.Instance.Resolve<INavigationService>().CurrentViewModelType.Name.ShouldEqual($"{name}ViewModel");
        }

        [When(@"I click on the goback button")]
        public void WhenIClickOnTheGobackButton()
        {
            GetCurrentViewModel<AnotherViewModel>().BackCommand.Execute(null);
        }
    }
}