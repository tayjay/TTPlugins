namespace RoundModifiers.API
{
    public interface IModifier
    {
        void Register();
        
        void Unregister();
        
        string Name { get; set; }
        
        string Description { get; set; }
        
        ImpactLevel Impact { get; set; }
        
    }
    public enum ImpactLevel
    {
        None,
        Graphics,
        MinorGameplay,
        MajorGameplay,
        Gamemode,

    }
}