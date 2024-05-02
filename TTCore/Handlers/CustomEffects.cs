using System;
using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using Mirror;
using TTCore.API;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using UnityEngine;

namespace TTCore.Handlers;

public class CustomEffects
{
    //private static Dictionary<Type,StatusEffectBase> PlayerEffects { get; set; }
    
    private static List<Type> Effects { get; set; }
    
    
    public static void RegisterEffect<T>() where T : StatusEffectBase
    {
        Effects.Add(typeof(T));
    }
    

    public static void UnregisterEffect<T>() where T : StatusEffectBase
    {
        Effects.Remove(typeof(T));
    }
    
    //To add the effect to the player it seems there this is where we should start
    /*
     * When a player object is created a "PlayerEffectController" is created and all the effects are added to it.
     * Effects are added using the Awake method in the StatusEffectBase class. It pulls the effects from the Children of the object.
     * The effects are added to a "AllEffects" array, 1 to the EffectsLength, and to a dictionary "_effectsByType". A byte is also appended to _syncEffectsIntensity.
     * If we can apply all of these when a player is created we can add the effects to the player.
     */
    

    
    private static void OnPlayerEffectsAwake(PlayerEffectsAwakeArgs ev)
    {
        List<Type> effects = ListPool<Type>.Pool.Get(Effects);
        foreach (StatusEffectBase stat in ev.Controller.effectsGameObject.GetComponentsInChildren<StatusEffectBase>())
        {
            if(effects.Contains(stat.GetType()))
            {
                effects.Remove(stat.GetType());
            }
        }
        foreach (Type effect in effects)
        {
            ev.Controller.effectsGameObject.GetComponentInChildren<SilentWalk>().gameObject.AddComponent(effect);
            Log.Debug("Added effect: " + effect.Name);
        }
        /*foreach(Component c in ev.Controller.effectsGameObject.GetComponentsInChildren<Component>())
        {
            Log.Debug(c?.GetType().Name);
        }*/

        
        ListPool<Type>.Pool.Return(effects);
    }

    public static void Register()
    {
        //PlayerEffects = DictionaryPool<Type, StatusEffectBase>.Pool.Get();
        Effects = ListPool<Type>.Pool.Get();
        Custom.PlayerEffectsAwake += OnPlayerEffectsAwake;
    }

    public static void Unregister()
    {
        Custom.PlayerEffectsAwake -= OnPlayerEffectsAwake;
        ListPool<Type>.Pool.Return(Effects);
        //DictionaryPool<Type, StatusEffectBase>.Pool.Return(PlayerEffects);
    }
}