using System.ComponentModel;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers.LevelUp;

public class LevelUpConfig : ModConfig
{
    [Description("The base amount of XP needed to level up. Default is 75")]
    public float BaseXP { get; set; } = 75;
    [Description("The amount of XP needed to level up per level. Default is 25")]
    public float XPPerLevel { get; set; } = 25;
    
}