using Exiled.API.Features;
using MapGeneration;
using TTCore.API;

namespace TTCore.Events.Handlers
{
    public class MapEvents : IRegistered
    {
        public void OnMapGenerated()
        {
            Log.Info("Map generated!");
        }


        public void Register()
        {
            SeedSynchronizer.OnMapGenerated += OnMapGenerated;
        }

        public void Unregister()
        {
            SeedSynchronizer.OnMapGenerated -= OnMapGenerated;
        }
    }
}