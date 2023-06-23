using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace util
{
    // The audio settings for the game. This was renamed from "AudioSettings" since Unity has a clas sof the same name.
    public class AudioControls : MonoBehaviour
    {
        // the instance of the audio settings.
        private static AudioControls instance;

        // Adjusts the audio levels when a new level is loaded.
        public bool adjustAudioOnLevelLoaded = true;

        // Audio Tags
        // Feel free to change these ta
        // The tag for BGM objects.
        public const string BGM_TAG = "BGM";

        // The volume for the background music.
        private float bgmVolume = 0.3F;

        // The tag for the SFX objects.
        public const string SFX_TAG = "SFX";

        // The volume for the sound effects.
        private float sfxVolume = 0.6F;

        // There is no "JNG" (jingle) volume since that wouldn't make sense.

        // The tag for voices.
        public const string VCE_TAG = "VCE";

        // Voice volume.
        private float vceVolume = 1.0F;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Checks for the instance.
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }

            // This object should not be destroyed.
            DontDestroyOnLoad(this);
        }

        // // Start is called before the first frame update
        // void Start()
        // {
        // }

        // Use instead of OnLevelWasLoaded, as said function has been depreciated.
        // This function is called when the object is enabled and active
        private void OnEnable()
        {
            // This is called if the object is enabled when the program starts running.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // Returns the instance of the audio settings.
        public static AudioControls Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioControls>(true);

                    // Generate new instance if an existing instance was not found.
                    if (instance == null)
                    {
                        // makes a new settings object.
                        GameObject go = new GameObject("(Singleton) Audio Settings");

                        // adds the instance component to the new object.
                        instance = go.AddComponent<AudioControls>();
                    }

                }

                // returns the instance.
                return instance;
            }
        } 

        // Is the audio muted?
        public bool Mute
        {
            get
            {
                // Checks if the audio is muted.
                return AudioListener.pause;
            }

            set
            {
                // Mutes/unmutes all audio.
                AudioListener.pause = value;
            }
        }

        // Setting the background volume.
        public float BackgroundMusicVolume
        {
            get
            {
                return bgmVolume;
            }

            set
            {
                // Adjusts the BGM volume and adjusts all the audio objects with the BGM tag.
                bgmVolume = Mathf.Clamp01(value);
                AdjustBackgroundMusicAudioLevels();
            }
        }

        // Setting the sound effect volume.
        public float SoundEffectVolume
        {
            get
            {
                return sfxVolume;
            }

            set
            {
                // Adjusts the SFX volume and adjusts all the audio objects with the SFX tag.
                sfxVolume = Mathf.Clamp01(value);
                AdjustSoundEffectAudioLevels();
            }
        }

        // Setting the voice volume.
        public float VoiceVolume
        {
            get
            {
                return vceVolume;
            }

            set
            {
                // Adjusts the VCE volume and adjusts all the audio objects with the VCE tag.
                vceVolume = Mathf.Clamp01(value);
                AdjustVoiceAudioLevels();
            }
        }

        // Called when a level is loaded - this function has been depreciated.
        // Use onSceneLoaded callback instead.
        // private void OnLevelWasLoaded(int level)
        // {
        //     // Adjusts the audio levels when a new level is loaded.
        //     if(adjustAudioOnLevelLoaded)
        //         AdjustAllAudioLevels(bgmVolume, sfxVolume, vceVolume);
        // }

        // Adjusts the audio source that's supplied through this function.
        // For this to work, it needs to have a usable tag and set source audio object.
        public void AdjustAudio(AudioSourceControl audio)
        {
            // checks which tag to use.
            if (audio.CompareTag(BGM_TAG)) // BGM
            {
                audio.SetVolumeAsPercentageOfMax(bgmVolume);
            }
            else if (audio.CompareTag(SFX_TAG)) // SFX
            {
                audio.SetVolumeAsPercentageOfMax(sfxVolume);
            }
            else if (audio.CompareTag(VCE_TAG)) // VCE (Voice)
            {
                audio.SetVolumeAsPercentageOfMax(vceVolume);
            }
            else // No recognizable tag.
            {
                Debug.LogAssertion("No recognizable audio tag has been set, so the audio can't be adjusted.");
            }
        }


        // Applies the audio levels by using the saved audio settings.
        public void AdjustAllAudioLevels()
        {
            AdjustAllAudioLevels(bgmVolume, sfxVolume, vceVolume);
        }

        // Adjusts all the audio levels.
        public void AdjustAllAudioLevels(float newBgmVolume, float newSfxVolume, float newVceVolume)
        {
            // Finds all the audio source controls.
            AudioSourceControl[] audios = FindObjectsOfType<AudioSourceControl>();

            // Saves the bgm, sfx, and tts volume objects.
            bgmVolume = Mathf.Clamp01(newBgmVolume);
            sfxVolume = Mathf.Clamp01(newSfxVolume);
            vceVolume = Mathf.Clamp01(newVceVolume);

            // Goes through each source.
            foreach (AudioSourceControl audio in audios)
            {
                // Adjusts the audio.
                AdjustAudio(audio);
            }

            // TODO: what was the point of this?
            // // changing the values.
            // bgmVolume = newBgmVolume;
            // sfxVolume = newSfxVolume;
            // ttsVolume = newTtsVolume;

        }

        // Adjust all audio levels with the provided tag.
        private void AdjustAllAudioLevelsWithTag(string audioTag)
        {
            // Finds objects with the right tag.
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(audioTag);

            // Goes through the tagged objects.
            foreach (GameObject tagged in taggedObjects)
            {
                // The audio control object.
                AudioSourceControl asc;

                // Tries to get the audio control object.
                if (tagged.TryGetComponent(out asc))
                {
                    // Adjusts the audio level.
                    AdjustAudio(asc);
                }
            }
        }

        // Adjust all BGM audio levels.
        public void AdjustBackgroundMusicAudioLevels()
        {
            AdjustAllAudioLevelsWithTag(BGM_TAG);
        }

        // Adjusts all BGM audio levels with new volume level.
        public void AdjustBackgroundMusicAudioLevels(float newVolume)
        {
            // Don't use the shorthand since it calls a function to adjust the audio levels anyway.
            bgmVolume = Mathf.Clamp01(newVolume);
            AdjustBackgroundMusicAudioLevels();
        }

        // Adjust all SFX audio levels.
        public void AdjustSoundEffectAudioLevels()
        {
            AdjustAllAudioLevelsWithTag(SFX_TAG);
        }

        // Adjusts all SFX audio levels with new volume level.
        public void AdjustSoundEffectAudioLevels(float newVolume)
        {
            // Don't use the shorthand since it calls a function to adjust the audio levels anyway.
            sfxVolume = Mathf.Clamp01(newVolume);
            AdjustSoundEffectAudioLevels();
        }

        // Adjust all TTS audio levels.
        public void AdjustVoiceAudioLevels()
        {
            AdjustAllAudioLevelsWithTag(VCE_TAG);
        }

        // Adjusts all TTS audio levels with new volume level.
        public void AdjustVoiceAudioLevels(float newVolume)
        {
            // Don't use the shorthand since it calls a function to adjust the audio levels anyway.
            vceVolume = Mathf.Clamp01(newVolume);
            AdjustVoiceAudioLevels();
        }

        // Called when the scene was loaded.
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Adjusts all the audio levels.
            AdjustAllAudioLevels(bgmVolume, sfxVolume, vceVolume);

            // Refreshes the game mute, since this caused problems before.
            Mute = Mute;
        }

        // This behaviour is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // Remove from the scene manager.
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

