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


    [Then("the game displayed is like")]
    public async Task ThenTheGameDisplayedIsLike(DataTable table)
    {
        var marks = await this.GameRequests.GetAllMarks(this.GameId);
        marks.Should().BeEquivalentTo(new MarksDto(ToMarks(table)));
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
