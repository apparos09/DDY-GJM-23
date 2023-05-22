using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A tile the player and enemies travel over.
    public class FloorTile : MonoBehaviour
    {
        // The collider for the floor tile.
        public new BoxCollider2D collider;

        // If friction is 0, then it causes no speed reduction.
        public float friction = 1.0F;

        // Start is called before the first frame update
        void Start()
        {

        }

        // OnTriggerStay is called once per frame for every Collider other that is touching the trigger.
        private void OnTriggerStay(Collider other)
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}