using Gro.EarthModel;

namespace Gro.Infection;

public sealed class GameState
{
    public GamePhase Phase { get; set; } = GamePhase.SelectingRegion;
    public Zone? StartingZone { get; set; }
}
