using FluentAssertions;
using Reqnroll;

namespace AcceptanceTests.Steps;

[Binding]
public class GameSteps(ScenarioContext context) : BaseSteps(context)
{
    private Guid GameId
    {
        get => this.Context.Get<Guid>();
        set => this.Context.Set(value);
    }

    [When("I start a new game")]
    public async Task WhenIStartANewGame()
    {
        this.GameId = await this.GameRequests.Start();
    }

    [Then("The game displayed is like")]
    public void ThenTheGameDisplayedIsLike(DataTable _)
    {
        this.GameId.Should().NotBeEmpty();
    }

}
