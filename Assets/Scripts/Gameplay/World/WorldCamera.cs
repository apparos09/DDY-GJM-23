using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace DDY_GJM_23
{
    // The world camera.
    public class WorldCamera : MonoBehaviour
    {
        // The camera this world camera is attached to.
        // Make sure the WorldCamera script and Camera scrpt are attached to the same object.
        public new Camera camera;

        // The gameplay manager.
        public GameplayManager gameManager;

        // If set to 'true', the camera follows the player.
        public bool followPlayer = true;

        [Header("Anchor")]

        // The anchor of the camera's position.
        // The camera has limits for how far it can move from the anchor.
        public GameObject anchor;

        // The maximum distance the camera can move away from the anchor on the x-axis.
        [Tooltip("The maximum x-distance the camera can move away from the anchor.")]
        public float anchorMaxDistanceX = 0.0F;

        // The maximum distance the camera can move away from the anchor on the y-axis.
        [Tooltip("The maximum y-distance the camera can move away from the anchor.")]
        public float anchorMaxDistanceY = 0.0F;

        // TODO: have limit to see if the player goes off screen.

        // The minmimum and maximum view size.
        [Tooltip("The minimum of the camera view size.")]
        public Vector2 viewSizeMin = Vector2.zero;

        [Tooltip("The maximum of the camera view size.")]
        public Vector2 viewSizeMax = Vector2.one;

        // Start is called before the first frame update
        void Start()
        {
            // Grab the camera component.
            if(camera == null)
                camera = GetComponent<Camera>();

            // Gets the game manager.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        // Returns the minimum world point.
        public Vector3 GetWorldCameraViewSizeMinWorld()
        {
            Vector3 worldPoint = camera.ViewportToWorldPoint(viewSizeMin);
            return worldPoint;
        }


        // Returns the maximum world point.
        public Vector3 GetWorldCameraViewSizeMaxWorld()
        {
            Vector3 worldPoint = camera.ViewportToWorldPoint(viewSizeMax);
            return worldPoint;
        }

        // Gets the camera center in world.
        public Vector2 GetWorldCameraCenterWorld()
        {
            // Grabs the min and max.
            Vector3 min = GetWorldCameraViewSizeMinWorld();
            Vector3 max = GetWorldCameraViewSizeMaxWorld();

            // Calculates the center.
            Vector3 center = (min + max) / 2.0F;

            // Returns the center.
            return center;
        }

        // SET POSITION //
        // Sets the camera position (z stays the same).
        public void SetCameraPosition(Vector2 xy)
        {
            Vector3 newPos = new Vector3(xy.x, xy.y, transform.position.z);
            transform.position = newPos;
        }

        // Sets the camera position.
        public void SetCameraPosition(float x, float y)
        {
            SetCameraPosition(new Vector2(x, y));
        }

        // Sets the camera position (x, y, z).
        public void SetCameraPosition(Vector3 newPos)
        {
            transform.position = newPos;
        }

        // Sets the camera position (x, y, z).
        public void SetCameraPosition(float x, float y, float z)
        {
            SetCameraPosition(new Vector3(x, y, z));
        }

        // Sets the camera to the player position.
        public void SetCameraToPlayerPositionXY()
        {
            SetCameraToPlayerPositionXY(Vector2.zero);
        }

        // Sets the camera to the player position, offset by the provided amount.
        public void SetCameraToPlayerPositionXY(Vector2 offset)
        {
            // Calculate position.
            Vector3 newPos = gameManager.player.transform.position + new Vector3(offset.x, offset.y, 0.0F);
            newPos.z = transform.position.z;
            
            // Set position.
            transform.position = newPos;
        }

        // Sets the camera to the player position (xyz)
        public void SetCameraToPlayerPositionXYZ()
        {
            SetCameraToPlayerPositionXY(Vector2.zero);
        }

        // Sets the camera to the player position (xyz)
        public void SetCameraToPlayerPositionXYZ(Vector3 offset)
        {
            // Calculate position.
            Vector3 newPos = gameManager.player.transform.position + offset;

            // Set position.
            transform.position = newPos;
        }


        // LateUpdate is called every frame, if the Behaviour is enabled.
        private void LateUpdate()
        {
            // If the camera should follow the player.
            if(followPlayer)
            {
                // Gets the player.
                Player player = gameManager.player;

                // Checks if the anchor exists.
                if (anchor != null) // Set
                {
                    // The new position.
                    Vector3 newPos = anchor.transform.position;

                    // The x-position.
                    newPos.x = Mathf.Abs(player.transform.position.x - anchor.transform.position.x) < anchorMaxDistanceX ?
                        player.transform.position.x :
                        newPos.x + anchorMaxDistanceX;

                    // The y-position.
                    newPos.y = Mathf.Abs(player.transform.position.y - anchor.transform.position.y) < anchorMaxDistanceY ?
                        player.transform.position.y :
                        newPos.y + anchorMaxDistanceY;

                    // Set the camera's new position.
                    SetCameraPosition(newPos.x, newPos.y);
                }
                else // Not set
                {
                    // Set the camera to the player's position (ignore z).
                    SetCameraToPlayerPositionXY();
                }
            }
        }
    }
}