using TechTalk.SpecFlow;

namespace SpecFlow.XForms
{
    public abstract class TestSetupHooks
    {
        /// <summary>
        ///     The after scenario block.
        /// </summary>
        [AfterScenario]
        public virtual void AfterScenarioBlock()
        {
            TestAppBootstrap.Destroy();
        }
    }
}
