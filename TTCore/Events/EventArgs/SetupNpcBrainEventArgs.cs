using Exiled.API.Features;
using TTCore.Npcs.AI;
using TTCore.Npcs.AI.Behaviours;

namespace TTCore.Events.EventArgs;

public class SetupNpcBrainEventArgs
{
    public SetupNpcBrainEventArgs(Npc npc)
    {
        this.Npc = npc;
        this.Brain = npc.GameObject.GetComponent<Brain>();
    }
    
    public Npc Npc { get; }
    public Brain Brain { get; set; }
}