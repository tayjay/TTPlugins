﻿using System.Collections.Generic;
using Exiled.API.Features;
using MapGeneration;
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
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
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
        Position = player.Position;
        Rotation = player.Rotation;
        Health = player.Health;
        MaxHealth = player.MaxHealth;
        Hume = player.HumeShield;
        MaxHume = player.HumeShieldStat.MaxValue;
        Room = player.CurrentRoom?.Name;
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