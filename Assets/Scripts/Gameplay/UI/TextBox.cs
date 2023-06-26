using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace DDY_GJM_23
{
    // A textbox that pops up in the game world. I don't think you'll need to tie events to the text box?
    public class TextBox : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        // The text object for the text box.
        public TMP_Text text;

        // The pages for the text box.
        public List<string> pages = new List<string>();

        // The index of the current page.
        public int pageIndex = -1;

        [Header("Buttons")]

        // If 'true', buttons are disabled if they can't be used.
        public bool disableButtonsIfUnusable = true;

        // The previous page button.
        public Button prevPageButton;

        // The next page button.
        public Button nextPageButton;

        // // If 'true', text is loaded instantly. If false, text is loaded word by word.
        // // TODO: handle using text highlight elements like bold, underline, etc.
        // public bool instantText = true;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Grabs the gameplay manager instance.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        //// Start is called before the first frame update
        //void Start()
        //{
            
        //}

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            OnTextBoxOpened();    
        }

        // This function is called when the behaviour becomes disabled and inactive
        private void OnDisable()
        {
            OnTextBoxClosed();
        }

        // Opens the text box.
        public void OnTextBoxOpened()
        {
            Time.timeScale = 0;
            gameManager.player.enableInputs = false;
        }

        // Closes the text box.
        public void OnTextBoxClosed()
        {
            Time.timeScale = 1;
            gameManager.player.enableInputs = true;
        }

        // Goes to the previous page.
        public void PreviousPage()
        {
            // Reduce the index.
            pageIndex -= 1;

            // Bounds check.
            if(pageIndex < 0)
                pageIndex = 0;

            // Updates the text box text.
            UpdateText();
        }

        // Goes to the next page.
        // If 'closeIfLast' is set to 'true', then the text box will be closed if there is no page.
        public void NextPage(bool closeIfNoPage)
        {
            // Increase the index.
            pageIndex += 1;

            // Prevent an out of bounds check.
            if (pageIndex >= pages.Count)
            {
                pageIndex = pages.Count - 1;

                // There are no pages left.
                if (closeIfNoPage)
                {
                    gameManager.CloseTextBox();
                }
                else
                {
                    // Updates the text box text.
                    UpdateText();
                }
            }
            else
            {
                // Updates the text box text.
                UpdateText();
            }            
        }

        // Goes to the next page. Defaults 'closeIfNoPage' to true.
        public void NextPage()
        {
            NextPage(true);
        }


        // Goes to the first page.
        public void OpenFirstPage()
        {
            pageIndex = 0;
            UpdateText();
        }

        // Clears pages out of the text box.
        public void ClearPages()
        {
            pages.Clear();
            pageIndex = -1;

            text.text = string.Empty;
        }


        // Updates the text being displayed in the text box.
        public void UpdateText()
        {
            // Index out of bounds.
            if (pageIndex < 0 || pageIndex >= pages.Count)
                return;

            // TODO: add scrolling text and ability to turn off tutorial elements.

            // Replaces the text.
            text.text = pages[pageIndex];
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: add scrolling text.

            // Checks if the previous button is set.
            if(prevPageButton != null)
            {
                // If the button interaction should be changed.
                if (disableButtonsIfUnusable && pageIndex <= 0)
                {
                    if(prevPageButton.interactable)
                        prevPageButton.interactable = false;
                }
                else if (disableButtonsIfUnusable && pageIndex > 0)
                {
                    if(!prevPageButton.interactable)
                        prevPageButton.interactable = true;
                }
                    
            }
        }
    }
}