using Exiled.API.Features;

namespace TTAdmin.Admin;

public class TTAdminCommandSender : ServerConsoleSender
{
    public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
    {
        if(!logToConsole) return;
        this.Print(text);
    }

    public override void Print(string text)
    {
        Log.Info(text);
    }

    public override string SenderId => "TTAdmin";
    public override string Nickname => "TTAdmin";
    public override ulong Permissions => ServerStatic.PermissionsHandler.FullPerm;
    public override byte KickPower => byte.MaxValue;
    public override bool FullPermissions => true;
}