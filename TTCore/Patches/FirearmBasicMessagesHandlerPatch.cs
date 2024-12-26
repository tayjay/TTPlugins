using Exiled.API.Features;
using Exiled.API.Features.Items;
using HarmonyLib;
using InventorySystem.Items.Firearms.BasicMessages;
using Mirror;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;

namespace TTCore.Patches;

/*[HarmonyPatch(typeof(FirearmBasicMessagesHandler),"ServerRequestReceived")]
public class FirearmBasicMessagesHandlerPatch
{
    [HarmonyPostfix]
    public static void Postfix(NetworkConnection conn, RequestMessage msg)
    {
        if (msg.Request == RequestType.Inspect)
        {
            Log.Debug("Inspecting firearm...");
            Player player = Player.Get(conn.identity);
            player.TryGetItem(msg.Serial, out Item item);
            if (item is Firearm firearm)
            {
                Custom.OnInspectFirearm(new InspectFirearmEventArgs(player, firearm));
            }
        }
    }
}*/