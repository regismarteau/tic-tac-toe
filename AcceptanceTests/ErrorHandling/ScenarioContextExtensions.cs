using Reqnroll;

namespace AcceptanceTests.ErrorHandling;

public static class ScenarioContextExtensions
{
    private const string ErrorHandlingTag = "ErrorHandling";

    public static bool IsAnErrorHandlingScenario(this ScenarioContext context)
    {
        return context.ScenarioInfo.CombinedTags.Contains(ErrorHandlingTag);
    }
}

public record AcceptanceError(string? Title, string? Detail);
