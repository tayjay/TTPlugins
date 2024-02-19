using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class DealDamageXP : XP, IHurtEvent
    {
        public void OnHurt(HurtEventArgs ev)
        {
            if (ev.Player == null || ev.Attacker == null) return;
            if (ev.Attacker.Role.Team == Team.SCPs) return;
            if (ev.Player.Role.Team == Team.Scientists || ev.Player.Role.Team == Team.ClassD) return;
            float damage = ev.Amount;
            if(ev.Player.Role.Team == Team.SCPs)
                damage /= 4;
            
            GiveXP(ev.Attacker, damage);
        }
    }
}