namespace RoundModifiers.API
{
    public struct IModInfo
    {
        public string Name { get; }
        public string Description { get; }
        public string[] Aliases { get; }
        public ImpactLevel Impact { get; }
    }
}