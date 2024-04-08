using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.Events.Handlers;
using MoonSharp.Interpreter;
using PlayerRoles;
using RemoteAdmin;
using Player = Exiled.API.Features.Player;
using Server = Exiled.API.Features.Server;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaServer
    {
        
        public static int PlayerCount => Exiled.API.Features.Player.List.Count;
        public static int NpcCount => Exiled.API.Features.Npc.List.Count;
        public static int ScpCount => Player.List.Count(p => p.Role.Team == Team.SCPs);
        public static int HumanCount => Player.List.Count(p => p.Role.Side == Side.Mtf || p.Role.Side == Side.ChaosInsurgency);
        public static int DClassCount => Player.List.Count(p => p.Role.Team == Team.ClassD);
        public static int ScientistCount => Player.List.Count(p => p.Role.Team == Team.Scientists);
        public static int FoundationCount => Player.List.Count(p => p.Role.Team == Team.FoundationForces);
        public static int ChaosCount => Player.List.Count(p => p.Role.Team == Team.ChaosInsurgency);

        public static List<Player> Players => Player.List.ToList();
        
        public static string RACommand(string command)
        {
            return RemoteAdmin.CommandProcessor.ProcessQuery(command, new ServerConsoleSender());
        }

        public static string LACommand(string command)
        {
            return Exiled.API.Features.Server.ExecuteCommand(command);
        }

        public static void Restart()
        {
            Server.Restart();
        }
    }
}