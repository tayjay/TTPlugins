using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class HealthBars : Modifier
{
    private List<HPBar> _hpBars { get; set; }

    public void OnRoundStart()
    {
        
    }
    
    /*public IEnumerator<float> OnUpdate()
    {
        foreach (HPBar hpBar in _hpBars)
        {
            hpBar.UpdatePositionAndRotation();
            hpBar.UpdateHealth();
        }
        yield return 0.5f;
    }*/

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if(!ev.Player.ReferenceHub.gameObject.TryGetComponent<HPBar>(out var _))
        {
            HPBar hpBar = ev.Player.ReferenceHub.gameObject.AddComponent<HPBar>();
            hpBar.Init(ev.Player);
            _hpBars.Add(hpBar);
        }
    }

    public void OnDied(DiedEventArgs ev)
    {
        
            
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        _hpBars = ListPool<HPBar>.Pool.Get();
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        ListPool<HPBar>.Pool.Return(_hpBars);
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "HealthBars",
        FormattedName = "<color=red><size=85%>Health Bars</size></color>",
        Aliases = new[] { "hb", "hp" },
        Description = "Adds health bars to players",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
        Balance = -1,
        Category = Category.Visual
    };

    public class HPBar : MonoBehaviour
    {
        public Player Player { get; set; }
        public float MaxHealth => Player.MaxHealth;
        public float Health => Player.Health;
        public float Hume => Player.HumeShieldStat.CurValue;

        public Primitive FullBar, HealthBar;
        
        public HPBar()
        {
            
        }

        public void Init(Player player)
        {
            Player = player;
            /*PrimitiveSettings settings = new PrimitiveSettings(PrimitiveType.Cylinder, Color.gray,player.Position+(Vector3.up*1), player.Rotation.eulerAngles, new Vector3(-0.2f, -0.05f, -0.05f), true);
            FullBar = Primitive.Create(settings);*/
            PrimitiveSettings settings2 = new PrimitiveSettings(PrimitiveType.Cube, Color.red,player.Position+(Vector3.up*RoundModifiers.Instance.Config.HealthBars_Height), player.Rotation.eulerAngles, Vector3.one, true);
            HealthBar = Primitive.Create(settings2);
            
            //FullBar.Base.transform.SetParent(player.GameObject.transform);
            HealthBar.Base.transform.SetParent(player.GameObject.transform);
            Log.Debug("Initialized HPBar");
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            //UpdatePositionAndRotation();
            //Log.Info("Updated HPBar Pos "+Player.Position+" "+FullBar.Position);
        }
        
        protected float lastUpdate = 0;

        private void FixedUpdate()
        {
            if (Player.IsDead) return;
            if (Time.time - lastUpdate < 0.5f) return;
            UpdateHealth();
            //Log.Info("FixedUpdate HPBar");
        }

        /*public void UpdatePositionAndRotation()
        {
            if (Player.IsAlive)
            {
                FullBar.Position = Player.Position + (Vector3.up * 2);
                FullBar.Rotation = Player.Rotation;
                HealthBar.Position = Player.Position + (Vector3.up * 2);
                HealthBar.Rotation = Player.Rotation;
            }
            else
            {
                FullBar.Position = Vector3.zero;
                FullBar.Rotation = Quaternion.identity;
                HealthBar.Position = Vector3.zero;
                HealthBar.Rotation = Quaternion.identity;
            }
            
        }*/

        /*private void OnAnimatorMove()
        {
            UpdatePositionAndRotation();
            Log.Info("Animator moved");
        }*/

        public void UpdateHealth()
        {
            //Log.Debug($"{Player.Nickname} HP: {Health}/{MaxHealth}");
            HealthBar.Scale = new Vector3((Health / MaxHealth)*-1*RoundModifiers.Instance.Config.HealthBars_Length, -0.05f, -0.05f);
            lastUpdate = Time.time;
        }
    }
}