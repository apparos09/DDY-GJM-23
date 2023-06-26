using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        public int pageIndex = 0;

        // If 'true', text is loaded instantly. If false, text is loaded word by word.
        // TODO: handle using text highlight elements like bold, underline, etc.
        public bool instantText = true;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the gameplay manager instance.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;
        }

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

        public void NextPage()
        {
            // Increase the index.
            pageIndex += 1;

            // Prevent an out of bounds check.
            if (pageIndex >= pages.Count)
                pageIndex = pages.Count - 1;

            // Updates the text box text.
            UpdateText();
        }


        // Updates the text being displayed in the text box.
        public void UpdateText()
        {
            // Index out of bounds.
            if (pageIndex < 0 && pageIndex >= pages.Count)
                return;

            // TODO: add scrolling text and ability to turn off tutorial elements.

            // Replaces the text.
            text.text = pages[pageIndex];
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: add scrolling text.
        }
    }
}