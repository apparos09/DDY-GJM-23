using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

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

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the game settings instance.
            if (settings == null)
                settings = GameSettings.Instance;
        }

        // VOLUME

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

        // Sets if the game is muted or not.
        public void SetMute(Toggle toggle)
        {
            settings.audioControls.Mute = toggle.isOn;
        }

        // Setsm ute using the mute toggle.
        public void SetMute()
        {
            SetMute(muteToggle);
        }
    }
}