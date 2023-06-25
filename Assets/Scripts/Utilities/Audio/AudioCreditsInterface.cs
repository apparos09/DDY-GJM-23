using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace util
{
    public class AudioCreditsInterface : MonoBehaviour
    {
        // The audio references object.
        public AudioCredits audioCredits;

        // The credit index for the audio reference.
        private int creditIndex = 0;

        // The user interface for the credits menu.
        [Header("UI")]

        // The name of the song.
        public TMP_Text songTitleText;

        // The name of the artist(s).
        public TMP_Text artistsText;

        // The name of the album/group that the song comes from.
        public TMP_Text collectionText;

        // The source of the song, which will be a website most likely.
        public TMP_Text sourceText;

        // The link to the the song (website, website page, etc.). This is a link to the source you used.
        public TMP_Text link1Text;

        // The link to the the song (website, website page, etc.). This second link is for the orgination of the audio.
        public TMP_Text link2Text;

        // The text for the copyright information.
        public TMP_Text copyrightText;

        // The page number text, which is a fraction (000/000)
        public TMP_Text pageNumberText;



        // Start is called before the first frame update
        void Start()
        {
            // The audio credits component is not set.
            if (audioCredits == null)
            {
                // If an attempt to grab the component from the gameObject fails, automatically add it.
                if(!gameObject.TryGetComponent(out audioCredits))
                {
                    // Adds the audio credits component.
                    audioCredits = gameObject.AddComponent<AudioCredits>();
                }
                    
            }

            // Loads credit and sets page number.
            UpdateCredit();
        }

        // Sets the index of the page.
        public void SetPageIndex(int newIndex)
        {
            // The reference count.
            int refCount = audioCredits.GetCreditCount();

            // No references to load.
            if (refCount == 0)
            {
                creditIndex = -1;
                return;
            }

            // Sets the new index, clamping it so that it's within the page count.
            creditIndex = Mathf.Clamp(newIndex, 0, refCount - 1);

            // Updates the displayed credit.
            UpdateCredit();
        }

        // Goes to the previous page.
        public void PreviousPage()
        {
            // Generates the new index.
            int newIndex = creditIndex - 1;

            // Goes to the end of the list.
            if (!audioCredits.IndexInBounds(newIndex))
                newIndex = audioCredits.GetCreditCount() - 1;

            SetPageIndex(newIndex);
        }

        // Goes to the next page.
        public void NextPage()
        {
            // Generates the new index.
            int newIndex = creditIndex + 1;

            // Goes to the start of the list.
            if (!audioCredits.IndexInBounds(newIndex))
                newIndex = 0;

            SetPageIndex(newIndex);
        }

        // Sets the page number text.
        public virtual void UpdatePageNumberText()
        {
            // Updates the page number.
            if(pageNumberText != null)
                pageNumberText.text = (creditIndex + 1).ToString() + "/" + audioCredits.GetCreditCount().ToString();
        }

        // Updates the credit.
        public void UpdateCredit()
        {
            // No credit to update, or index out of bounds.
            if (audioCredits.GetCreditCount() == 0 || !audioCredits.IndexInBounds(creditIndex))
                return;

            // Gets the credit.
            AudioCredits.AudioCredit credit = audioCredits.audioCredits[creditIndex];

            // This checks for each text component since the user may not want to display all info.

            // Updates all of the information.
            // Song Title - the song's name.
            if(songTitleText != null)
                songTitleText.text = credit.title;

            // Artists - the artist(s) responsible for the audio.
            if(artistsText != null)
                artistsText.text = credit.artists;

            // Collection - the album or package the audio came from.
            if(collectionText != null)
                collectionText.text = credit.collection;

            // Source  - preferably the publisher or host of the audio (e.g., the free audio website name).
            if (sourceText != null)
                sourceText.text = credit.source;

            // Primary Link - preferably the direct source link of where the audio came from (e.g., free audio website).
            if (link1Text != null)
                link1Text.text = credit.link1;

            // Secondary Link - preferably the original source link of the audio (e.g., the artist's website).
            if (link2Text != null)
                link2Text.text = credit.link2;
            
            // Copyright - copyright information. Also include information on composition and arrangement if needed.
            if(copyrightText != null)
                copyrightText.text = credit.copyright;

            // Updates the page number.
            UpdatePageNumberText();
        }
    }
}