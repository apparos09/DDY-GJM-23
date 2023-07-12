using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;
using TMPro;
using Unity.VisualScripting;

namespace DDY_GJM_23
{
    // The user interface for the game settings.
    public class GameSettingsUI : MonoBehaviour
    {
        // The game settings.
        public GameSettings settings;

        [Header("Audio")]

        // The slider for the BGM volume.
        public Slider bgmSlider;

        // The slider for the SFX volume.
        public Slider sfxSlider;

        // The mute toggle.
        public Toggle muteToggle;

        // The tutorial toggle.
        public Toggle tutorialToggle;

        [Header("Screen Size")]

        // Dropdown for the screen size.
        public TMP_Dropdown screenSizeDropdown;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the game settings instance.
            if (settings == null)
                settings = GameSettings.Instance;


            // If the game is running in WebGL, disable the screen size changer.
            if(Application.platform == RuntimePlatform.WebGLPlayer)
            {
                screenSizeDropdown.interactable = false;
            }
            else
            {
                screenSizeDropdown.interactable = true;
            }
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            // Makes sure the settings is set.
            if(settings == null)
                settings = GameSettings.Instance;

            // Checks if the settings has audio controls attached. 
            if(settings.audioControls == null)
            {
                // Tries to get the component.
                if(!settings.TryGetComponent(out settings.audioControls))
                {
                    // Adds the audio controls component if it's not set.
                    settings.audioControls = AudioControls.Instance;
                }
            }
                
            // Set the slider and toggle values.
            bgmSlider.value = settings.audioControls.BackgroundMusicVolume;
            sfxSlider.value = settings.audioControls.SoundEffectVolume;
            muteToggle.isOn = settings.audioControls.Mute;


            // Checks the screen resolution.
            if(Screen.fullScreen)
            {
                // Set to full screen value.
                screenSizeDropdown.value = 3;
            }
            else
            {
                // Checks the current resolution (checks via height)
                switch (Screen.currentResolution.height)
                {
                    case 576: // 1024 X 576
                        screenSizeDropdown.value = 0;
                        break;

                    case 720: // 1280 X 720
                        screenSizeDropdown.value = 1;
                        break;

                    case 1080: // 1920 X 1080
                        screenSizeDropdown.value = 2;
                        break;
                }
            }
        }

        // AUDIO //

        // BGM
        // Sets the BGM volume.
        public void SetBgmVolume(Slider slider)
        {
            float volume = slider.value / slider.maxValue; 
            settings.audioControls.BackgroundMusicVolume = volume;
        }

        // Sets the BGM volume
        public void SetBgmVolume()
        {
            SetBgmVolume(bgmSlider);
        }

        // SFX
        // Sets the SFX volume.
        public void SetSfxVolume(Slider slider)
        {
            float volume = slider.value / slider.maxValue;
            settings.audioControls.SoundEffectVolume = volume;
        }

        // Sets the SFX volume
        public void SetSfxVolume()
        {
            SetSfxVolume(sfxSlider);
        }

        // MUTE
        // Sets if the game is muted or not.
        public void SetMute(Toggle toggle)
        {
            settings.audioControls.Mute = toggle.isOn;
        }

        // Sets mute using the mute toggle.
        public void SetMute()
        {
            SetMute(muteToggle);
        }

        // TUTORIAL
        // Sets tutorial using the tutorial toggle.
        public void SetTutorial(Toggle toggle)
        {
            settings.useTutorial = toggle.isOn;
        }

        // Sets tutorial using the tutorial toggle.
        public void SetTutorial()
        {
            SetTutorial(tutorialToggle);
        }


        // SCREEN SIZE //

        // Sets the screen size.
        public void SetScreenSize(TMP_Dropdown dropdown)
        {
            // Checks the value.
            switch(dropdown.value)
            {
                case 0: // 1024 X 576
                default:
                    SceneHelper.SetScreenSize1024x576();
                    break;

                case 1: // 1280 X 720
                    SceneHelper.SetScreenSize1280x720();
                    break;

                case 2: // 1920 X 1080
                    SceneHelper.SetScreenSize1920x1080();
                    break;

                case 3: // Fullscreen
                    SceneHelper.SetFullScreen(true);
                    break;
            }
        }

        // Sets the screen size.
        public void SetScreenSize()
        {
            SetScreenSize(screenSizeDropdown);
        }
    }
}