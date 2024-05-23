using Exiled.API.Features;
using HarmonyLib;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;

namespace TTCore.Patches;

[HarmonyPatch(typeof(AttachmentsUtils), "AttachmentsValue")]
public class AttachmentUtilsPatch
{
    [HarmonyPostfix]
    public static void Postfix(Firearm firearm, AttachmentParam param, ref float __result)
    {
        if (param == AttachmentParam.FireRateMultiplier)
        {
            __result = 2f;
            
        } else if(param == AttachmentParam.DrawSpeedMultiplier)
        {
            __result = 2f;
        } else if (param == AttachmentParam.DamageMultiplier)
        {
            __result *= 1000f;
        }
    }
}