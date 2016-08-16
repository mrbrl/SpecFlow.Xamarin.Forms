using TechTalk.SpecFlow;

namespace SpecFlow.XamarinForms.DemoApp.UnitTest.Infrastructure
{
    [Binding]
    public class SetupHooks : TestSetupHooks
    {       
        /// <summary>
        ///     The after scenario block.
        /// </summary>
        [AfterScenario]
        public void AfterScenarioBlock()
        {
            base.AfterScenarioBlock();
        }

        /// <summary>
        ///     The before scenario.
        /// </summary>
        [BeforeScenario]
        public void BeforeScenario()
        {
            // bootstrap test app with your test app and your starting viewmodel
            new TestAppBootstrap().RunApplication<DemoAppTest, MainViewModel>();
        }
    }
}