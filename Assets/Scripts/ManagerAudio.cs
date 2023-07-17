using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace DDY_GJM_23
{
    // The audio for the manager.
    public class ManagerAudio : MonoBehaviour
    {
        // The BGM source.
        public AudioSource bgmSource;

        // The SFX source.
        public AudioSource sfxSource;

        // The button source effect.
        public AudioClip buttonSfx;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Finds the button audios.
            ButtonAudio[] buttonAudios = FindObjectsOfType<ButtonAudio>();

            // Goes through each button.
            foreach (ButtonAudio buttonAudio in buttonAudios)
            {
                // Gives the SFX sources.
                if (buttonAudio.audioSource == null)
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
