using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class Pills : Modifier
{
    private Dictionary<uint, int> pillCount = new Dictionary<uint, int>();
    
    public void OnRoundStart()
    {
        pillCount.Clear();
    }

    public void OnTakePills(UsedItemEventArgs ev)
    {
        if(ev.Item.Type != ItemType.Painkillers) return;
        if(pillCount.ContainsKey(ev.Player.NetId))
        {
            pillCount[ev.Player.NetId]++;
        }
        else
        {
            pillCount[ev.Player.NetId] = 1;
        }
        
    }
    
    public void GiveEffect(Player player)
    {
        if(pillCount.ContainsKey(player.NetId))
        {
            if(pillCount[player.NetId] >= 3)
            {
                //player.EnableEffect<>()
            }
        }
    }
    
    public IEnumerator<float> OnUpdate()
    {
        throw new System.NotImplementedException();
    }
    protected override void RegisterModifier()
    {
        throw new System.NotImplementedException();
    }

    protected override void UnregisterModifier()
    {
        throw new System.NotImplementedException();
    }

    public override ModInfo ModInfo { get; }
}