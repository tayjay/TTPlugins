using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;
using TTCore.Utilities;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class Debug : Modifier
{

    public void OnShot(ShotEventArgs ev)
    {
        //DEBUG CODE
        Room room = Room.Get(ev.Position);
        Vector3 relativePosition = TransformUtils.CalculateRelativePosition(ev.Position, room.Position, room.Rotation);
        //new RoomPosition(RoomType.EzIntercom, new Vector3(-0.09283455f, -5.81958f, -0.5506745f))
        Log.Info($"Relative shot position: new RoomPosition(RoomType.{room.Type},new Vector3({relativePosition.x}f, {relativePosition.y}f, {relativePosition.z}f));");
        //END DEBUG CODE
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Shot += OnShot;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Shot -= OnShot;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Debug",
        Hidden = true
    };
}