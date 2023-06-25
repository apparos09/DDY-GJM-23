using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    
    // A script for loading in audio credits.
    public class AudioCredits : MonoBehaviour
    {
        // An audio credit struct.
        public struct AudioCredit
        {
            // The name of the song.
            public string name;

            // The artist(s) that made the song.
            public string artist;

            // The collection the song is part of (album, EP, group, etc.).
            public string collection;

            // The source of the audio (website).
            public string source;

            // The primary link to the audio.
            public string link1;

            // The secondary link to the audio.
            public string link2;

            // The copyright information for the song. Use (\n) to seperate lines.
            public string copyright;
        }

        // The list of references.
        public List<AudioCredit> audioCredits = new List<AudioCredit>();

        // Start is called before the first frame update
        void Start()
        {
            // TODO: load in the audio references.
            // Title
            AudioCredit ac = new AudioCredit();
            ac.name = "Dawn of the Apocaylse";
            ac.artist = "Rafael Krux";
            ac.source = "FreePD";
            ac.link1 = "https://freepd.com/horror.php";
            ac.link2 = "https://music.orchestralis.net/track/28566414";

            ac.copyright = "\"Dawn of the Apocalypse\" by Rafael Krux (orchestralis.net)" +
                "\nLicensed under Creative Commons: By Attribution 4.0 International (CC BY 4.0)" +
                "\nhttps://creativecommons.org/licenses/by/4.0/";

            audioCredits.Add(ac);

            // Gameplay
            ac = new AudioCredit();
            ac.name = "Mysterious Lights";
            ac.artist = "Bryan Teoh";
            ac.source = "FreePD";
            ac.link1 = "https://freepd.com/horror.php";
            ac.link2 = "https://www.bryanteoh.com/";

            ac.copyright = "Mysterious Lights\" by Bryan Teoh" +
                "\nLicensed under Creative Commons: CC0 1.0 Universal (CC0 1.0) Public Domain Dedication" +
                "\nhttps://creativecommons.org/publicdomain/zero/1.0/";

            audioCredits.Add(ac);

            // Results
            ac = new AudioCredit();
            ac.name = "No Winners";
            ac.artist = "Ross Bugden";
            ac.source = "GameSounds.xyz, YouTube";
            ac.link1 = "https://gamesounds.xyz/?dir=Music%20-%20Ross%20Bugden";
            ac.link2 = "https://www.youtube.com/watch?v=9qk-vZ1qicI";

            ac.copyright = "\"No Winners\" by Ross Bugden (https://youtu.be/9qk-vZ1qicI)" +
                "\nConfirmed to be free to copy, modify, distribute, and perform for work, even for commercial purposes, all without asking permission.";

            audioCredits.Add(ac);
        }

        // Returns the page count for the audio references.
        public int GetCreditCount()
        {
            return audioCredits.Count;
        }

        // Checks if the index is in the page range.
        public bool IndexInBounds(int index)
        {
            return (index >= 0 && index < audioCredits.Count);
        }

        // Returns a credit from the list.
        public AudioCredit GetCredit(int index)
        {
            // Returns the requested audio credit, or a blank one if out of bounds.
            if (IndexInBounds(index))
                return audioCredits[index];
            else
                return new AudioCredit();
        }
    }
}