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

        [Header("HUD")]
        // The timer text.
        public TMP_Text timerText;

        // The number of scraps the player has.
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

        [Header("UI")]

        // This button is used to retire (end the game early).
        // It should only be active if the player is in the base when the menu is opened.
        [Tooltip("Ends the game early. Should only be available if the player is in the base.")]
        public Button retireButton;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the gameplay manager instance.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        // Call to open the settings.
        public void OpenSettings()
        {
            // ...

            // Only active if player is in the base.
            retireButton.interactable = gameManager.homeBase.IsPlayerInBase();
        }

        // Call to close the settings.
        public void CloseSettings()
        {
            // ...
        }

        public void OpenMap()
        {

        }

        public void CloseMap()
        {

        }

        // Sets the weapon icon based on the type.
        public void SetWeaponIcon(Weapon.weaponType type)
        {

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
                switch (weapon.WeaponType)
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

        // Update is called once per frame
        void Update()
        {
            // Formats the timer and displays it on screen.
            if (!gameManager.pausedTimers)
                timerText.text = gameManager.GetTimerFormatted();

            // Update scrap count.
            if (gameManager.scrapsTotal.ToString() != scrapsText.text)
                scrapsText.text = gameManager.scrapsTotal.ToString();

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