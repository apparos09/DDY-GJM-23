using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The game notifications.
    public class Tutorial : MonoBehaviour
    {
        // The current text box has a limit of 280 characters (measured using the 'W' key)

        // The enum for the tutorials.
        public enum trlType { none, debug, opening, scrapItem, keyItem, healthItem, 
            weaponRefill, punch, gunSlow, gunMid, gunFast, runPower, swimPower,
            softBlock, hardBlock, lockBlock, portal, death  }

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

        // The area mechanics.
        public bool usedSoftBlock = false;
        public bool usedHardBlock = false;
        public bool usedLockBlock = false;
        public bool usedPortal = false;

        // The death tutorial.
        public bool usedDeath = false;



        // Gets the tutorial by type.
        public List<string> GetTutorialByType(trlType type, bool ignoreUsed)
        {
            // The list of pages.
            List<string> pages = new List<string>();

            // Gets set to 'true', if the tutorial has already been cleared.
            bool alreadyUsed = false;

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
                    alreadyUsed = usedOpening;
                    pages = GetOpeningTutorial();
                    break;

                    // ITEMS

                case trlType.scrapItem:
                    alreadyUsed = usedScrapItem;
                    pages = GetScrapItemTutorial();
                    break;

                case trlType.keyItem:
                    alreadyUsed = usedKeyItem;
                    pages = GetKeyItemTutorial();
                    break;

                case trlType.healthItem:
                    alreadyUsed = usedHealthItem;
                    pages = GetHealthItemTutorial();
                    break;

                case trlType.weaponRefill:
                    alreadyUsed = usedWeaponRefill;
                    pages = GetWeaponRefillTutorial();
                    break;


                    // WEAPONS
                case trlType.punch:
                    alreadyUsed = usedPunch;
                    pages = GetPunchTutorial();
                    break;

                case trlType.gunSlow:
                    alreadyUsed = usedGunSlow;
                    pages = GetGunSlowTutorial();
                    break;

                case trlType.gunMid:
                    alreadyUsed = usedGunMid;
                    pages = GetGunMidTutorial();
                    break;

                case trlType.gunFast:
                    alreadyUsed = usedGunFast;
                    pages = GetGunFastTutorial();
                    break;

                case trlType.runPower:
                    alreadyUsed = usedRunPower;
                    pages = GetRunPowerTutorial();
                    break;

                case trlType.swimPower:
                    alreadyUsed = usedSwimPower;
                    pages = GetSwimPowerTutorial();
                    break;


                    // AREA
                case trlType.softBlock:
                    alreadyUsed = usedSoftBlock;
                    pages = GetSoftBlockTutorial();
                    break; 
                
                case trlType.hardBlock:
                    alreadyUsed = usedHardBlock;
                    pages = GetHardBlockTutorial();
                    break;

                case trlType.lockBlock:
                    alreadyUsed = usedLockBlock;
                    pages = GetLockBlockTutorial();
                    break;

                case trlType.portal:
                    alreadyUsed = usedPortal;
                    pages = GetPortalTutorial();
                    break;


                    // DEATH
                case trlType.death:
                    alreadyUsed = usedDeath;
                    pages = GetDeathTutorial();
                    break;
            }

            // If the tutorial has already been used, and cleared tutorial should be ignored.
            if (alreadyUsed && ignoreUsed)
                pages.Clear();

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
                "Welcome to the outside world! Your job is to collect as many scraps as you can and bring them back to home base while your life support system lasts. The timer above shows how long you have until your suit fails. Make sure that you're in the base before time runs out.",
                "Use the <b>WASD keys</b> to move, the <b>space bar</b> to attack, and the <b>up arrow key</b> to open the map. In the map window, you have the option to end early, but you can only do so if you're in the base area. The map can be closed by pressing the <b>up arrow key</b> again.",
                "If you ever need to restore your health and/or replenish your weapons, just return to the home base. And if you want to change the game settings, use the <b>escape key</b> to toggle the settings window. That's all, so good luck, scavenger!"
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
                "You found some <b>scrap</b>! You can carry an unlimited amount of scrap, but make sure to bring it all back to the home base at some point. If you die, you'll lose all of the scrap that you're holding, so keep a close eye on your health."
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
               "This is a <b>key</b>! Keys are used to remove lock blocks."
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
                "This is a <b>health pack</b>, which restores some of your health upon being used. To use a health pack, press the down arrow key."
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
                "You got a <b>weapon refill</b>! This replenishes your current weapon. If your current weapon doesn't need to be replenished, the refill is given to another one of your weapons."
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
                "You got the <b>punch move</b>! This is an infinite use move that attacks the targets in front of you. If you have multiple weapons, use the left and right arrow keys to switch between them."
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
                "You got the <b>S-Type Blaster</b>! This blaster fires slow, powerful shots. If you have multiple weapons, use the left and right arrow keys to switch between them."
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
                "You got the <b>M-Type Blaster</b>! This blaster fires moderately fast, decently strong shots. If you have multiple weapons, use the left and right arrow keys to switch between them."
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
                "You got the <b>F-Type Blaster</b>! This blaster fires fast, weak shots. If you have multiple weapons, use the left and right arrow keys to switch between them."
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
                "You got the <b>R-Gear</b>! This gear allows you to run faster on solid surfaces when equipped, but you can only attack by punching. If you have multiple weapons, use the left and right arrow keys to switch between them."
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
                "You got the <b>S-Gear</b>! When equipped, this gear allows you to run faster on liquid surfaces, and makes you immune to poison hazards. However, you can only attack by punching when this gear is active. If you have multiple weapons, use the left and right arrow keys to switch between them."
            };

            usedSwimPower = true;

            // Returns the pages.
            return pages;
        }


        // AREA MECHANICS
        // Soft block tutorial.
        public List<string> GetSoftBlockTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "This is a <b>soft block</b>! This block can be broken by any kind of weapon."
            };

            usedSoftBlock = true;

            // Returns the pages.
            return pages;
        }

        // Hard block tutorial.
        public List<string> GetHardBlockTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "This is a <b>hard block</b>! This block can only be broken by blaster weapons."
            };

            usedHardBlock = true;

            // Returns the pages.
            return pages;
        }

        // Lock Block Tutorial
        public List<string> GetLockBlockTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                 "This is a <b>lock block</b>! Lock blocks can only be removed using a key."
            };

            usedLockBlock = true;

            // Returns the pages.
            return pages;
        }

        // Portal tutorial.
        public List<string> GetPortalTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You encountered a <b>portal</b>! It'll take you to another area in the world. Make sure to use the map to see where you are."
            };

            usedPortal = true;

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
                "<b>You died.</b> The scraps you were holding have been lost, but the ones already at the base are okay. You've also lost your keys and health packs, but you still have the weapons that you collected."
            };

            usedDeath = true;

            // Returns the pages.
            return pages;
        }
    }
}