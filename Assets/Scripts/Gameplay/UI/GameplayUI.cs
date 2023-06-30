using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using util;

namespace DDY_GJM_23
{
    // The Ui for the gameplay.
    public class GameplayUI : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        // Automatically updates the player's health.
        public bool autoHealthUpdate = true;

        // Automatically updates the weapon uses.
        public bool autoWeaponUsesUpdate = true;

        [Header("HUD/Scrap Goal")]
        // The text for the scrap goal.
        public TMP_Text scrapGoalText;

        // TODO: Add colours?

        [Header("HUD/General")]
        // The timer text.
        public TMP_Text timerText;

        // The number of scraps the player has on hand.
        [Tooltip("The number of scraps the player has on-hand, not in the base.")]
        public TMP_Text scrapsText;

        // The number of keys the player has.
        public TMP_Text keysText;

        // The number of heals the player has.
        public TMP_Text healsText;

        // The player health bar.
        public ProgressBar healthBar;

        [Header("HUD/Weapons")]

        // The weapon image.
        public Image weaponImage;

        // The ammo count for the ammo.
        public TMP_Text usesText;

        // Sprite for slow gun.
        public Sprite punchSprite;

        // Sprite for slow gun.
        public Sprite gunSlowSprite;

        // Sprite for mid gun.
        public Sprite gunMidSprite;

        // Sprite for fast gun.
        public Sprite gunFastSprite;

        // Sprite for run power.
        public Sprite runPowerSprite;

        // Sprite for swim power.
        public Sprite swimPowerSprite;

        [Header("HUD/Map")]

        // THe map that gets enabled.
        public GameplayMap map;     

        [Header("UI")]

        // The textbox for the game.
        public TextBox textBox;

        // This button is used to retire (end the game early).
        // It should only be active if the player is in the base when the menu is opened.
        [Tooltip("Ends the game early. Should only be available if the player is in the base.")]
        public Button endEarlyButton;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the gameplay manager instance.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;

