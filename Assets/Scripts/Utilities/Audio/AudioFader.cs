using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fades in (or out) the audio.
namespace util
{
    public class AudioFader : MonoBehaviour
    {
        // The audio source.
        public AudioSource audioSource;

        // Gets set to 'true' when the audio is fading.
        private bool fading;

        // The fade direction (-1 = fade out, 1 = fade in).
        private int fadeDirec = 0;

        // If set to 'true', the audio stops when faded out.
        [Tooltip("If true, the audio source is stopped when a fade out is completed. It also resets the audio to its starting volume.")]
        public bool stopOnFadeOut = true;

        // If 'true', the volume is set back to its starting value whe a fade out is finished.
        [Tooltip("Resets the volume to its origina value when a fade out finishes.")]
        public bool resetVolumeOnFadeOutEnd = true;

        // The fade duration (in seconds).
        [Tooltip("The fade duration in seconds.")]
        public float fadeDuration = 5.0F;

        // The fade LERP components.
        private float fadeT = 0.0F;
        private float fadeStart = 0.0F;
        private float fadeEnd = 0.0F;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Fade in the audio.
        public void FadeIn()
        {
            // If the audio is already fading, don't do anything.
            // TODO: implement system to handle it. (What did I mean by this?)
            if (fading)
                return;

            fadeDirec = 1;

            fadeEnd = audioSource.volume;
            fadeStart = 0.0F;
            audioSource.volume = 0.0F;
            audioSource.Play();

            fading = true;

        }

        // Fade out the audio.
        public void FadeOut()
        {
            // If the audio is already fading, don't do anything.
            // TODO: implement system to handle it.
            if (fading)
                return;

            fadeDirec = -1;
            fadeStart = audioSource.volume;
            fadeEnd = 0.0F;
            audioSource.Play();

            fading = true;
        }

        // Returns the fade direction.
        public int GetFadeDirection()
        {
            return fadeDirec;
        }

        // Checks for fade in.
        public bool IsFadingIn()
        {
            return fading && fadeDirec > 0;
        }

        // Checks for fade out.
        public bool IsFadingOut()
        {
            return fading && fadeDirec < 0;
        }

        // Update is called once per frame
        void Update()
        {
            // Should be fading, and the audio is playing.
            if (fading && audioSource.isPlaying)
            {
                // If the fade direction is set.
                if (fadeDirec != 0)
                {
                    // Reduce the fade by using unscaledDeltaTime (this should not be affected by timeScale).
                    fadeT += Time.unscaledDeltaTime / fadeDuration;
                    fadeT = Mathf.Clamp01(fadeT);

                    // Set the volume.
                    audioSource.volume = Mathf.Lerp(fadeStart, fadeEnd, fadeT);

                    // If the transition has finished.
                    if (fadeT >= 1.0F)
                    {
                        
                        // Set fadeT to 0.
                        fadeT = 0.0F;

                        // If the audio is fading out.
                        if(IsFadingOut())
                        {
                            // If the audio should be stopped now that the fade out is done.
                            if (stopOnFadeOut)
                            {
                                // Stops the audio.
                                audioSource.Stop();
                            }

                            // Returns the volume to it's original setting.
                            if (resetVolumeOnFadeOutEnd)
                            {
                                audioSource.volume = fadeStart;
                            }
                        }

                        // No longer fading.
                        fading = false;

                    }
                }
            }
        }
    }
}