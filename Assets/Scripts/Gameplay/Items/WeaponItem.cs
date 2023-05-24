using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A weapon item - gives the player a weapon.
    // TODO: have a seperate script for ammo.
    public class WeaponItem : WorldItem
    {
        [Header("Weapon")]

        // The type of weapon given to the player.
        public Weapon.weaponType type = Weapon.weaponType.none;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            id = itemId.weapon;
        }

        // Give the player the weapon.
        protected override void GiveItem()
        {
            GameplayManager.Instance.player.AddWeapon(type);

            // Item has been gotten.
            OnItemGet();
        }

    }
}