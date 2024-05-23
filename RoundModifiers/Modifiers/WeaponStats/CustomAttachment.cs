using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Attachments.Components;

namespace RoundModifiers.Modifiers.WeaponStats;

public class CustomAttachment : Attachment
{
    public override AttachmentName Name { get; } = AttachmentName.None;
    public override AttachmentSlot Slot { get; } = AttachmentSlot.Unassigned;
    public override float Weight { get; } = 0f;
    public override float Length { get; } = 0f;
    public override AttachmentDescriptiveAdvantages DescriptivePros { get; } = AttachmentDescriptiveAdvantages.None;
    public override AttachmentDescriptiveDownsides DescriptiveCons { get; } = AttachmentDescriptiveDownsides.None;

    public override void Initialize()
    {
        base.Initialize();
        SetParameterValue(AttachmentParam.DrawSpeedMultiplier, 0.1f);
        SetParameterValue(AttachmentParam.FireRateMultiplier, 0.1f);
    }
}