using System;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using UnityEngine;

namespace TTCore.Components;

public class AttachObjectToPlayerHandler : MonoBehaviour
{
    private Player player;
    private Transform otherTransform;
    
    private void OnEnable()
    {
        if (player.Role is FpcRole fpcRole)
        {
            fpcRole.FirstPersonController.FpcModule.Motor.OnBeforeMove += new Action<Vector3>(this.OnBeforeMove);
        }
    }
    
    private void OnDisable()
    {
        if (player.Role is FpcRole fpcRole)
        {
            fpcRole.FirstPersonController.FpcModule.Motor.OnBeforeMove -= new Action<Vector3>(this.OnBeforeMove);
        }
    }
    
    
    
    public void Init(Player player, Transform transform)
    {
        this.player = player;
        this.otherTransform = transform;
    }

    public void OnBeforeMove(Vector3 moveDir)
    {
        otherTransform.position = moveDir;
        otherTransform.rotation = player.Rotation;
    }
}