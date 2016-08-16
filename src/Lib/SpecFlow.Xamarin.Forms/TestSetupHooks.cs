using TechTalk.SpecFlow;

namespace SpecFlow.Xamarin.Forms
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
