namespace GameStore.Infrastructure.Data;

public static class GenreSeedData
{
    private const string Races = "Races";
    private const string Strategy = "Strategy";
    private const string Sports = "Sports";
    private const string Action = "Action";

    public static readonly List<(string Name, string? Parent)> PredefinedGenres =
    [
        (Strategy, null),
        ("RTS", Strategy),
        ("TBS", Strategy),
        ("RPG", null),
        (Sports, null),
        (Races, Sports),
        ("Rally", Races),
        ("Arcade", Races),
        ("Formula", Races),
        ("Off-road", Races),
        (Action, null),
        ("FPS", Action),
        ("TPS", Action),
        ("Adventure", null),
        ("Puzzle & Skill", null)
    ];
}