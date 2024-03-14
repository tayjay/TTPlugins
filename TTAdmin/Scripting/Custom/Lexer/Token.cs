namespace TTAdmin.Scripting.Custom.Lexer
{
    public class Token 
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        // You could add properties like line number, column for error reporting
    }
}