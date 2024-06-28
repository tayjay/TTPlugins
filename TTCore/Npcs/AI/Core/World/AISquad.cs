using System.Collections.Generic;
using Exiled.API.Features;
using TTCore.Npcs.AI.Core.Management;

namespace TTCore.Npcs.AI.Core.World;

public class AISquad(Player leader, int limit = 5)
{
    public readonly Player Leader = leader;

    public readonly List<AIPlayerProfile> Members = [];

    public int Limit { get; private set; } = limit;

    public void SetLimit(int limit)
    {
        Limit = limit;
        if (Members.Count > Limit)
            Members.RemoveRange(Limit, Members.Count - Limit);
    }
}