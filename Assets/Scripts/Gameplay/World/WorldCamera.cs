using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The world camera.
    public class WorldCamera : MonoBehaviour
    {
        // The camera this world camera is attached to.
        // Make sure the WorldCamera script and Camera scrpt are attached to the same object.
        public new Camera camera;

        // The target that the camera is aimed at.
        // TODO: determine how far the camera can move away from the target. 
        public GameObject target;

        // The minmimum and maximum view size.
        public Vector2 viewSizeMin = Vector2.zero;
        public Vector2 viewSizeMax = Vector2.one;

        // Start is called before the first frame update
        void Start()
        {
            // Grab the camera component.
            if(camera == null)
                camera = GetComponent<Camera>();
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
            Vector3 newPos = GameplayManager.Instance.player.transform.position + new Vector3(offset.x, offset.y, 0.0F);
            newPos.z = transform.position.z;
            
            // Set position.
            transform.position = newPos;
        }
    }
}