using Exiled.API.Features;
using Exiled.API.Features.Roles;

namespace TTCore.Npcs.AI.Behaviours;

public class DummyBehaviour : AIBehaviour
{
    public override void UpdateBehaviour()
    {
        
    }

    public override void FixedUpdateBehaviour()
    {
        /*Log.Debug("Tick Dummy");
        if (Owner.Role is FpcRole fpcRole)
        {
            fpcRole.FirstPersonController.FpcModule.Motor.WantsToJump = true;
            Log.Debug("Jumping!");
        }*/
            
    }

    public DummyBehaviour() : base(Priority.High)
    {
    }
}