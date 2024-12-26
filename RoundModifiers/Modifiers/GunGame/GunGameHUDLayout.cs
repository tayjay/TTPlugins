using TTCore.HUDs;

namespace RoundModifiers.Modifiers.GunGame;

/*public class GunGameHUDLayout : HUDLayout
{
    // Needed elements
    // Current place, current kills, player name, current weapon, next weapon,
    // Lead player(if not this player), next player (If leader is this player)
    public int Place { get; set; }
    public int Kills { get; set; }
    public string Name { get; set; }
    public string OtherName { get; set; }
    public int OtherPlace { get; set; }
    public int OtherKills { get; set; }
    public string CurrentWeapon { get; set; }
    public string NextWeapon { get; set; }
    
    public GunGameHUDLayout(HUD hud, float refreshRate = 1) : base(hud, refreshRate)
    {
        Place = 0;
        Kills = 0;
        Name = "";
        OtherName = "";
        OtherPlace = 0;
        OtherKills = 0;
        CurrentWeapon = "";
        NextWeapon = "";
    }
    
    public override bool ShouldDisplay()
    {
        return OwnerHUD.Owner.IsAlive;
    }

    public override string BuildHUD()
    {
        string firstLine = "";
        string secondLine = "";
        if (Place == 1)
        {
            firstLine = $"1. {Name} - Kills: {Kills}";
            secondLine = $"{OtherPlace}. {OtherName} - Kills: {OtherKills}";
        }
        else
        {
            firstLine = $"1. {OtherName} - Kills: {OtherKills}";
            secondLine = $"{Place}. {Name} - Kills: {Kills}";
        }
        
        
        string hud = "<color=orange><size=75%>";
        hud+=firstLine;
        hud += "\n"+secondLine;
        hud += "</size>\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" +
               $"<size=75%>{(DisplayingHint == null ? " " : DisplayingHint.Text)}</size>\n" +
               "\n<size=65%>" +
               $"<align=left>Current Weapon: {CurrentWeapon}</align>\n";
        if (GunGame.GunGameConfig.Sequential)
        {
            hud += $"\n<align=right>NextWeapon: {NextWeapon}</align>";
        }
        hud+= "\n\n\n\n\n\n\n\n\n\n\n\n\n</size></color>";
        
        return hud;

    }
}*/