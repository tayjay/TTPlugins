using TTAdmin.Scripting.Custom.Parser;

namespace TTAdmin.Scripting.Custom.Lexer
{
    public class IfStatementNode : ASTNode
    {
        public ASTNode Condition { get; set; }  // ASTNode representing the condition expression
        public ASTNode ThenBlock { get; set; }  // ASTNode representing the code to execute if true
        public ASTNode ElseBlock { get; set; }  // (Optional) ASTNode for the else code
    }
}