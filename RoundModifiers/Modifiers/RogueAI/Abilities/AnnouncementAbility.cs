using Exiled.API.Features;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class AnnouncementAbility : Ability
    {
        private string message;
        private bool isHeld, isNoisy;
        
        public AnnouncementAbility(string msg, bool isHeld = true, bool isNoisy = false, int aggressionLevel = 0) : base("CASSIE Announcement", "", Exiled.API.Enums.Side.None, aggressionLevel, lifetime:20)
        {
            message = msg;
            this.isHeld = isHeld;
            this.isNoisy = isNoisy;
        }
        
        public override bool Setup()
        {
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