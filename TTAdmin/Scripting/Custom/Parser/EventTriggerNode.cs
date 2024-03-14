namespace TTAdmin.Scripting.Custom.Parser
{
    public class EventTriggerNode : ASTNode 
    {
        public string EventName { get; set; }
        public ASTNode CodeBlock { get; set; }
    }
}