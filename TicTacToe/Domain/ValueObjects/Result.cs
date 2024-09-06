namespace Domain.ValueObjects;

internal abstract record Result;
internal abstract record Completed : Result;

internal record NoWinnerYet : Result;
internal record WonBy(Player Player) : Completed;
internal record Draw : Completed;
