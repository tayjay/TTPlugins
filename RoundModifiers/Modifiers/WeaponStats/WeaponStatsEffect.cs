using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CustomPlayerEffects;
using Exiled.API.Features;
using InventorySystem.Items.Firearms.Attachments;
using PlayerRoles.FirstPersonControl;

namespace RoundModifiers.Modifiers.WeaponStats;

public class WeaponStatsEffect : StatusEffectBase, IWeaponModifierPlayerEffect, IMovementSpeedModifier, IStaminaModifier
{
    public static readonly AttachmentParameterValuePair[] WeaponModifiers = new AttachmentParameterValuePair[6]
    {
        new AttachmentParameterValuePair(AttachmentParam.OverallRecoilMultiplier, 0.075f),
        new AttachmentParameterValuePair(AttachmentParam.AdsInaccuracyMultiplier, 0.07f),
        new AttachmentParameterValuePair(AttachmentParam.ReloadSpeedMultiplier, 20.25f),
        new AttachmentParameterValuePair(AttachmentParam.DrawSpeedMultiplier, 20.2f),
        new AttachmentParameterValuePair(AttachmentParam.AdsSpeedMultiplier, 20.2f),
        new AttachmentParameterValuePair(AttachmentParam.FireRateMultiplier, 20.2f)
    };
    
    public Dictionary<AttachmentParam, float> _processedParams = new Dictionary<AttachmentParam, float>();

    public override void IntensityChanged(byte prevState, byte newState)
    {
        base.IntensityChanged(prevState, newState);
        foreach (AttachmentParameterValuePair weaponModifier in Scp1853.WeaponModifiers)
            this._processedParams[weaponModifier.Parameter] = weaponModifier.Value;
        Log.Debug("New intensity: " + newState);
    }


    public bool TryGetWeaponParam(AttachmentParam param, [UnscopedRef] out float val)
    {
        //this.Hub
        
        bool worked = this._processedParams.TryGetValue(param, out val);
        Log.Debug("TryGetWeaponParam "+param + " " + val + " " + worked);
        return worked;
    }

    public bool ParamsActive => this.IsEnabled;
    public bool MovementModifierActive => this.IsEnabled;
    public float MovementSpeedMultiplier => 1.2f;
    public float MovementSpeedLimit => float.MaxValue;
    public bool StaminaModifierActive => this.IsEnabled;
    public float StaminaUsageMultiplier => 0f;
    public float StaminaRegenMultiplier => 1f;
    public bool SprintingDisabled => this.IsEnabled;
    
    
    
    
    public float ProcessValue(float baseValue, float multiplier)
    {
        return (float) (1.0 + ((double) baseValue - 1.0) * (double) multiplier);
    }
}