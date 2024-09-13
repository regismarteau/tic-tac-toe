namespace Domain.Gameplay;

public abstract record Result;
public abstract record Completed : Result;

public record Undetermined : Result;
public record WonBy(Player Player) : Completed;
public record Draw : Completed;
