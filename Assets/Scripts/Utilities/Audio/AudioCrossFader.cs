using UnityEditor.Animations;
using UnityEngine;

namespace util
{
    // Loops a piece of audio using a cross-fade.
    public class AudioCrossFader : AudioSegmentLooper
    {
        [Header("Cross Fade")]

        // The main audio fade. This is the one that plays the audio.
        [Tooltip("The source of the audio. The audio source for mainFade should be the same one set for this component.")]
        public AudioFader mainFade;

        // The transition fade. This is the object used to perform the crossfade.
        // Make sure it has the same audio settings as the main fade's audio source.
        [Tooltip("The object used to perform the crossfade. Make sure it has the same settings as mainFade")]
        public AudioFader transitionFade;

        // The fade length. This overrides the fade lengths of the set fades.
        [Tooltip("The cross fade duration. This overrides the fade durations of mainFade and transitionFade.")]
        public float fadeDuration = 5.0F;

        // Start is called before the first frame update
        protected override void Start()
        {
            // Implements the main fade's audio source as the main cross fader audio source.
            if (audioSource == null)
                audioSource = mainFade.audioSource;

            // The transition shouldn't loop.
            transitionFade.audioSource.loop = false;

            // Stop the audio when the transition fade ends.
            transitionFade.stopOnFadeOut = true;

            // Call the base start function.
            base.Start();
        }

        // Called to loop the clip back to its start.
        protected override void OnLoopClip()
        {
            // Make sure the transition fade has the clip from the main fade.
            transitionFade.audioSource.clip = mainFade.audioSource.clip;

            // Set the fade durations.
            mainFade.fadeDuration = fadeDuration;
            transitionFade.fadeDuration = fadeDuration;

            // Set the trasition fade to the current audio time.
            // Note that the time won't be set if the transition fade is not currently playing.
            // As such, I need to play it, set the time, then pause it.
            transitionFade.audioSource.Play();
            transitionFade.audioSource.time = audioSource.time; // Doesn't use clip end.
            transitionFade.audioSource.Pause();

            // Set the main audio source to the clip start.
            // These two should be the same audio source.

            // Checks if the loop should be relative to where the audio currently is...
            // Versus what clipStart and clipEnd are set to.
            if (loopRelative)
            {
                // Calculates how much clipStart should be offset by.
                float offsetStart = audioSource.time - clipEnd;

                // Set current clip start as clipStart adjusted by the offset amount.
                float currClipStart = clipStart + offsetStart;


                // If the current clip start is negative (i.e., it's before the start of the audio itself)...
                // Then use normal clipStart.
                if (currClipStart >= 0)
                {
                    audioSource.time = currClipStart;
                    mainFade.audioSource.time = currClipStart;
                }
                else
                {
                    audioSource.time = clipStart;
                    mainFade.audioSource.time = clipStart;
                }
            }
            else
            {
                audioSource.time = clipStart;
                mainFade.audioSource.time = clipStart;
            }

            // Play the audio, fading in the main fade, and fading out for the transition.
            mainFade.FadeIn();
            transitionFade.FadeOut();

            // If the main fade or main audio source should not loop, pause all of the audio sources.
            if(!audioSource.loop || !mainFade.audioSource.loop)
            {
                audioSource.Pause();
                mainFade.audioSource.Pause();
                transitionFade.audioSource.Pause();
            }
        }
    }
}