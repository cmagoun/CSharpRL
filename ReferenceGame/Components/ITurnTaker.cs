using ReferenceGame.Systems;
using SadSharp.Modes.Map;

namespace ReferenceGame.Components
{
    public enum WhoControls { Player, Computer }
    public interface ITurnTaker
    {
        WhoControls Who { get; }
        MoveResult TakeTurn(string entityId, MapMode mm);
    }
}
