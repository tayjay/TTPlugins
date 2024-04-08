using System;
using System.Reflection;
using PlayerRoles.FirstPersonControl;
using PluginAPI.Core;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class MoonGravity : Modifier
{
    
    public void OnWaitingForPlayers()
    {
        
    }
    
    protected override void RegisterModifier()
    {
        try
        {
            var Gravity = typeof(FpcMotor).GetField("Gravity", BindingFlags.Static | BindingFlags.NonPublic);
            Gravity?.SetValue(null, new Vector3(0,-10f, 0));
            Log.Info("Set gravity to "+FpcMotor.Gravity.y);
        } catch (Exception e)
        {
            Log.Error($"Error setting gravity: {e}");
        }
    }

    protected override void UnregisterModifier()
    {
        //new Vector3(0.0f, -19.6f, 0.0f);
        try
        {
            var Gravity = typeof(FpcMotor).GetField("Gravity", BindingFlags.Static | BindingFlags.NonPublic);
            Gravity?.SetValue(null, new Vector3(0,-19.6f, 0));
            Log.Info("Set gravity to regular gravity");
        } catch (Exception e)
        {
            Log.Error($"Error setting gravity: {e}");
        }
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "MoonGravity",
        Aliases = new[] { "moon_gravity" },
        FormattedName = "Moon Gravity",
        Description = "Changes the gravity to be like the moon",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false
    };

}