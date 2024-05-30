using TTCore.API;

namespace RoundModifiers.API
{
    public abstract class Modifier :IRegistered
    {
        
        protected abstract void RegisterModifier();
        
        protected abstract void UnregisterModifier();


        public void Register()
        {
            IsEnabled = true;
            RegisterModifier();
        }

        public void Unregister()
        {
            if(!IsEnabled) return;
            IsEnabled = false;
            UnregisterModifier();
        }
        
        public bool IsEnabled { get; private set; }

        public abstract ModInfo ModInfo { get; }
    }
    public enum ImpactLevel
    {
        None,
        Graphics,
        MinorGameplay,
        MajorGameplay,
        Gamemode,
    }

    public enum Category
    {
        None,
        Visual,
        ScpRole,
        HumanRole,
        Gamemode,
        Scale,
        Health,
        Weapon,
        NewRole,
        Facility,
        Lights,
        Names,
        Voice,
    }
}