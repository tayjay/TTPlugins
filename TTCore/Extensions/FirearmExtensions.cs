using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.Firearms.BasicMessages;

namespace TTCore.Extensions
{
    public static class FirearmExtensions
    {
        /*public static bool Shoot(this Firearm firearm, Player shooter, Player target)
        {
            ShotMessage msg = new ShotMessage();
            msg.ServerCreate(shooter, target);
            
            if(!firearm.Base.ActionModule.ServerAuthorizeShot())
                return false;
            firearm.Base.HitregModule.ServerProcessShot(msg);
            firearm.Base.OnWeaponShot();
            firearm.Base.ActionModule.DoClientsideAction(true);
            return true;
            /*firearm.Base.ActionModule.ServerAuthorizeShot();
            #1#
        }

        public static void ServerCreate(this ShotMessage msg, Player shooter, Player target)
        {
            msg.TargetNetId = target.NetId;
            msg.TargetPosition = target.RelativePosition;
            msg.TargetRotation = target.Rotation;
            msg.ShooterWeaponSerial = shooter.CurrentItem.Base.ItemSerial;
            msg.ShooterPosition = shooter.RelativePosition;
            msg.ShooterCameraRotation = shooter.Rotation;
        }*/
    }
}