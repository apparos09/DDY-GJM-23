using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A weapon uses item.
    public class WeaponUsesItem : WorldItem
    {
        [Header("Ammo")]

        // The type of ammo given. If the type is 'none', then it works for any weapon
        public Weapon.weaponType weaponType = Weapon.weaponType.none;

        // The amount of uses given for the weapon.
        public int usesAmount = 1;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Id not set.
            if (id == itemId.none)
                id = itemId.weaponUses;
        }

        // Gives the player the item.
        protected override void GiveItem()
        {
            GameplayManager.Instance.player.AddWeaponUses(weaponType, usesAmount);

            // Item has been gotten.
            OnItemGet();
        }
    }
}