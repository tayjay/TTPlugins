using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;
using TTAdmin.Scripting.Custom.Parser;

namespace TTAdmin.Scripting.Custom
{
    public class ScriptLoader
    {

        public void Initialize()
        {
            //Check if the directory exists
            if (!Directory.Exists("scripts"))
            {
                //Create the directory
                Directory.CreateDirectory("scripts");
                Log.Info("Created scripts directory");
            }
            else
            {
                Log.Info(Directory.GetParent("scripts")?.FullName);
            }
        }
        
        public ASTNode LoadScriptFromFile(string filePath)
        {
            try
            {
                // 1. Read the file contents
                string scriptText = File.ReadAllText(filePath);

                // 2. Lexing
                Lexer.Lexer lexer = new Lexer.Lexer(scriptText);
                IEnumerable<Lexer.Token> tokens = lexer.Tokenize();

                // 3. Parsing
                Parser.Parser parser = new Parser.Parser(tokens);
                ASTNode rootNode = parser.Parse();

                return rootNode;
            }
            catch (IOException ex)
            {
                Log.Error($"Error reading script file: {ex.Message}");
                return null; // Or handle the error more gracefully
            }
        }
    }
}