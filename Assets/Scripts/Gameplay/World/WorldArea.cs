using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A world area.
    public class WorldArea : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        // The collider for entering and exiting the area.
        public BoxCollider2D areaCollider;

        // Start is called before the first frame update
        void Start()
        {
            // Save the instance.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        // Called when entering the area trigger.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player;

            // Gets the component.
            if(collision.TryGetComponent<Player>(out player))
            {
                // Set current area.
                if(gameManager.world.IsCurrentArea(this))
                {
                    // Set the current area.
                    gameManager.world.SetCurrentArea(this);
                }
            }
        }

        // Called when an area is being opened.
        public void OnAreaEnter()
        {

        }

        // Called when an area is being closed.
        public void OnAreaExit()
        {

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