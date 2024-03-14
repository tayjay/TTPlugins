using System.Collections.Generic;
using TTAdmin.Scripting.Custom.Parser;

namespace TTAdmin.Scripting.Custom.Lexer
{
    public class ActionNode : ASTNode
    {
        public string ActionName { get; set; }   // E.g., "GIVE_ITEM", "SET_LOBBY_STATE"
        public List<ASTNode> Arguments { get; set; } 
    }
}