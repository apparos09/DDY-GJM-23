using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A world area.
    public class WorldArea : MonoBehaviour
    {
        // The collider for entering and exiting the area.
        public BoxCollider2D areaCollider;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Called when entering the area trigger.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player;

            // Gets the component.
            if(collision.TryGetComponent<Player>(out player))
            {
                // ... Move camera.
            }
        }

        // Get the camera position based on the world area.
        public Vector3 GetAreaCenter()
        {
            // Gets the center of the area in world space.
            Vector3 areaCenter = transform.TransformPoint(areaCollider.bounds.center);

            return areaCenter;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}