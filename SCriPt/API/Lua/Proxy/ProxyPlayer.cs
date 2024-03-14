using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using MoonSharp.Interpreter;
using PlayerRoles;
using UnityEngine;

namespace SCriPt.API.Lua.Proxy
{
    public class ProxyPlayer
    {
        Player Player { get; }
        
        [MoonSharpHidden]
        public ProxyPlayer(Player player)
        {
            Player = player;
        }
        
        public string Nickname => Player.Nickname;
        public int Id => Player.Id;
        public float Health
        {
            get => Player.Health;
            set => Player.Health = value;
        }

        public string Role
        {
            get => Player.Role.Type.ToString();
        }
        
        public void Kill()
        {
            Player.Kill(DamageType.Custom);
        }

        public void SetRole(string role, int flag)
        {
            if(Enum.TryParse(role, out RoleTypeId roleTypeId) && Enum.IsDefined(typeof(RoleSpawnFlags),flag))
                Player.RoleManager.ServerSetRole(roleTypeId, RoleChangeReason.RemoteAdmin, (RoleSpawnFlags)flag);
        }
        
        public void SetRole(string role)
        {
            SetRole(role,-1);
        }
        
        public void ShowHint(string message, float duration)
        {
            Player.ShowHint(message, duration);
        }
        
        public void Broadcast(string message, ushort duration)
        {
            Player.Broadcast(duration, message);
        }
        
        
        public float PosX
        {
            get => Player.Position.x;
            set => Player.Position = new Vector3(value,Player.Position.y,Player.Position.z);
        }
        
        public float PosY
        {
            get => Player.Position.y;
            set => Player.Position = new Vector3(Player.Position.x,value,Player.Position.z);
        }
        
        public float PosZ
        {
            get => Player.Position.z;
            set => Player.Position = new Vector3(Player.Position.x,Player.Position.y,value);
        }
        
        
        public float ScaleX
        {
            get => Player.Scale.x;
            set => Player.Scale = new Vector3(value,Player.Scale.y,Player.Scale.z);
        }
        
        public float ScaleY
        {
            get => Player.Scale.y;
            set => Player.Scale = new Vector3(Player.Scale.x,value,Player.Scale.z);
        }
        
        public float ScaleZ
        {
            get => Player.Scale.z;
            set => Player.Scale = new Vector3(Player.Scale.x,Player.Scale.y,value);
        }

        public Item CurrentItem => Player.CurrentItem;
    }
}