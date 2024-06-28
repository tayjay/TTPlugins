using System.Collections.Generic;
using Exiled.API.Features;

namespace TTAdmin.Data;

public class InventoryData
{
    public Dictionary<string,int> Ammo { get; set; }
    public List<string> Items { get; set; }
    public string CurrentItem { get; set; }

    public InventoryData(Player player)
    {
        //Log.Debug("Creating Inventory data");
        Ammo = new Dictionary<string, int>();
        Items = new List<string>();
        CurrentItem = "";
        //Log.Debug("Checking player data");
        if(player == null) return;
        //Log.Debug("Player is not null");
        if(player.Ammo == null) return;
        //Log.Debug("Player ammo is not null");
        if(player.Items == null) return;
        //Log.Debug("Player items is not null");
        
        foreach (var ammo in player.Ammo)
        {
            Ammo.Add(ammo.Key.ToString(), ammo.Value);
        }
        
        foreach (var item in player.Items)
        {
            Items.Add(item.Type.ToString());
        }
        CurrentItem = player.CurrentItem?.Type.ToString();
        //Log.Debug("Finished setting inventory data");
    }
}