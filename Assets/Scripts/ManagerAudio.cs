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

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // BUTTONS
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

            // TOGGLES
            // Finds the toggle audios.
            ToggleAudio[] toggleAudios = FindObjectsOfType<ToggleAudio>();

            // Goes through each toggle.
            foreach (ToggleAudio toggleAudio in toggleAudios)
            {
                // Gives the SFX sources.
                if (toggleAudio.audioSource == null)
                {
                    toggleAudio.audioSource = sfxSource;
                }
            }

            // SLIDERS
            // Finds the button audios.
            SliderAudio[] sliderAudios = FindObjectsOfType<SliderAudio>();

            // Goes through each slider.
            foreach (SliderAudio sliderAudio in sliderAudios)
            {
                // Gives the SFX sources.
                if (sliderAudio.audioSource == null)
                {
                    sliderAudio.audioSource = sfxSource;
                }
            }
        }
    }
}
