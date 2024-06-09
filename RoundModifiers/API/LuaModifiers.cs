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
}