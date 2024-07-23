using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using MapGeneration;
using RelativePositioning;
using UnityEngine;

namespace TTAdmin.Data;

public class PlayerData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Ip { get; set; }
    public uint NetId { get; set; }
    public string SteamId { get; set; }
    public string Role { get; set; }
    public List<string> CustomRoles { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public RelativePosition RelativePosition { get; set; }
    public Vector3 Scale { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float Hume { get; set; }
    public float MaxHume { get; set; }
    public string Room { get; set; }
    /*public Dictionary<string,int> Ammo { get; set; }
    public List<string> Inventory { get; set; }*/
    public InventoryData Inventory { get; set; }
    public string CurrentItem { get; set; }
    public List<EffectData> Effects { get; set; }
    public bool IsNpc { get; set; }
    public bool GodMode { get; set; }
    public bool CanNoClip { get; set; }
    public bool KeycardBypass { get; set; }
    public string DisplayNickname { get; set; }
    
    public Dictionary<string, object> SessionVariables { get; set; }

    public PlayerData(Player player)
    {
        Id = player.Id;
        NetId = player.NetId;
        Name = player.Nickname;
        Ip = player.IPAddress;
        SteamId = player.UserId;
        Role = player.Role.Type.ToString();
        CustomRoles = new List<string>();
        foreach (var customRole in player.GetCustomRoles())
        {
            CustomRoles.Add(customRole.Name);
        }
        Position = player.Position;
        Rotation = player.Rotation;
        RelativePosition = player.RelativePosition;
        Scale = player.Scale;
        Health = player.Health;
        MaxHealth = player.MaxHealth;
        Hume = player.HumeShield;
        MaxHume = player.HumeShieldStat.MaxValue;
        if (player.GameObject == null || player.CurrentRoom == null)
        {
            Room = null;
        }
        else
        {
            Room = player.CurrentRoom.Name;
        }
        Inventory = new InventoryData(player);        
        CurrentItem = player.CurrentItem?.Type.ToString();
        Effects = EffectData.FromEffects(player.ReferenceHub.playerEffectsController.AllEffects);
        IsNpc = player.IsNPC;
        GodMode = player.IsGodModeEnabled;
        CanNoClip = player.IsNoclipPermitted;
        KeycardBypass = player.IsBypassModeEnabled;
        DisplayNickname = player.DisplayNickname;
        SessionVariables = player.SessionVariables;
    }
    
    public static List<PlayerData> ConvertList(IEnumerable<Player> players)
    {
        List<PlayerData> playerData = new List<PlayerData>();
        foreach (var player in players)
        {
            playerData.Add(new PlayerData(player));
        }
        return playerData;
    }
}