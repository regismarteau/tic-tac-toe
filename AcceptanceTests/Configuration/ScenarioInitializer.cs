using Reqnroll;

namespace AcceptanceTests.Configuration
{
    [Binding]
    public class ScenarioInitializer
    {
        [BeforeScenario]
        public static void Init(ScenarioContext context)
        {
            context.Set(new TestServer());
        }

        [AfterScenario]
        public static void Clean(ScenarioContext context)
        {
            context.Get<TestServer>()?.Dispose();
        }
    }
}
