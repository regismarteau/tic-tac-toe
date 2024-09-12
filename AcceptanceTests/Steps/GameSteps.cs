using Domain.ValueObjects;
using FluentAssertions;
using Queries;
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

    [Given("a game started")]
    [When("I start a new game")]
    public async Task WhenIStartANewGame()
    {
        this.GameId = await this.GameRequests.Start();
    }

    [When("^I play on (.+?) cell$")]
    public async Task WhenIPlayOnTopLeftCell(Cell cell)
    {
        await this.GameRequests.Play(this.GameId, cell);
    }

    [When("I attempt to play an unknown game")]
    public async Task WhenIAttemptToPlayAnUnknownGame()
    {
        await this.GameRequests.Play(Guid.NewGuid(), Cell.TopLeft);
    }

    [Then("the game looks like")]
    public async Task ThenTheGameLooksLike(DataTable table)
    {
        var game = await this.GameRequests.GetGame(this.GameId);
        game.Marks.Should().BeEquivalentTo(ToMarks(table));
    }

    [Then("the game ends in a draw")]
    public async Task ThenTheGameEndsInADraw(DataTable table)
    {
        var game = await this.GameRequests.GetGame(this.GameId);
        game.Marks.Should().BeEquivalentTo(ToMarks(table));
        game.Result.Should().Be(ResultDto.Draw);
    }

    [Then("^the game has been won by (me|the computer)$")]
    public async Task ThenTheGameHasBeenWonBy(string winner, DataTable table)
    {
        var game = await this.GameRequests.GetGame(this.GameId);
        game.Marks.Should().BeEquivalentTo(ToMarks(table));
        game.Result.Should().Be(winner == "me" ? ResultDto.WonByPlayerX : ResultDto.WonByPlayerO);
    }

    private static List<MarkDto> ToMarks(DataTable table)
    {
        return table.Header
            .Concat(table.Rows.SelectMany(row => row.Values))
            .Select((cellContent, index) => new { CellContent = cellContent, Index = index })
            .Where(cell => !string.IsNullOrWhiteSpace(cell.CellContent))
            .Select(cell => new MarkDto(
                Symbol: cell.CellContent.ToLowerInvariant() == "x" ? SymbolDto.Cross : SymbolDto.Nought,
                Cell: (CellDto)cell.Index))
            .ToList();
    }
}
