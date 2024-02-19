using PlayerRoles;

namespace RoundModifiers.Modifiers.LevelUp.HUD
{
    public class LevelUpHUDLayout : TTCore.HUDs.HUDLayout
    {
        public int Level { get; set; }
        public float XP { get; set; }
        public float XPNeeded { get; set; }
        public string ActivePerks { get; set; }
        
        
        public LevelUpHUDLayout(TTCore.HUDs.HUD hud, float refreshRate = 1) : base(hud, refreshRate)
        {
            Level = 0;
            XP = 0;
            XPNeeded = 0;
            ActivePerks = "";
        }
        
        public override string BuildHUD()
        {
            /*
             * Info to display
             * - Player's level
             * - Player's XP
             * - XP needed to level up
             */
            string color;
            switch (OwnerHUD.Owner.Role.Type)
            {
                case RoleTypeId.Scientist:
                    color = "yellow";
                    break;
                case RoleTypeId.ClassD:
                    color = "orange";
                    break;
                case RoleTypeId.FacilityGuard:
                case RoleTypeId.NtfCaptain:
                case RoleTypeId.NtfPrivate:
                case RoleTypeId.NtfSergeant:
                case RoleTypeId.NtfSpecialist:
                    color = "blue";
                    break;
                case RoleTypeId.ChaosConscript:
                case RoleTypeId.ChaosRepressor:
                case RoleTypeId.ChaosRifleman:
                case RoleTypeId.ChaosMarauder:
                    color = "green";
                    break;
                default:
                    color = "white";
                    break;
            }
            return $"<color={color}><size=70%>Level: {Level}\n" +
                   $"XP: {XP}/{XPNeeded}</size>\n"+
                   "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" +
                   $"<size=75%>{(DisplayingHint == null ? " " : DisplayingHint.Text)}</size>\n" +
                   "\n<size=65%>" +
                   $"<align=right>Active Perks: \n{ActivePerks}</align>\n" +
                   "\n\n\n\n\n\n\n\n\n</size></color>";

        }
    }
}