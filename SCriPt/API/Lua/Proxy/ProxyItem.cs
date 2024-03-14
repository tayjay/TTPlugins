using Exiled.API.Features;
using Exiled.API.Features.Items;
using MoonSharp.Interpreter;

namespace SCriPt.API.Lua.Proxy
{
    public class ProxyItem
    {
        protected Item Item { get; }
        
        [MoonSharpHidden]
        public ProxyItem(Item item)
        {
            Item = item;
        }
        
        public ushort Serial => Item.Serial;
        public string Type => Item.Type.ToString();
        
        public bool IsAmmo => Item.IsAmmo;
        public bool IsArmor => Item.IsArmor;
        public bool IsKeycard => Item.IsKeycard;
        public bool IsConsumable => Item.IsConsumable;
        public bool IsThrowable => Item.IsThrowable;
        public bool IsUsable => Item.IsUsable;
        public bool IsWeapon => Item.IsWeapon;
        public bool IsDisarmer => Item.IsDisarmer;
        public Player Owner => Item.Owner;
        
        public void Destroy()
        {
            Item.Destroy();
        }
    }
}