namespace TTAdmin.Data;

public class CommandData
{
    public string Sender { get; set; }
    public string Command { get; set; }
    public string[] Arguments { get; set; }
    public bool Result { get; set; }
    public string Response { get; set; }
    
    public CommandData(string sender, string command, string[] arguments, bool result, string response)
    {
        Sender = sender;
        Command = command;
        Arguments = arguments;
        Result = result;
        Response = response;
    }
    
    public enum Type
    {
        PlayerGameConsole,
        RemoteAdmin,
        Console
    }
}