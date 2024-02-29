using Exiled.API.Features;
using TTCore.Npcs.AI;

namespace TTCore.Extensions;

public static class NpcExtensions
{
    public static bool HasBrain(this Npc npc)
    {
        return npc.GameObject.TryGetComponent(out Brain _);
    }
    
    public static bool TryGetBrain(this Npc npc, out Brain brain)
    {
        return npc.GameObject.TryGetComponent<Brain>(out brain);
    }
}