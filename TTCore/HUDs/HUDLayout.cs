using System.Collections.Generic;

namespace TTCore.HUDs;

public class HUDLayout
{
    public float RefreshRate { get; set; }
    public Dictionary<string,object> Data { get; set; }
        
    public HUD.HUDHint DisplayingHint => OwnerHUD.DisplayingHint;
    public HUD OwnerHUD { get; set; }
        
    public HUDLayout(HUD hud = null, float refreshRate = 1f)
    {
        OwnerHUD = hud;
        RefreshRate = refreshRate;
    }

    public void SetHUD(HUD gameHUD)
    {
        OwnerHUD = gameHUD;
    }

    public virtual string BuildHUD()
    {
        return DisplayingHint?.Text;
    }
    
    public virtual bool ShouldDisplay(HUD hud)
    {
        return true;
    }

    public class StylizedString
    {
            
        public string Text { get; private set; }
            
        public StylizedString(string text)
        {
            Text = text;
        }
            
        public StylizedString Bold()
        {
            Text = "<b>" + Text + "</b>";
            return this;
        }
            
        public StylizedString Italic()
        {
            Text = "<i>" + Text + "</i>";
            return this;
        }
            
        public StylizedString Size(int size)
        {
            Text = "<size=" + size + ">" + Text + "</size>";
            return this;
        }
            
        public StylizedString Color(string color)
        {
            Text = "<color=" + color + ">" + Text + "</color>";
            return this;
        }
            
        public static implicit operator string(StylizedString s)
        {
            return s.Text;
        }
    }
}