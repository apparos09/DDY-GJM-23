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

        // The limits for the camera.
        public Vector2 cameraLimitsMin;
        public Vector2 cameraLimitsMax;

        // Start is called before the first frame update
        void Start()
        {
            // Save the instance.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;

            // Gets the box collider 2D.
            if (areaCollider == null)
                areaCollider = GetComponent<BoxCollider2D>();
        }

        // Called when entering the area trigger.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Gets the component.
            if(collision.gameObject.tag == Player.PLAYER_TAG)
            {
                // Set current area.
                if(!gameManager.world.IsCurrentArea(this))
                {
                    // Set the current area.
                    gameManager.world.SetCurrentArea(this);
                }
            }
        }

        // Called when an area is being opened.
        public void OnAreaEnter()
        {


            CalculateCameraLimits();
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

        // Calculates the camera limits.
        public void CalculateCameraLimits()
        {
            // TODO: check if the calculations are accurate.

            // The minimum and maximum of the area colliders.
            Vector3 areaColMin = transform.TransformPoint(areaCollider.bounds.min);
            Vector3 areaColMax = transform.TransformPoint(areaCollider.bounds.max);

            // Calculates the camera points (relative to world origin).
            // Camera center (in world space).
            Vector3 camCenter = gameManager.camera.ScreenToWorldPoint(
                new Vector3(gameManager.camera.pixelWidth / 2.0F, gameManager.camera.pixelHeight / 2.0F, 0));

            // // Camera maximum (in world space).
            // Vector3 camMax = gameManager.camera.ScreenToWorldPoint(
            //     new Vector3(gameManager.camera.pixelWidth , gameManager.camera.pixelHeight, 0));
            // 
            // // Camera minimum (in world space).
            // Vector3 camMin = gameManager.camera.ScreenToWorldPoint(Vector3.zero);


            // Calculates the camera minimum and maximum.
            cameraLimitsMin = areaColMin + camCenter;
            cameraLimitsMax = areaColMax - camCenter;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}