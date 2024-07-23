namespace RoundModifiers.API;

public class LuaModifiers
{
    public static string Modifiers()
    {
        return "Modifiers was successfully connected!";
    }

    public static string GetModInfo(string name)
    {
        if(RoundModifiers.Instance.TryGetModifier(name, out Modifier mod, false))
            return mod.ModInfo.Name;
        return "Modifier not found!";
    }
    
    public static bool IsModifierActive(string name)
    {
        return RoundModifiers.Instance.IsModifierActive(name);
    }
    
    public static void AddToRound(string name)
    {
        if (!RoundModifiers.Instance.TryGetModifier(name, out Modifier modifier)) return;
        RoundModifiers.Instance.RoundManager.AddRoundModifier(modifier.ModInfo);
    }
    
    public static void ClearRound()
    {
        RoundModifiers.Instance.RoundManager.ClearRoundModifiers();
    }
    
    public static void RemoveFromRound(string name)
    {
        if (!RoundModifiers.Instance.TryGetModifier(name, out Modifier modifier)) return;
        RoundModifiers.Instance.RoundManager.RemoveRoundModifier(modifier.ModInfo);
    }
    
    public static void AddToNextRound(string name)
    {
        if (!RoundModifiers.Instance.TryGetModifier(name, out Modifier modifier)) return;
        RoundModifiers.Instance.RoundManager.AddNextRoundModifier(modifier.ModInfo);
    }
    
    public static void RemoveFromNextRound(string name)
    {
        if (!RoundModifiers.Instance.TryGetModifier(name, out Modifier modifier)) return;
        RoundModifiers.Instance.RoundManager.RemoveNextRoundModifier(modifier.ModInfo);
    }
    
    public static void ClearNextRound()
    {
        RoundModifiers.Instance.RoundManager.ClearNextRoundModifiers();
    }
}