            // If the map isn't set, get the component in the children.
            if(map == null)
            {
                map = GetComponentInChildren<GameplayMap>();
            }
        }

        // GENERAL //
        // Sets the weapon icon based on the type.
        public void SetWeaponIcon(Weapon.weaponType type)
        {
            // Set the default color.
            weaponImage.color = Color.white;

            // Updates the weapon icon.
            switch (type)
            {
                default:
                case Weapon.weaponType.none:
                    weaponImage.sprite = null;
                    weaponImage.color = new Color(0, 0, 0, 0);

                    break;

                case Weapon.weaponType.punch:
                    weaponImage.sprite = punchSprite;
                    break;

                case Weapon.weaponType.gunSlow:
                    weaponImage.sprite = gunSlowSprite;
                    break;

                case Weapon.weaponType.gunMid:
                    weaponImage.sprite = gunMidSprite;
                    break;

                case Weapon.weaponType.gunFast:
                    weaponImage.sprite = gunFastSprite;
                    break;

                case Weapon.weaponType.runPower:
                    weaponImage.sprite = runPowerSprite;
                    break;

                case Weapon.weaponType.swimPower:
                    weaponImage.sprite = swimPowerSprite;
                    break;
            }
        }

        // Updates the weapon information.
        public void UpdateWeaponInfo()
        {
            // Gets the current weapon.
            Weapon weapon = gameManager.player.currentWeapon;

            // Weapon set.
            if (weapon != null)
            {
                // Set icon to white color.
                weaponImage.color = Color.white;

                // Updates the weapon info.
                switch (weapon.type)
                {
                    default:
                    case Weapon.weaponType.none:
                        weaponImage.sprite = null;
                        weaponImage.color = new Color(0, 0, 0, 0);

                        break;

                    case Weapon.weaponType.punch:
                        weaponImage.sprite = punchSprite;
                        break;

                    case Weapon.weaponType.gunSlow:
                        weaponImage.sprite = gunSlowSprite;
                        break;

                    case Weapon.weaponType.gunMid:
                        weaponImage.sprite = gunMidSprite;
                        break;

                    case Weapon.weaponType.gunFast:
                        weaponImage.sprite = gunFastSprite;
                        break;

                    case Weapon.weaponType.runPower:
                        weaponImage.sprite = runPowerSprite;
                        break;

                    case Weapon.weaponType.swimPower:
                        weaponImage.sprite = swimPowerSprite;
                        break;
                }


                // Set the ammo count.
                if (weapon.infiniteUse)
                {
                    usesText.text = "-";
                }
                else
                {
                    usesText.text = weapon.uses.ToString();
                }
            }
            else // Weapon not set.
            {
                weaponImage.sprite = null;
                weaponImage.color = new Color(0, 0, 0, 0);
                usesText.text = "-";
            }

        }

        // MAP //

        // Checks if the map is currently open.
        public bool IsMapOpen()
        {
            bool result = map.isActiveAndEnabled;
            return result;
        }

        // Open the map.
        public void OpenMap()
        {
            Time.timeScale = 0.0F;
            gameManager.player.enableInputs = false;

            // Activate the map object.
            map.gameObject.SetActive(true);

            // Calls this in case it didn't get set with onEnable (which is currently not happening for some reason).
            // map.PlacePlayerMarker();

            // Only active if player is in the base.
            endEarlyButton.interactable = gameManager.homeBase.IsPlayerInBase();
        }

        // Close the map.
        public void CloseMap()
        {
            Time.timeScale = 1.0F;
            gameManager.player.enableInputs = true;

            // Deactivate the map object.
            map.gameObject.SetActive(false);
        }

        // Toggles the map on and off
        public void ToggleMap()
        {
            // Checks if the map is open.
            if (IsMapOpen()) // Map currently open.
            {
                CloseMap();
            }
            else // Map not currently open.
            {
                OpenMap();
            }
        }


        // TUTORIAL //
        // Return 'true' if the text box is open.
        public bool IsTextBoxOpen()
        {
            bool result = textBox.isActiveAndEnabled;
            return result;
        }


        // NOTE: a text box shouldn't be opened while the map is also open.
        // Opens the text box.
        public void OpenTextBox(List<string> pages)
        {
            textBox.pages = pages;
            textBox.gameObject.SetActive(true);
            textBox.OpenFirstPage();

            // NOTE: this always opens on the first page. You will never need to open up on another page at the start...
            // So leaving it like this is fine.
        }

        // Closes the text box.
        // If 'clearPages' is true, the text box elements are cleared out.
        public void CloseTextBox(bool clearPages = false)
        {
            // Clears out the pages.
            if (clearPages)
                textBox.ClearPages();

            // Turn off the text box.
            textBox.gameObject.SetActive(false);
        }


        // SETTINGS //
        // Call to open the settings.
        public void OpenSettings()
        {
            // Stop the game.
            Time.timeScale = 1.0F;

            // Stop the player.
            gameManager.player.enableInputs = false;

            // Only active if player is in the base.
            endEarlyButton.interactable = gameManager.homeBase.IsPlayerInBase();
        }

        // Call to close the settings.
        public void CloseSettings()
        {
            // Start the game.
            Time.timeScale = 1.0F;


            // Allow player inputs if the map and the text box aren't open.
            if(!IsMapOpen() && !IsTextBoxOpen())
            {
                gameManager.player.enableInputs = true;
            }
        }



        // Ends the game early by going to the results scene.
        public void EndGame()
        {
            gameManager.ToResultsScene();
        }

        // Update is called once per frame
        void Update()
        {
            // Scrap Goal - TODO: account for negatives and there being no goal
            if (scrapGoalText.text != gameManager.scrapGoal.ToString())
            {
                // Set the ToString
                // TODO: change the text based on how much the player has at base. 
                scrapGoalText.text = gameManager.scrapGoal.ToString();
            }

            // Formats the timer and displays it on screen.
            if (!gameManager.pausedTimers)
                timerText.text = gameManager.GetTimerFormatted();

            // // Update scrap count (OLD) - shows total scrap count.
            // if (gameManager.scrapsTotal.ToString() != scrapsText.text)
            //     scrapsText.text = gameManager.scrapsTotal.ToString();

            // Update scrap count (player's amount on hand).
            if (gameManager.player.scrapCount.ToString() != scrapsText.text)
                scrapsText.text = gameManager.player.scrapCount.ToString();

            // Update key count.
            if (gameManager.player.keyCount.ToString() != keysText.text)
                keysText.text = gameManager.player.keyCount.ToString();

            // Update heals count.
            if (gameManager.player.healAmount.ToString() != healsText.text)
                healsText.text = gameManager.player.healCount.ToString();


            // Player Health Update
            if (autoHealthUpdate)
            {
                float healthPercent = gameManager.player.GetHealthAsPercentage();

                // The amoutns are different.
                if(healthPercent != healthBar.GetValueAsPercentage())
                healthBar.SetValue(healthPercent);
            }


            // Weapon Uses Update
            if (autoWeaponUsesUpdate)
            {
                // The weapon.
                Weapon weapon = gameManager.player.currentWeapon;

                // Weapon found.
                if (weapon != null)
                {
                    // Set the ammo count.
                    if (weapon.infiniteUse)
                    {
                        usesText.text = "-";
                    }
                    else
                    {
                        usesText.text = weapon.uses.ToString();
                    }
                }
            }

        }
    }
}