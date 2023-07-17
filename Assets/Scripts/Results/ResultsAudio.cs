using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // Audio for the results screen.
    public class ResultsAudio : ManagerAudio
    {
        // The results manager
        public ResultsManager manager;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Grabs the instance.
            if (manager == null)
                manager = ResultsManager.Instance;
        }

    }
}
