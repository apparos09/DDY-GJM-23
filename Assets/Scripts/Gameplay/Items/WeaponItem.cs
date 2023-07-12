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

            // Auto-set id.
            if (id == itemId.none)
                id = itemId.weapon;
        }

        // Give the player the weapon.
        protected override void GiveItem()
        {
            GameplayManager manager = GameplayManager.Instance;
            
            // Set the manager.
            manager.player.AddWeapon(type);

            // Attempts to activate the tutorial.
            switch(type)
            {
                case Weapon.weaponType.punch: // Punch
                    manager.ActivateTutorial(Tutorial.trlType.punch);
                    break;

                case Weapon.weaponType.gunSlow: // Gun Slow
                    manager.ActivateTutorial(Tutorial.trlType.gunSlow);
                    break;

                case Weapon.weaponType.gunMid: // Gun Mid
                    manager.ActivateTutorial(Tutorial.trlType.gunMid);
                    break;

                case Weapon.weaponType.gunFast: // Gun Fast
                    manager.ActivateTutorial(Tutorial.trlType.gunFast);
                    break;

                case Weapon.weaponType.runPower: // Run Power
                    manager.ActivateTutorial(Tutorial.trlType.runPower);
                    break;

                case Weapon.weaponType.swimPower: // Swim Power
                    manager.ActivateTutorial(Tutorial.trlType.swimPower);
                    break;
            }

            // Item has been gotten.
            OnItemGet();
        }

    }
}