using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace DDY_GJM_23
{
    // The results manager.
    public class ResultsManager : MonoBehaviour
    {
        // The ResultsManager instance.
        private static ResultsManager instance;

        // Gets set to 'true' when the ResultsManager is initialized.
        // This isn't needed, but it helps with the clarity.
        private bool initialized = false;

        // The results data.
        public ResultsData results;

        [Header("UI")]

        // The text for showing the amount of player deaths.
        public TMP_Text deathsText;

        // The text for showing the player's end scrap count.
        public TMP_Text scrapsTotalText;

        // The text for showing the amount of times the player visited the base.
        public TMP_Text baseVisitsTex;

        // The number of keys used by the player.
        public TMP_Text keysUsedText;

        // Number of heals used by the player.
        public TMP_Text healthItemsUsed;

        // The text for showing how long the game went on for.
        public TMP_Text gameLengthText;

        // The text for showing if the player lived, and how successful they were.
        public TMP_Text ratingText;

        [Header("UI/Weapons")]

        // The color for an active icon (got item).
        public Color activeColor = Color.white;

        // The color for an inactive icon (didn't get item).
        public Color inactiveColor = Color.grey;

        // The gun slow icon.
        public Image gunSlowIcon;

        // The gun mid icon.
        public Image gunMidIcon;

        // The gun fast icon.
        public Image gunFastIcon;

        // The run upgrade icon.
        public Image runPowerIcon;

        // The swim upgrade icon.
        public Image swimPowerIcon;

        [Header("Other")]
        // The audio for the results screen.
        public ResultsAudio resultsAudio;

        // Constructor
        private ResultsManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Run code for initialization.
            if (!initialized)
            {
                initialized = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Finds the results data.
            if (results == null)
                results = FindObjectOfType<ResultsData>();

            // Results were found, so load them up.
            if (results != null)
                LoadResults();

            // Gets the results audio.
            if (resultsAudio == null)
                resultsAudio = GetComponent<ResultsAudio>();
        }

        // Gets the instance.
        public static ResultsManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<ResultsManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("ResultsManager (ResultsManager)");
                        instance = go.AddComponent<ResultsManager>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }

        // Loads the results.
        public void LoadResults()
        {
            // Deaths
            deathsText.text = "Deaths: " + results.deaths;

            // Scraps
            scrapsTotalText.text = "Scraps Collected: " + results.scrapsTotal.ToString();

            // Base Visits
            baseVisitsTex.text = "Home Base Visits: " + results.baseVisits.ToString();

            // The number of keys used.
            keysUsedText.text = "Keys Used: " + results.keysUsed.ToString();

            // The number of heal items used.
            healthItemsUsed.text = "Heals Used: " + results.healthItemsUsed.ToString();

            // The game length.
            gameLengthText.text = "Game Length: " + StringFormatter.FormatTime(results.gameLength, false, true, false);


            // WEAPONS
            gunSlowIcon.color = results.gotGunSlow ? activeColor : inactiveColor;
            gunMidIcon.color = results.gotGunMid ? activeColor : inactiveColor;
            gunFastIcon.color = results.gotGunFast ? activeColor : inactiveColor;
            
            runPowerIcon.color = results.gotRunPower ? activeColor : inactiveColor;
            swimPowerIcon.color = results.gotSwimPower ? activeColor : inactiveColor;


            // RATINGS
            // Old
            // ratingText.text = (results.survived) ? "You survived!" : "You did not survive!";
            string ratingMessage = "...";
            int scrapsTotal = results.scrapsTotal;
            int scrapGoal = results.scrapGoal;

            // New
            switch(results.survived)
            {
                // The player survived.
                default:
                case true:
                    // Checks the scrap total in reference to the scrap goal.
                    if(scrapsTotal == 0) // No scraps.
                    {
                        ratingMessage = "You survived, but you didn't collect any scraps! What happened? The base can't go on if days are wasted like this.";
                    }
                    else if(scrapsTotal < scrapGoal) // Under goal.
                    {
                        ratingMessage = "You survived, but you didn't collect many scraps. Maybe next time around you'll be able to collect more.";
                    }
                    else if(scrapsTotal == scrapGoal) // Met goal.
                    {
                        ratingMessage = "You survived, and you collected a good amount of scraps! It was a pretty productive day.";
                    }
                    else if(scrapsTotal > scrapGoal) // Over goal.
                    {
                        ratingMessage = "You survived, and you collected a lot of scraps! You're great at this!";
                    }
                    else
                    {
                        ratingMessage = "You survived!";
                    }

                break;

                    // The player did not survive.
                case false:
                    // Checks the scrap total in reference to the scrap goal.
                    if (scrapsTotal == 0) // No scraps.
                    {
                        ratingMessage = "You didn't survive, and you didn't collect any scraps... Someone else will have to take up the job now...";
                    }
                    else if (scrapsTotal < scrapGoal) // Under goal.
                    {
                        ratingMessage = "You didn't survive, and you didn't collect many scraps... Someone else will have to take up the job now...";
                    }
                    else if (scrapsTotal == scrapGoal) // Met goal.
                    {
                        ratingMessage = "You didn't survive, but you collected a good amount of scraps. Someone else will have to take up the job now though...";
                    }
                    else if (scrapsTotal > scrapGoal) // Over goal.
                    {
                        ratingMessage = "You didn't survive, but you did collect a lot of scraps! Your work was greatly appreciated.";
                    }
                    else
                    {
                        ratingMessage = "You did not survive!";
                    }

                    break;
            }

            // Set the ratings message.
            ratingText.text = ratingMessage;

            // Destroys the results game object.
            Destroy(results.gameObject);
        }

        // Loads the title scene.
        public void ToTitleScene()
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}