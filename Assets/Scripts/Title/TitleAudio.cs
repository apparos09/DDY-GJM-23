using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace DDY_GJM_23
{
    // Manages audio for the title screen.
    public class TitleAudio : MonoBehaviour
    {
        // The title manager.
        public TitleManager manager;

        [Header("BGM")]

        // The BGM source.
        public AudioSource bgmSource;

        [Header("SFX")]

        // The SFX source.
        public AudioSource sfxSource;

        // The button source effect.
        public AudioClip buttonSfx;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the instance if it's not set.
            if (manager == null)
                manager = TitleManager.Instance;

            // Finds the button audios.
            ButtonAudio[] buttonAudios = FindObjectsOfType<ButtonAudio>();

            // Goes through each button.
            foreach(ButtonAudio buttonAudio in buttonAudios) 
            {
                // Gives the SFX sources.
                if(buttonAudio.audioSource == null)
                {
                    buttonAudio.audioSource = sfxSource;
                }
            }
        }

        // Plays the menu button SFX.
        public void PlayButtonSfx()
        {
            // Plays the button sound effect.
            sfxSource.PlayOneShot(buttonSfx);
        }
    }
}
