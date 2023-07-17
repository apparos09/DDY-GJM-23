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

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Grabs the instance.
            if (manager == null)
                manager = GameplayManager.Instance;
        }
    }
}