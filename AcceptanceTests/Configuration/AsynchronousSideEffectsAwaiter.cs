using Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AcceptanceTests.Configuration;

public class AsynchronousSideEffectsAwaiter(TicTacToeDbContext dbContext)
{
    public async Task WaitForSideEffects()
    {
        var isThereAnyEventsToHandle = true;
        do
        {
            isThereAnyEventsToHandle = await dbContext.Outbox.AnyAsync();
            await Task.Delay(10);
        }
        while (isThereAnyEventsToHandle);
    }
}
