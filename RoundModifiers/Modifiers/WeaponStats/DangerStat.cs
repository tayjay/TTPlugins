using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using TTCore.Events.EventArgs;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.WeaponStats;

public class DangerStat : Stat, IInspecting
{
    public override string Name => "Danger";
    public override int Rarity => 3;
    public override string Description => "This weapon will display a warning when enemies are nearby when inspecting it.";
    public void Inspecting(InspectFirearmEventArgs ev)
    {
        string description = $"<size=75%>Name: {Name}\n" +
                             $"Description: {Description}\n";
        List<Player> Scps = Player.List.Where(p => p.Role.Side == Side.Scp).ToList();
        bool ScpNearby = false;
        foreach (Player p in ev.Player.CurrentRoom.Players)
        {
            if(ScpNearby) break;
            if (p.Role.Side != ev.Player.Role.Side)
            {
                description += $"<color=red>Enemy Contact</color>";
                ScpNearby = true;
            }
        }

        foreach (Door roomDoor in ev.Player.CurrentRoom.Doors.Where(door => door.Rooms.Count > 1))
        {
            if(ScpNearby) break;
            Room room = roomDoor.Rooms.First(room => room!=ev.Player.CurrentRoom);
            foreach (Player p in room.Players)
            {
                if(ScpNearby) break;
                if (p.Role.Side != ev.Player.Role.Side)
                {
                    description += $"<color=yellow>Enemy Nearby</color>";
                    ScpNearby = true;
                }
            }
        }
        if (!ScpNearby) description += "<color=green>No Enemies Nearby</color>";
        description += "</size>";
        ev.Player.ShowHUDHint(description, 5f);
    }
}