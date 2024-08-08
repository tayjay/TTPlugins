using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using PlayerRoles;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class Pogo : Modifier
{
    CoroutineHandle _jumpLoop;
    
    public void OnRoundStart()
    {
        _jumpLoop = Timing.RunCoroutine(JumpLoop());
    }

    public IEnumerator<float> JumpLoop()
    {
        yield return Timing.WaitForSeconds(3f);
        while (true)
        {
            foreach (Player player in Player.List.Where(p=>p.IsAlive && p.Role!=RoleTypeId.Scp079))
            {
                try
                {
                    if(player.Role is FpcRole fpcRole)
                    {
                        fpcRole.FirstPersonController.FpcModule.Motor.WantsToJump = true;
                        Log.Debug("Jumping");
                    }
                } catch {}

                yield return Timing.WaitForOneFrame;
            }
            yield return Timing.WaitForSeconds(1f);
        }
    }
    
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
    }

    protected override void UnregisterModifier()
    {
        Timing.KillCoroutines(_jumpLoop);
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Pogo",
        FormattedName = "<color=yellow>Pogo</color>",
        Description = "Always jumping!",
        Aliases = new[] { "jumping" },
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
        Balance = -1,
    };
}