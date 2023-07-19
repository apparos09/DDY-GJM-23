using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // Adds audio to a slider.
    public class SliderAudio : MonoBehaviour
    {
        // The slider this script is for.
        public Slider slider;

        // THe audio for the user inputs.
        public AudioSource audioSource;

        // The audio clip for the toggle.
        public AudioClip audioClip;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Moved here in case the slider has not been set enabled before the game was closed.

            // Button not set.
            if (slider == null)
            {
                // Tries to get the component.
                slider = GetComponent<Slider>();
            }

            // Add to the onValueChanged function.
            AddOnValueChanged();
        }

        // Add OnValueChanged Delegate
        public void AddOnValueChanged()
        {
            // If the slider isn't set, return.
            if (slider == null)
                return;

            // Listener for the tutorial toggle.
            slider.onValueChanged.AddListener(delegate
            {
                OnValueChanged(slider.value);
            });
        }

        // Remove OnValueChanged Delegate
        public void RemoveOnValueChanged()
        {
            // If the slider isn't set, return.
            if (slider == null)
                return;

            // Remove the listener for onValueChanged if the slider has been set.
            if (slider != null)
            {
                slider.onValueChanged.RemoveListener(OnValueChanged);
            }
        }


        // Called when the slider has been changed.
        private void OnValueChanged(float value)
        {
            if (audioSource != null && audioClip != null)
                audioSource.PlayOneShot(audioClip);
        }

        // Script is destroyed.
        private void OnDestroy()
        {
            RemoveOnValueChanged();
        }
    }
}