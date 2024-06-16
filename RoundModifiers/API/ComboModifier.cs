using System;
using System.Collections.Generic;
using System.Linq;

namespace RoundModifiers.API;

public abstract class ComboModifier : Modifier
{
    public List<Modifier> Modifiers { get; }
    
    public int TotalBalance => Modifiers.Sum(m => m.ModInfo.Balance);
    
    protected ComboModifier()
    {
        Modifiers = new List<Modifier>();
    }
    
    
    protected void AddModifier(Modifier modifier)
    {
        Modifiers.Add(modifier);
    }
    
    protected void AddModifier(string modName)
    {
        if(RoundModifiers.Instance.TryGetModifier(modName, out Modifier mod))
            Modifiers.Add(mod);
    }
    
    protected void AddModifier<T>() where T : Modifier
    {
        if(RoundModifiers.Instance.GetModifier<T>() is { } mod)
            Modifiers.Add(mod);
    }
    
    protected void RemoveModifier(Modifier modifier)
    {
        Modifiers.Remove(modifier);
    }
    
    protected void RemoveModifier(string modName)
    {
        if(RoundModifiers.Instance.TryGetModifier(modName, out Modifier mod))
            Modifiers.Remove(mod);
    }
    
    protected void RemoveModifier<T>() where T : Modifier
    {
        if(RoundModifiers.Instance.GetModifier<T>() is { } mod)
            Modifiers.Remove(mod);
    }
    
    protected override void RegisterModifier()
    {
        foreach(Modifier modifier in Modifiers)
        {
            modifier.Register();
        }
    }

    protected override void UnregisterModifier()
    {
        foreach(Modifier modifier in Modifiers)
        {
            modifier.Unregister();
        }
    }

}