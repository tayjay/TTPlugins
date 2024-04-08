using Exiled.API.Features;
using MoonSharp.Interpreter;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaCassie
    {
        public static bool IsSpeaking => Cassie.IsSpeaking;
        
        public static void Message(string message)
        {
            Exiled.API.Features.Cassie.Message(message);
        }
        
        public static void Message(string message, bool isHeld)
        {
            Exiled.API.Features.Cassie.Message(message, isHeld);
        }
        
        public static void Message(string message, bool isHeld, bool isNoisy)
        {
            Exiled.API.Features.Cassie.Message(message, isHeld, isNoisy);
        }
        
        public static void Message(string message, bool isHeld, bool isNoisy, bool isSubtitles)
        {
            Exiled.API.Features.Cassie.Message(message, isHeld, isNoisy, isSubtitles);
        }
        
        public static void GlitchyMessage(string message, float glitchChance, float jamChance)
        {
            Exiled.API.Features.Cassie.GlitchyMessage(message, glitchChance, jamChance);
        }
        
        public static bool IsValidMessage(string message)
        {
            return Exiled.API.Features.Cassie.IsValid(message);
        }
        
        public static bool IsValidSentence(string sentence)
        {
            return Exiled.API.Features.Cassie.IsValidSentence(sentence);
        }
        
        
    }
}