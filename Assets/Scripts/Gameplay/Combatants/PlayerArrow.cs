using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The player arrow.
    public class PlayerArrow : MonoBehaviour
    {
        // The player.
        public Player player;

        // Auto-update.
        public bool autoUpdate = true;

        // Start is called before the first frame update
        void Start()
        {
            // Tries to find the player component.
            if (player == null)
                player = GetComponentInParent<Player>();
        }

        // Transforms the arrow for the facing direction.
        public void TransformArrow()
        {
            // Gets the rotation.
            float theta = player.GetFacingDirectionAsRotation();

            // If the angleh asn't changed, do nothing.
            if (gameObject.transform.eulerAngles.z == theta)
                return;

            // Gets the new rotation.
            Vector3 newRot = gameObject.transform.eulerAngles;
            newRot.z = theta;

            // Sets the new rotation.
            gameObject.transform.eulerAngles = newRot;
        }

        // Update is called once per frame
        void Update()
        {
            // If the arrow should be automatically updated.
            if(autoUpdate)
            {
                TransformArrow();
            }
        }
    }
}