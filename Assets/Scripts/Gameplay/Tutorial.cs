using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace DDY_GJM_23
{
    // The game notifications.
    public class Tutorial : MonoBehaviour
    {
        // The current text box has a limit of 280 characters (measured using the 'W' key)

        // The enum for the tutorials.
        public enum trlType { none, debug, opening, scrapItem, keyItem, healthItem, 
            weaponRefill, punch, gunSlow, gunMid, gunFast, runPower, swimPower, death  }

        // Variables for getting notifications.
        public bool usedOpening = false;

        // The item tutorials.
        public bool usedScrapItem = false;
        public bool usedKeyItem = false;
        public bool usedHealthItem = false;
        public bool usedWeaponRefill = false;

        // The weapon tutorials.
        public bool usedPunch = false;
        public bool usedGunSlow = false;
        public bool usedGunMid = false;
        public bool usedGunFast = false;
        public bool usedRunPower = false;
        public bool usedSwimPower = false;

        // The death tutorial.
        public bool usedDeath = false;



        // Gets the tutorial by type.
        public List<string> GetTutorialByType(trlType type)
        {
            // The list of pages.
            List<string> pages = new List<string>();

            switch (type)
            {
                case trlType.none:
                default:
                    pages = new List<string>();
                    break;

                case trlType.debug:
                    pages = GetDebugTutorial();
                    break;

                case trlType.opening:
                    pages = GetOpeningTutorial();
                    break;

                case trlType.scrapItem:
                    pages = GetScrapItemTutorial();
                    break;

                case trlType.keyItem:
                    pages = GetKeyItemTutorial();
                    break;

                case trlType.healthItem:
                    pages = GetHealthItemTutorial();
                    break;

                case trlType.punch:
                    pages = GetPunchTutorial();
                    break;

                case trlType.gunSlow:
                    pages = GetGunSlowTutorial();
                    break;

                case trlType.gunMid:
                    pages = GetGunMidTutorial();
                    break;

                case trlType.gunFast:
                    pages = GetGunFastTutorial();
                    break;

                case trlType.runPower:
                    pages = GetRunPowerTutorial();
                    break;

                case trlType.swimPower:
                    pages = GetSwimPowerTutorial();
                    break;

                case trlType.death:
                    pages = GetDeathTutorial();
                    break;
            }

            return pages;
        }

        // Gets the debug text.
        public List<string> GetDebugTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "This is a test.",
                "This is only a test."
            };

            // Returns the pages.
            return pages;
        }

        // Gets the opening text.
        public List<string> GetOpeningTutorial()
        {
            // Grabs the player from the gameplay manager.
            GameplayManager manager = GameplayManager.Instance;
            Player player = manager.player;

            // The page to be returned.
            List<string> pages = new List<string>()
            {
                "Welcome to the outside world. You'll need to get as many scraps as you can and bring them back to base while your life support system lasts.",
                "The timer above shows how long you have. You want to make sure that you're in the base before time runs out.",
                "For help with navigation, you can view the map by pressing the (" + player.mapKey.ToString() + ") key.",
                "If you want to end early, open the map screen to get the option. Keep in mind that you can only end early if you're currently in the base.",
                "Use WASD to move, and the space bar to attack. If you ever need to restore your health, return to the home base.",
                "That's all, so good luck scavenger."
            };

            // The opening has been used.
            usedOpening = true;

            return pages;
        }

        // ITEMS //
        // Gets the scrap item tutorial.
        public List<string> GetScrapItemTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You found some scrap! You can carry as much scrap as you want, but make sure to bring it all back to the base at some point.",
                "If you die, you'll lose the scrap you have, so keep a close eye on your health.",
            };

            usedScrapItem = true;

            // Returns the pages.
            return pages;
        }

        // Gets the key item tutorial.
        public List<string> GetKeyItemTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "This is a key! You can use it to unlock lock boxes to access more areas! " +
                "Keys are used automatically upon touching a locked object.",
            };

            usedKeyItem = true;

            // Returns the pages.
            return pages;
        }

        // Gets the health item tutorial.
        public List<string> GetHealthItemTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "This is a health pack, which restores health. To use the health pack, use the (" 
                + GameplayManager.Instance.player.healKey.ToString() + ") key.",
            };

            usedHealthItem = true;

            // Returns the pages.
            return pages;
        }

        // Gets the weapon refill item tutorial.
        public List<string> GetWeaponRefillTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You get a weapon refill. This restores the uses for your current weapon. If your current weapon has all its uses, this is passed to the next weapon in your list."
            };

            usedWeaponRefill = true;

            // Returns the pages.
            return pages;
        }


        // WEAPONS //
        // Gets the punch tutorial.
        public List<string> GetPunchTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You got the punch weapon. This is an infinite use weapon that damages the targets in front of you.",
                "If you have multiple weapons, you can switch between them using the the left and right arrows."
            };

            usedPunch = true;

            // Returns the pages.
            return pages;
        }

        // Gets the gun slow tutorial.
        public List<string> GetGunSlowTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You got the S-Type Blaster! This blaster fires a slow shot that does major damage.",
                " If you have multiple weapons, you can switch between them using the the left and right arrows."
            };

            usedGunSlow = true;

            // Returns the pages.
            return pages;
        }

        // Gets the gun mid tutorial.
        public List<string> GetGunMidTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You got the M-Type Blaster! This blaster fires moderately fast shots that do decent damage." +
                " If you have multiple weapons, you can switch between them using the the left and right arrows."
            };

            usedGunMid = true;

            // Returns the pages.
            return pages;
        }

        // Gets the gun fast tutorial.
        public List<string> GetGunFastTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You got the F-Type Blaster! This blaster fires fast shots that do little damage." +
                " If you have multiple weapons, you can switch between them using the the left and right arrows."
            };

            usedGunFast = true;

            // Returns the pages.
            return pages;
        }

        // Gets the run up tutorial.
        public List<string> GetRunPowerTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You got the Run Gear! When equipped, you run faster, but you can only attack by punching." +
                " If you have multiple weapons, you can switch between them using the the left and right arrows."
            };

            usedRunPower = true;

            // Returns the pages.
            return pages;
        }

        // Gets the swim up tutorial.
        public List<string> GetSwimPowerTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You got the Swim Gear. This gear allows you to swim faster, and makes you immune to poison damage." +
                " If you have multiple weapons, you can switch between them using the the left and right arrows."
            };

            usedSwimPower = true;

            // Returns the pages.
            return pages;
        }


        // DEATH //
        // Gets the death tutorial.
        public List<string> GetDeathTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You died. The scraps you were holding have been lost, but the ones at the base are okay.",
                "You've lost your keys and health packs as well, but you still have the weapons that you collected."
            };

            usedDeath = true;

            // Returns the pages.
            return pages;
        }
    }
}