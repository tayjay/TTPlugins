using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace RoundModifiers.Modifiers.LevelUp.Boosts
{
    public class StatusEffectBoost : Boost
    {
        private EffectType _effectType;
        private byte _intensity;
        private bool _onRespawn;
        
        public StatusEffectBoost(EffectType effectType, byte intensity, bool onRespawn = true, Tier tier = Tier.Common) : base(tier)
        {
            _effectType = effectType;
            _intensity = intensity;
            _onRespawn = onRespawn;
        }
        
        public bool SafeToGive(Player player)
        {
            switch (_effectType)
            {
                case EffectType.Scp207:
                    return !player.IsEffectActive<AntiScp207>() && !player.IsEffectActive<Scp1853>();
                case EffectType.AntiScp207:
                    return !player.IsEffectActive<Scp207>();
                case EffectType.Scp1853:
                    return !player.IsEffectActive<Scp207>();
            }
            return true;
        }

        public override bool AssignBoost(Player player)
        {
            if(_onRespawn)
                HasBoost[player.NetId] = true;
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
            player.EnableEffect(_effectType, intensity, 1000000);
            Log.Info("Giving "+player.Nickname+" "+_effectType.ToString()+" status effect.");
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