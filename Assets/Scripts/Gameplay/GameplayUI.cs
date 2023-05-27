using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DDY_GJM_23
{
    // The Ui for the gameplay.
    public class GameplayUI : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        [Header("HUD")]
        // The timer text.
        public TMP_Text timerText;

        [Header("UI")]

        // This button is used to retire (end the game early).
        // It should only be active if the player is in the base when the menu is opened.
        [Tooltip("Ends the game early. Should only be available if the player is in the base.")]
        public Button retireButton;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the gameplay manager instance.
            if(gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        // Call to open the menu.
        public void OpenMenu()
        {
            // ...

            // Only active if player is in the base.
            retireButton.interactable = gameManager.homeBase.IsPlayerInBase();
        }

        // Call to close the menu.
        public void CloseMenu()
        {
            // ...
        }

        // Update is called once per frame
        void Update()
        {
            // Formats the timer and displays it on screen.
            if (!gameManager.pausedTimers)
                timerText.text = gameManager.GetTimerFormatted();
        }
    }
}