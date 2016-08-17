using TechTalk.SpecFlow;

namespace SpecFlow.XForms.DemoApp.UnitTest.Infrastructure
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
            // read specflow doc for more info
            base.AfterScenarioBlock();
        }

        /// <summary>
        ///     The before scenario.
        /// </summary>
        [BeforeScenario]
        public void BeforeScenario()
        {
            // runs before each scenario
            // bootstrap test app with your test app and your starting viewmodel
            new TestAppBootstrap().RunApplication<DemoAppTest, MainViewModel>();
        }
    }
}