using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The gameplay audio.
    public class GameplayAudio : ManagerAudio
    {
        [Header("Gameplay")]

        // The gameplay manager
        public GameplayManager manager;

        // Audio source for sound effects that are meant to loop.
        public AudioSource sfxLoopSource;

        // The BGM normal pitch.
        public const float BGM_NORMAL_PITCH = 1.0F;

        // THE BGM fast pitch.
        public const float BGM_FAST_PITCH = 1.1F;

        [Header("Gameplay/Audio Clips")]
        // The button source effect.
        public AudioClip buttonSfx;

        // The slider source effect.
        public AudioClip sliderSfx;

        [Header("Gameplay/Audio Clips/Player")]
        
        // Player Weapon Switch
        public AudioClip playerWeaponSwitchSfx;

        // Player Punch
        public AudioClip playerPunchSfx;

        // Player Shot
        public AudioClip playerShotSfx;

        // Player Hurt
        public AudioClip playerHurtSfx;

        // Player Item Pickup
        public AudioClip playerItemGetSfx;

        // Player Item Use
        public AudioClip playerItemUseSfx;

        // Player Map
        public AudioClip playerMapSfx;

        // Player Death
        public AudioClip playerDeathSfx;

        [Header("Gameplay/Audio Clips/Enemies")]

        // Enemy Shot (Attack)
        public AudioClip enemyShotSfx;

        // Enemy Hurt
        public AudioClip enemyHurtSfx;

        [Header("Gameplay/Audio Clips/World")]
        // Rock/Stone Block
        public AudioClip blockBreakSfx;

        // Lock
        public AudioClip lockBlockUnlockSfx;

        // Portal
        public AudioClip portalSfx;

        // Minute Chime
        public AudioClip timeChimeSfx;

        // Seconds Remaining
        public AudioClip secondsLeftSfx;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Grabs the instance.
            if (manager == null)
                manager = GameplayManager.Instance;
        }

        // BGM //
        // Checks if the BGM is going at a normal speed.
        public bool IsBgmNormalSpeed()
        {
            bool result = bgmSource.pitch == BGM_NORMAL_PITCH;
            return result;
        }

        // Plays the BGM at normal speed.
        public void SetBgmNormalSpeed()
        {
            // Normal pitch.
            bgmSource.pitch = BGM_NORMAL_PITCH;
        }

        // Checks if the BGM is going at a fast speed.
        public bool IsBgmFastSpeed()
        {
            bool result = bgmSource.pitch == BGM_FAST_PITCH;
            return result;
        }

        // Sets the BGM to play fast.
        public void SetBgmFastSpeed()
        {
            // Faster pitch.
            bgmSource.pitch = BGM_FAST_PITCH;
        }


        // SFX //
        // UI/Normal

        // Plays the menu button SFX.
        public void PlayButtonSfx()
        {
            // Plays the button sound effect.
            sfxSource.PlayOneShot(buttonSfx);
        }

        // Re-uses the button sound effect.
        public void PlayToggleSfx()
        {
            PlayButtonSfx();
        }

        // Plays the menu slider SFX.
        public void PlaySliderSfx()
        {
            // Plays the slider sound effect.
            sfxSource.PlayOneShot(sliderSfx);
        }


        // GAMEPLAY //

        // Player
        // Player switchd weapons.

        public void PlayPlayerWeaponSwitchSfx()
        {
            sfxSource.PlayOneShot(playerWeaponSwitchSfx);
        }

        // Player punched
        public void PlayPlayerPunchSfx()
        {
            sfxSource.PlayOneShot(playerPunchSfx);
        }

        // Player shot a projectile
        public void PlayPlayerShotSfx()
        {
            sfxSource.PlayOneShot(playerShotSfx);
        }

        // Player took damage
        public void PlayPlayerHurtSfx()
        {
            sfxSource.PlayOneShot(playerHurtSfx);
        }

        // Player picked up item
        public void PlayPlayerItemGetSfx()
        {
            sfxSource.PlayOneShot(playerItemGetSfx);
        }

        // Player used item
        public void PlayPlayerItemUseSfx()
        {
            sfxSource.PlayOneShot(playerItemUseSfx);
        }

        // Player opened or closed map
        public void PlayPlayerMapSfx()
        {
            sfxSource.PlayOneShot(playerMapSfx);
        }

        // Plays the player's death sound.
        public void PlayPlayerDeathSfx()
        {
            sfxSource.PlayOneShot(playerDeathSfx);
        }


        // Enemy
        // Enemy shooting projectile
        public void PlayEnemyShotSfx()
        {
            sfxSource.PlayOneShot(enemyShotSfx);
        }

        // Enemy hurt
        public void PlayEnemyHurtSfx()
        {
            sfxSource.PlayOneShot(enemyHurtSfx);
        }

        // World
        // Block broken
        public void PlayBlockBreakSfx()
        {
            sfxSource.PlayOneShot(blockBreakSfx);
        }

        // Lock block unlocked
        public void PlayLockBlockUnlockSfx()
        {
            sfxSource.PlayOneShot(lockBlockUnlockSfx);
        }

        // Portal use
        public void PlayPortalSfx()
        {
            sfxSource.PlayOneShot(portalSfx);
        }

        // Minute chime
        public void PlayTimeChimeSfx()
        {
            sfxSource.PlayOneShot(timeChimeSfx);
        }

        // Seconds remaining
        public void PlaySecondsLeftSfx()
        {
            // This is set for sfxLoopSource.
            // sfxSource.PlayOneShot(secondsLeftSfx);
            sfxLoopSource.clip = secondsLeftSfx;
            sfxLoopSource.Play();
        }
    }
}