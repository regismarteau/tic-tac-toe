using AcceptanceTests.ErrorHandling;
using FluentAssertions;
using Reqnroll;

namespace AcceptanceTests.Steps;

public class ErrorSteps : BaseSteps
{
    private readonly ScenarioContext context;
    public ErrorSteps(ScenarioContext context) : base(context)
    {
        this.context = context;
    }

    [Then("^an (.+) error occured$")]
    public void ThenAnCellAlreadyMarkedExceptionErrorOccured(string error)
    {
        var acceptanceError = this.context.Get<AcceptanceError>();
        acceptanceError.Detail.Should().Be(error);
    }
}
