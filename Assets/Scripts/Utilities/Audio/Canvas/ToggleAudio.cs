using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // Plays audio when clicking a toggle.
    public class ToggleAudio : MonoBehaviour
    {
        // The toggle this script is for.
        public Toggle toggle;

        // THe audio for the user inputs.
        public AudioSource audioSource;

        // The audio clip for the toggle.
        public AudioClip audioClip;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Moved here in case the toggle has not been set enabled before the game was closed.

            // Button not set.
            if (toggle == null)
            {
                // Tries to get the component.
                toggle = GetComponent<Toggle>();
            }

            // Add to the onValueChanged function.
            AddOnValueChanged();
        }

        // Add OnValueChanged Delegate
        public void AddOnValueChanged()
        {
            // If the toggle isn't set, return.
            if (toggle == null)
                return;

            // Listener for the toggle.
            toggle.onValueChanged.AddListener(delegate
            {
                OnValueChanged(toggle.isOn);
            });
        }

        // Remove OnValueChanged Delegate
        public void RemoveOnValueChanged()
        {
            // If the toggle isn't set, return.
            if (toggle == null)
                return;

            // Remove the listener for onValueChanged if the toggle has been set.
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(OnValueChanged);
            }
        }


        // Called when the toggle is clicked.
        private void OnValueChanged(bool isOn)
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