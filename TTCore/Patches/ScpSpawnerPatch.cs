using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Exiled.API.Features.Pools;
using HarmonyLib;
using PlayerRoles;
using PlayerRoles.RoleAssign;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;

namespace TTCore.Patches;

[HarmonyPatch(typeof(ScpSpawner),"SpawnScps")]
public class ScpSpawnerPatch
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        //Inject after line 107 to collect chosen SCPs and who is being selected for them
        //Doing so here will not allow choosing which player takes which SCP, that is down to their own preference settings
        List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);
        /*int insertionIndex = newInstructions.FindIndex((Predicate<CodeInstruction>) (instruction => instruction.opcode == OpCodes.Callvirt && instruction.operand.ToString().Contains("Spawn")));
        for (int i = 0; i < newInstructions.Count-1; i++)
        {
            if (newInstructions[i].opcode == OpCodes.Call && newInstructions[i].operand.ToString().Contains("UnityEngine.Random::Range"))
            {
                insertionIndex = i + 1;
                break;
            }
        }
        
        List<RoleTypeId> scpQueue = new List<RoleTypeId>();
        List<ReferenceHub> selectedPlayers = new List<ReferenceHub>();
        
        Custom.OnChooseScpSpawnQueue(new ChooseScpSpawnQueueEventArgs(scpQueue, selectedPlayers));*/
        return newInstructions;
    }
}