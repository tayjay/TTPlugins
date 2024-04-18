using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MoonSharp.Interpreter;
using PlayerRoles;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaRole
    {
        public static bool IsScp(Player player)
        {
            return IsScp(player.Role.Type);
        }

        public static bool IsScp(Role role)
        {
            return IsScp(role.Type);
        }
        
        public static bool IsScp(RoleTypeId role)
        {
            return role.GetTeam() == Team.SCPs;
        }
        
        public static bool IsNtf(Player player)
        {
            return IsNtf(player.Role.Type);
        }
        
        public static bool IsNtf(Role role)
        {
            return IsNtf(role.Type);
        }
        
        public static bool IsNtf(RoleTypeId role)
        {
            return role.GetTeam() == Team.FoundationForces;
        }
        
        public static bool IsScientist(Role role)
        {
            return IsScientist(role.Type);
        }
        
        public static bool IsScientist(RoleTypeId role)
        {
            return role == RoleTypeId.Scientist;
        }
        
        public static bool IsGuard(Role role)
        {
            return IsGuard(role.Type);
        }
        
        public static bool IsGuard(RoleTypeId role)
        {
            return role == RoleTypeId.FacilityGuard;
        }
        
        public static bool IsChaos(Role role)
        {
            return IsChaos(role.Type);
        }
        
        public static bool IsChaos(RoleTypeId role)
        {
            return role.GetTeam() == Team.ChaosInsurgency;
        }
        
        public static bool IsTutorial(Role role)
        {
            return IsTutorial(role.Type);
        }
        
        public static bool IsDClass(Role role)
        {
            return role.Type == RoleTypeId.ClassD;
        }
        
        public static bool IsClassD(Role role)
        {
            return IsClassD(role.Type);
        }
        
        public static bool IsClassD(RoleTypeId role)
        {
            return role == RoleTypeId.ClassD;
        }
        
        public static bool IsTutorial(RoleTypeId role)
        {
            return role == RoleTypeId.Tutorial;
        }
        
        public static bool IsPeanut(Role role)
        {
            return Is173(role);
        }
        
        public static bool Is173(Role role)
        {
            return role.Type == RoleTypeId.Scp173;
        }
        
        public static bool IsShyGuy(Role role)
        {
            return Is096(role);
        }
        
        public static bool Is096(Role role)
        {
            return role.Type == RoleTypeId.Scp096;
        }
        
        public static bool IsLarry(Role role)
        {
            return Is106(role);
        }
        
        public static bool IsDoctor(Role role)
        {
            return Is049(role);
        }
        
        public static bool IsZombie(Role role)
        {
            return Is0492(role);
        }
        
        public static bool IsComputer(Role role)
        {
            return Is079(role);
        }
        
        public static bool IsSkeleton(Role role)
        {
            return Is3114(role);
        }
        
        public static bool Is049(Role role)
        {
            return role.Type == RoleTypeId.Scp049;
        }
        
        public static bool Is0492(Role role)
        {
            return role.Type == RoleTypeId.Scp0492;
        }
        
        public static bool Is079(Role role)
        {
            return role.Type == RoleTypeId.Scp079;
        }
        
        public static bool Is106(Role role)
        {
            return role.Type == RoleTypeId.Scp106;
        }
        
        public static bool IsDog(Role role)
        {
            return Is939(role);
        }
        
        public static bool IsDog(RoleTypeId role)
        {
            return Is939(role);
        }
        
        public static bool Is939(Role role)
        {
            return Is939(role.Type);
        }
        
        public static bool Is939(RoleTypeId role)
        {
            return role == RoleTypeId.Scp939;
        }
        
        public static bool Is3114(Role role)
        {
            return role.Type == RoleTypeId.Scp3114;
        }
        
        public static bool IsSpectator(Role role)
        {
            return role.Type == RoleTypeId.Spectator;
        }
        
        public static bool IsDead(Role role)
        {
            return role.IsDead;
        }
        
        public static bool IsAlive(Role role)
        {
            return role.IsAlive;
        }
        
        
        
    }
}