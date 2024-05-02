namespace TTCore.Events.EventArgs;

public class PlayerEffectsAwakeArgs
{
    public PlayerEffectsController Controller { get; }
    
    public PlayerEffectsAwakeArgs(PlayerEffectsController controller)
    {
        Controller = controller;
    }
}