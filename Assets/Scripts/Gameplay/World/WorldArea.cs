using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A world area.
    public class WorldArea : MonoBehaviour
    {
        // Shows what sector this world area belongs to.
        // Sector 'white' doubles as the 'homeBase' sector.
        public enum worldSector { unknown, white, red, blue, yellow, green}

        // The gameplay manager.
        public GameplayManager gameManager;

        // The collider for entering and exiting the area.
        public BoxCollider2D areaCollider;

        // The sector of this world area.
        public worldSector sector = worldSector.unknown;

        [Header("Camera")]
        // Determins if the camera is fixedo r mves.
        public bool fixedCamera = true;

        // The camera view target when focusing on the area (relative to world size).
        // (0.5, 0.5) means the middle of the area collider. 
        public Vector2 camViewTarget = new Vector2(0.5F, 0.5F);

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

            // Sets the camera position (don't change z).

            Vector3 camPos = GetAreaCenter();
            gameManager.worldCamera.SetCameraPosition(camPos.x, camPos.y);
        }

        // Called when an area is being closed.
        public void OnAreaExit()
        {

        }

        public Vector3 GetAreaMin()
        {
            // Gets the min of the area in world space.
            Vector3 areaMin = transform.TransformPoint(areaCollider.bounds.min);

            return areaMin;
        }

        // Gets the area max.
        public Vector3 GetAreaMax()
        {
            // Gets the max of the area in world space.
            Vector3 areaMax = transform.TransformPoint(areaCollider.bounds.max);

            return areaMax;
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

            // The area center.
            // Using area max and min seems to give weird numbers, so we're using center for now.
            Vector3 areaMin = GetAreaMin();
            Vector3 areaMax = GetAreaMax();
            
            // Calculates the camera target (which normally is the area center).
            Vector2 camTarget = new Vector2(
                Mathf.Lerp(areaMin.x, areaMax.x, camViewTarget.x),
                Mathf.Lerp(areaMin.y, areaMax.y, camViewTarget.y)
                );

            // OLD - maybe change to use viewport instead?
            // Calculates the camera points (relative to world origin).
            // Camera center (in world space).
            Vector3 camMin = gameManager.worldCamera.GetWorldCameraViewSizeMinWorld();
            Vector3 camMax = gameManager.worldCamera.GetWorldCameraViewSizeMaxWorld();
            Vector3 camCenter = gameManager.worldCamera.GetWorldCameraCenterWorld();


            // // NEW
            // // The orthographic size is the camera height divided by 2.
            // float camHeight = gameManager.camera.orthographicSize * 2.0F;
            // 
            // // The camera width is the aspect ratio times the half height.
            // float camWidth = gameManager.camera.orthographicSize * gameManager.camera.aspect * 2.0F;
            // 
            // // TODO: maybe put the camera height and width functions in a seperate script.
            // Vector3 camCenter = new Vector3(camWidth / 2, camHeight / 2, 0.0F);

            // // Camera maximum (in world space).
            // Vector3 camMax = gameManager.camera.ScreenToWorldPoint(
            //     new Vector3(gameManager.camera.pixelWidth , gameManager.camera.pixelHeight, 0));
            // 
            // // Camera minimum (in world space).
            // Vector3 camMin = gameManager.camera.ScreenToWorldPoint(Vector3.zero);


            // NOTE: make sure that the hitbox is big enough so that the camera limits work properly.

            // Calculates the camera minimum and maximum.
            Vector3 temp;
            
            // temp = areaMin + (camCenter - camMin); // OLD
            temp = new Vector3(camTarget.x, camTarget.y, 0) - (camCenter - camMin); // NEW
            cameraLimitsMin = new Vector2(temp.x, temp.y);

            // temp = areaMax - (camMax - camCenter); // OLD
            temp = new Vector3(camTarget.x, camTarget.y, 0) + (camMax - camCenter); // NEW
            cameraLimitsMax = new Vector2(temp.x, temp.y);
        }

        // Update is called once per frame
        void Update()
        {
            // The camera should track the player.
            if(!fixedCamera)
            {
                // Updates the world camera.
                WorldCamera worldCam = gameManager.worldCamera;

                if(!(worldCam.transform.position.x == gameManager.player.transform.position.x) && 
                    (worldCam.transform.position.y == gameManager.player.transform.position.y))
                {
                    // Sets the camera to the player's position (ignores the z-axis).
                    worldCam.SetCameraToPlayerPositionXY();

                    // Calculates the new position.
                    Vector3 newPos = new Vector3(
                        Mathf.Clamp(worldCam.transform.position.x, cameraLimitsMin.x, cameraLimitsMax.x),
                        Mathf.Clamp(worldCam.transform.position.y, cameraLimitsMin.y, cameraLimitsMax.y),
                        worldCam.transform.position.z);

                    // Sets the camera position.
                    worldCam.SetCameraPosition(newPos);
                }
                
            }
        }
    }
}