using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The portal - only transports player.
    public class Portal : AreaEntity
    {
        [Header("Portal")]

        // The gameplay manager.
        public GameplayManager gameManager;

        // The object used to make the destination. If set to null, offset is used as the end position.
        [Tooltip("The destination object. If set to null, offset is used as the destination.")]
        public GameObject destination;

        // Valid tags for the portal to use.
        public List<string> validTags = new List<string>() { Player.PLAYER_TAG };

        // The position offset.
        public Vector3 offset = Vector3.zero;

        // If 'true', the portal cancels the rigidbody's velocity of the object.
        public bool cancelVelocity = true;

        [Header("Portal/End Portal")]

        // The ending portal.
        public Portal endPortal;

        // Uses the ignore next list.
        [Tooltip("If 'true', the ignore next list is used. " +
            "If the offset makes the object not collide with the end portal, set this to false.")]
        public bool useIgnoreNextList = true;

        // If set to 'true', the end portal ignores the next player collision.
        // Use this to stop the end portal from triggering until the player gets off it.
        [Tooltip("A list of objects to ignore next time they collide with this portal.")]
        public List<GameObject> ignoreNextList = new List<GameObject>();

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // The gameplay manager is not set, so set it.
            if(gameManager == null)
                gameManager = GameplayManager.Instance;

            // If the end portal is null, and the destination is not null.
            if (endPortal == null && destination != null)
                endPortal = destination.GetComponent<Portal>();
        }

        // OnCollisionEnter2D 
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Checks if the object should be ignored.
            if(ignoreNextList.Contains(collision.gameObject))
            {
                ignoreNextList.Remove(collision.gameObject);
            }
            else
            {
                // Valid tag.
                if (validTags.Contains(collision.gameObject.tag))
                    Transport(collision.gameObject);
            } 
        }

        // OnTriggerEnter2D
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Checks if the object should be ignored.
            if (ignoreNextList.Contains(collision.gameObject))
            {
                ignoreNextList.Remove(collision.gameObject);
            }
            else
            {
                // Valid tag.
                if (validTags.Contains(collision.gameObject.tag))
                    Transport(collision.gameObject);
            }
        }

        // Transports the user.
        public void Transport(GameObject user)
        {
            // Nothing to transport.
            if (user == null || destination == null)
                return;

            // If the game manager is not set, grab the instance.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;

            // Get the base position.
            Vector3 destPos = ((destination != null) ? destination.transform.position : Vector3.zero) + offset;

            // Adds the object to the end portal's ignore next list. 
            if(endPortal != null && useIgnoreNextList)
            {
                endPortal.ignoreNextList.Add(user);
            }


            // If the velocity should be cancelled.
            if(cancelVelocity)
            {
                // If the user has a rigidbody, cancel the velocity. 
                Rigidbody2D rigidbody2d;

                // If the entity has a rigidbody, remove the velocity.
                if (user.TryGetComponent(out rigidbody2d))
                {
                    rigidbody2d.velocity = Vector2.zero;
                }
            }
            

            // Move to the destination.
            user.transform.position = destPos;

            // Activates all areas so that the triggers trigger.
            // If the world isn't optimized, all colliders will be on regardless, so activating all areas isn't needed.
            if(World.OPTIMIZE_WORLD)
                gameManager.world.ActivateAllAreas();

        }

    }
}