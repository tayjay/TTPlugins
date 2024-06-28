using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;

namespace RoundModifiers.Modifiers.LevelUp.Boosts
{
    public class StatusEffectBoost : Boost
    {
        private EffectType _effectType;
        private byte _intensity;
        private bool _onRespawn;
        private bool _canGiveToScp;
        
        public StatusEffectBoost(EffectType effectType, byte intensity, bool onRespawn = true, Tier tier = Tier.Common, bool canGiveToScp=true) : base(tier)
        {
            _effectType = effectType;
            _intensity = intensity;
            _onRespawn = onRespawn;
            _canGiveToScp = canGiveToScp;
        }
        
        public bool SafeToGive(Player player)
        {
            switch (_effectType)
            {
                case EffectType.Scp207:
                    return !player.IsEffectActive<AntiScp207>() && !player.IsEffectActive<Scp1853>() && !player.IsEffectActive<Scp207>();
                case EffectType.AntiScp207:
                    return !player.IsEffectActive<Scp207>() && !player.IsEffectActive<Scp1853>() && !player.IsEffectActive<AntiScp207>() && !player.IsEffectActive<DamageReduction>();
                case EffectType.Scp1853:
                    return !player.IsEffectActive<Scp207>() && !player.IsEffectActive<AntiScp207>() && !player.IsEffectActive<Scp1853>();
                case EffectType.DamageReduction:
                    return !player.IsEffectActive<AntiScp207>() && !player.IsEffectActive<DamageReduction>();
                
            }
            return true;
        }

        public override bool AssignBoost(Player player)
        {
            if(player.TryGetEffect(_effectType, out StatusEffectBase effect))
            {
                if(effect.Intensity > 0) return false;
            }
            if(!_canGiveToScp && player.Role.Team == Team.SCPs) return false;
            
            return ApplyBoost(player);
        }

        public override bool ApplyBoost(Player player)
        {
            if (!SafeToGive(player)) return false;
            byte intensity = _intensity;
            if (player.TryGetEffect(_effectType, out StatusEffectBase effect))
            {
                intensity += effect.Intensity; //Add stacking of status effects
            }
            player.EnableEffect(_effectType, intensity, 0);
            Log.Info("Giving "+player.Nickname+" "+_effectType.ToString()+" status effect.");
            if(_onRespawn)
                HasBoost[player.NetId] = true;
            return true;
        }

        public override string GetName()
        {
            return "Status Effect: "+_effectType.ToString();
        }
        
        public override string GetDescription()
        {
            return "Granted the player "+_effectType.ToString()+" status effect.";
        }
        

        
    }
}