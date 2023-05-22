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