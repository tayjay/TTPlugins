using Exiled.API.Features;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class AnnouncementAbility : Ability
    {
        private string message;
        private bool isHeld, isNoisy, oneShot, isDone;
        
        public AnnouncementAbility(string msg, bool isHeld = true, bool isNoisy = false, int aggressionLevel = 0, bool oneShot = false) : base("CASSIE Announcement", "", Exiled.API.Enums.Side.None, aggressionLevel, lifetime:20)
        {
            message = msg;
            this.isHeld = isHeld;
            this.isNoisy = isNoisy;
            this.oneShot = oneShot;
            this.isDone = false;
        }
        
        public override bool Setup()
        {
            if (!oneShot) return true;
            if (isDone) return false;
            isDone = true;
            return true;
        }

        public override void Start()
        {
            base.Start();
            Cassie.Message(message, isHeld, isNoisy);
        }

        public override bool Update()
        {
            return isHeld;
        }
    }
}