using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class LockDoorAbility : Ability
    {
        public Door Door { get; set; }
        private int ticks, doorLockTIme;
        public LockDoorAbility(Door door = null, int doorLockTime = 10, Side helpingSide=Side.None) : base("Lock Door", "Locks a door", helpingSide, 5)
        {
            Door = door;
            doorLockTIme = doorLockTime;
            Lifetime = doorLockTime;
        }
        
        public override bool Setup()
        {
            Side leadingSide =
                (RoundModifiers.Instance.GetModifier<RogueAI>())
                .GetLeadingSide();
            
            Player randomPlayer = null;
            if(leadingSide == Side.None)
            {
                randomPlayer = Player.List.GetRandomValue();
            }
            else
            {
                randomPlayer = Player.Get(leadingSide).GetRandomValue();
            }
            if(randomPlayer == null)
            {
                return false;
            }
            Door closestDoor = Door.GetClosest(randomPlayer.Position, out float distance);
            if (closestDoor == null)
            {
                return false;
            }
            Door = closestDoor;
            return true;
        }

        public override void Start()
        {
            base.Start();
            ticks = 0;
            Door.IsOpen = false;
            Door.Lock(doorLockTIme,DoorLockType.Lockdown2176);
            Door.AllowsScp106 = false;
        }
        public override bool Update()
        {
            return true;
        }

        public override void End()
        {
            base.End();
            Door.Unlock();
            Door.AllowsScp106 = true;
        }
    }
}