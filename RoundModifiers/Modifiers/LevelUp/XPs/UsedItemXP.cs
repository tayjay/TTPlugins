using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class UsedItemXP : XP, IUsedItemEvent
    {
        public void OnUsedItem(UsedItemEventArgs ev)
        {
            float xp = 0;
            switch (ev.Usable.Type)
            {
                case ItemType.Adrenaline:
                case ItemType.Medkit:
                case ItemType.Painkillers:
                case ItemType.SCP330:
                    xp = 10;
                    break;
                case ItemType.SCP268:
                case ItemType.SCP1576:
                    xp = 20;
                    break;
                case ItemType.SCP207:
                case ItemType.AntiSCP207:
                case ItemType.SCP500:
                case ItemType.SCP1853:
                case ItemType.SCP2176:
                    xp = 50;
                    break;
                default:
                    xp = 0;
                    break;
            }
            if(xp>0)
                GiveXP(ev.Player, xp);
        }
    }
}