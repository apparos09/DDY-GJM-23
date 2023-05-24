using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    public class WeaponItem : WorldItem
    {
        [Header("Weapon")]

        // The type of weapon given to the player.
        public Weapon.weaponType type = Weapon.weaponType.none;

        // Give the player the weapon.
        protected override void GiveItem()
        {
            GameplayManager.Instance.player.AddWeapon(type);

            // Item has been gotten.
            OnItemGet();
        }

    }
}