using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Player;
using MoonSharp.Interpreter;
using SCriPt.API.Lua;

namespace SCriPt.Handlers
{
    public class EventManager
    {
        public static Dictionary<string,List<Closure>> RegisteredEvents = new Dictionary<string, List<Closure>>();
        
        
        public static void RegisterEvent(string name, Closure func)
        {
            Log.Debug("Registering event: "+name);
            if (!RegisteredEvents.ContainsKey(name))
            {
                RegisteredEvents.Add(name, new List<Closure>());
            }
            RegisteredEvents[name].Add(func);
            Log.Debug("Registered event: "+name);
        }

        public static void CallEvent(string name)
        {
            if(!RegisteredEvents.ContainsKey(name)) return;
            RegisteredEvents[name]?.ForEach(closure => ScriptLoader.AutoLoadScript.Call(closure));
        }
        
        public static void CallEvent<T>(string name, T args) where T : IExiledEvent
        {
            if(!RegisteredEvents.ContainsKey(name)) return;
            Log.Debug("Calling event: "+name+" with args: "+args?.GetType().Name);
            //RegisteredEvents[name]?.ForEach(closure => closure.Call(args));
            RegisteredEvents[name]?.ForEach(closure => ScriptLoader.AutoLoadScript.Call(closure,args));
            Log.Debug("Event args were "+args);
        }
        
        
        
    }
}