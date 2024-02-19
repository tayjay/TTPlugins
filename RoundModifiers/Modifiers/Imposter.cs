using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using Respawning;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class Imposter : Modifier
    {
        private RoleTypeId[] StartingRoles = new [] { RoleTypeId.ClassD , RoleTypeId.Scientist, RoleTypeId.FacilityGuard };
        private Dictionary<Player, ImposterInfo> Imposters = new Dictionary<Player, ImposterInfo>();
        
        CoroutineHandle _coroutine;
        
        
        public void OnRoleChanged(ChangingRoleEventArgs ev)
        {
            if (RoleExtensions.GetTeam(ev.NewRole) == Team.SCPs && ev.NewRole!=RoleTypeId.Scp079/* && ev.NewRole != RoleTypeId.Scp3114*/)
            {
                Imposters[ev.Player] = new ImposterInfo(ev.Player, StartingRoles.GetRandomValue());
                
                //ev.NewRole = RoleTypeId.Scp3114;
                Timing.CallDelayed(0.6f, () =>
                {
                    ev.Player.ChangeAppearance(StartingRoles.GetRandomValue());
                    Log.Info("Changing " + ev.Player.Nickname + " appearance.");
                });
            }
            else
            {
                if(Imposters.ContainsKey(ev.Player))
                    Imposters.Remove(ev.Player);
            }
        }

        public void OnRoundStart()
        {
            _coroutine = MEC.Timing.RunCoroutine(ImposterTick());
        }

        
        
        public IEnumerator<float> ImposterTick()
        {
            while (true)
            {
                foreach(ImposterInfo info in Imposters.Values)
                {
                    info.Update();
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }


        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            foreach(ImposterInfo info in Imposters.Values)
            {
                if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                {
                    Timing.CallDelayed(3f, () =>
                    {
                        info.disguise=RoleTypeId.NtfPrivate;
                        info.Hide();
                    });
                }
                else if(ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
                {
                    Timing.CallDelayed(3f, () =>
                    {
                        info.disguise = RoleTypeId.ChaosConscript;
                        info.Hide();
                    });
                }
            }
        }

        private void Hide(Player player)
        {
            Imposters[player].Hide();
        }
        
        private void Reveal(Player player, int time=10)
        {
            Imposters[player].Reveal(time);
        }



        public void OnPlayerAttack(HurtEventArgs ev)
        {
            if(ev.Attacker==null)
                return;
            if(ev.Player==null)
                return;
            if(ev.Attacker.Role.Team==Team.SCPs && ev.Attacker.Role.Type!=RoleTypeId.Scp079)
            {
                Reveal(ev.Attacker);
            }
            if (ev.Player.Role.Team == Team.SCPs && ev.Player.Role.Type != RoleTypeId.Scp079)
            {
                Reveal(ev.Player,30);
                /*if (Imposters[ev.Player.Id].IsHidden)
                {
                    //increase the damage threshold check
                }
                else
                {
                    //Already revealed, re-up the timer
                    Reveal(ev.Player,30);
                }*/
            }
        }
        
        
        public void OnSCP096Enrage(Exiled.Events.EventArgs.Scp096.EnragingEventArgs ev)
        {
            if (ev.IsAllowed)
            {
                Reveal(ev.Player, 120);
            }
        }
        
        public void OnScp096Calm(Exiled.Events.EventArgs.Scp096.CalmingDownEventArgs ev)
        {
            if (ev.IsAllowed)
            {
                Hide(ev.Player);
            }
        }
        
        public void OnScp173Blink(Exiled.Events.EventArgs.Scp173.BlinkingEventArgs ev)
        {
            if (ev.IsAllowed)
            {
                Reveal(ev.Player);
            }
        }
        
        
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            
            Exiled.Events.Handlers.Player.ChangingRole += OnRoleChanged;
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            
            Exiled.Events.Handlers.Player.Hurt += OnPlayerAttack;
            Exiled.Events.Handlers.Scp096.Enraging += OnSCP096Enrage;
            Exiled.Events.Handlers.Scp096.CalmingDown += OnScp096Calm;
            Exiled.Events.Handlers.Scp173.Blinking += OnScp173Blink;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            
            Exiled.Events.Handlers.Player.ChangingRole -= OnRoleChanged;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
            
            Exiled.Events.Handlers.Player.Hurt -= OnPlayerAttack;
            Exiled.Events.Handlers.Scp096.Enraging -= OnSCP096Enrage;
            Exiled.Events.Handlers.Scp096.CalmingDown -= OnScp096Calm;
            Exiled.Events.Handlers.Scp173.Blinking -= OnScp173Blink;
            
            MEC.Timing.KillCoroutines(_coroutine);
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "Imposter",
            Aliases = new []{"imposter"},
            Description = "SCPs are disguised as humans.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false
        };
        
        
        
        
        
        
        public class ImposterInfo
        {
            public Player Player;
            public bool IsHidden;
            public int revealTimer;
            public RoleTypeId disguise;
            public float DamageTimer;
            
            public ImposterInfo(Player player, RoleTypeId disguise)
            {
                Player = player;
                IsHidden = true;
                revealTimer = 0;
                this.disguise = disguise;
                DamageTimer = 0;
            }
            
            public void Reveal(int time=30)
            {
                if (IsHidden)
                {
                    //Not revealed, so reveal them
                    Player.ShowHint("You have taken a hostile action and have been revealed");
                    Player.ChangeAppearance(Player.Role.Type);
                }
                IsHidden = false;
                revealTimer = time;
                
            }
            
            public void Hide()
            {
                IsHidden = true;
                revealTimer = 0;
                Player.ShowHint("You have been disguised as a " + disguise.ToString());
                Player.ChangeAppearance(disguise, playersToAffect:Player.List.Where(p=>p.Role.Team!=Team.SCPs));
            }
            
            public void Update()
            {
                
                if (revealTimer > 0)
                {
                    revealTimer--;
                    if (revealTimer == 0)
                    {
                        Hide();
                    }
                }
            }
        }
    }
}