using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace RoundModifiers.Modifiers.WeaponStats.Interfaces;

public interface IAdding
{
    void Adding(Firearm firearm, Player player);
    void Removing(Firearm firearm, Player player);
}