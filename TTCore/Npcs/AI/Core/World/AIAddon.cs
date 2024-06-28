using Exiled.API.Features;
using UnityEngine;

namespace TTCore.Npcs.AI.Core.World;

public class AIAddon : MonoBehaviour
{
    public AIPlayer Core;

    public ReferenceHub ReferenceHub => Core.ReferenceHub;

    public Player Player => Core.Player;

    private void Awake()
    {
        Core = GetComponent<AIPlayer>();
        Init();
    }

    public virtual void Init() { }
}