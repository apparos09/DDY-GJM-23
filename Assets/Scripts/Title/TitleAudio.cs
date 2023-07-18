using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace DDY_GJM_23
{
    // Manages audio for the title screen.
    public class TitleAudio : ManagerAudio
    {
        [Header("Title")]

        // The results manager
        public TitleManager manager;

        // The button source effect.
        public AudioClip buttonSfx;

        // The slider source effect.
        public AudioClip sliderSfx;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Grabs the instance.
            if (manager == null)
                manager = TitleManager.Instance;
        }

        // Plays the menu button SFX.
        public void PlayButtonSfx()
        {
            // Plays the button sound effect.
            sfxSource.PlayOneShot(buttonSfx);
        }

        // Plays the menu slider SFX.
        public void PlaySliderSfx()
        {
            // Plays the slider sound effect.
            sfxSource.PlayOneShot(sliderSfx);
        }
    }
}
