using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Provides controls for audio sources.
namespace util
{
    // NOTE: for this to work the tag must be either "BGM" or 'SFX".
    public class AudioSourceControl : MonoBehaviour
    {
        // The audio source this script applies to.
        public AudioSource audioSource;

        // The maximum volume of the audio source, which is it's audio level. This can be public since it gets clamped anyway.
        [Tooltip("The maximum volume for the audio source.")]
        [Range(0.0F, 1.0F)]
        public float maxVolume = 1.0F;

        // If 'true', the max volume is set to the value of the audio source on start up.
        [Tooltip("Sets the maixmum volume automatically to the audio source's default volume.")]
        public bool autoSetMaxVolume = true;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Added this to help with creating an audio source with source control through a script.
            // If the audio source wans't set then try to grab the component.
            if (audioSource == null)
                audioSource = gameObject.GetComponent<AudioSource>();

            // The max volume has not been set.
            if (autoSetMaxVolume)
                maxVolume = audioSource.volume;
        }

        // Start is called before the first frame update
        void Start()
        {
            // adjusts the audio.
            // GameSettings.Instance.AdjustAudio(this);
        }

        // called when the object becomes active.
        private void OnEnable()
        {
            // adjusts the audio levels.
            // GameSettings.Instance.AdjustAudio(this);
        }

        // The maximum volume variable.
        public float MaxVolume
        {
            get
            {
                maxVolume = Mathf.Clamp01(maxVolume); // Clamps the vlaue.
                return maxVolume; // Returns the value.
            }

            set
            {
                maxVolume = Mathf.Clamp01(maxVolume); // Clamps the value.
                                                      // AdjustToGameSettings(); // Adjusts the audio.
            }
        }

        // Gets the volume as a percentage of the max volume.
        public float GetVolumeAsPercentageOfMax()
        {
            float result = audioSource.volume / maxVolume;
            return result;
        }

        // Sets the volume on a 0-1 scale in reference to the max volume.
        public void SetVolumeAsPercentageOfMax(float percent)
        {
            // Get the new volume.
            float newVol = maxVolume * percent;
            newVol = Mathf.Clamp01(newVol);

            // Set the new volume.
            audioSource.volume = newVol;
        }

        
        // ADJUSTING AUDIO //

        // Adjusts all the audio source controls.
        public static void AdjustAllAudioSourceControls(float newVolume, bool includeInactive)
        {
            // Clamps the new volume.
            float newVol = Mathf.Clamp01(newVolume);

            // Finds all the audios.
            AudioSourceControl[] audios = FindObjectsOfType<AudioSourceControl>(includeInactive);

            // Adjusts the volumes.
            foreach(AudioSourceControl asc in audios)
            {
                asc.SetVolumeAsPercentageOfMax(newVol);
            }
        }

        // Adjusts all the audio source controls.
        public static void AdjustAudioSourceControlsByTag(float newVolume, string audioTag, bool includeInactive)
        {
            // Clamps the new volume.
            float newVol = Mathf.Clamp01(newVolume);

            // Finds all objects with the provided tag.
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(audioTag);

            // Adjusts the volumes.
            foreach (GameObject tagged in taggedObjects)
            {
                // Audio source control.
                AudioSourceControl asc;

                // Tries to get the audio source control component.
                if(tagged.TryGetComponent(out asc))
                {
                    // Set volume as a percentage of the audio source control's max.
                    asc.SetVolumeAsPercentageOfMax(newVol);
                }
            }
        }


        // // Update is called once per frame
        // void Update()
        // {
        //     
        // }
    }
}