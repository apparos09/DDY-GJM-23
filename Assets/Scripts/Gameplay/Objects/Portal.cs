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

        // The object used to makr the destinati
        public GameObject destination;

        // Valid tags for the portal to use.
        public List<string> validTags = new List<string>() { Player.PLAYER_TAG };

        // The position offset.
        public Vector3 offset = Vector3.zero;

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // The gameplay manager is not set, so set it.
            if(gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        // OnCollisionEnter2D 
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Valid tag.
            if(validTags.Contains(collision.gameObject.tag))
                Transport(collision.gameObject);   
        }

        // OnTriggerEnter2D
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Valid tag.
            if (validTags.Contains(collision.gameObject.tag))
                Transport(collision.gameObject);
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

            // Move to the destination.
            user.transform.position = destination.transform.position + offset;

            // Activates all areas so that the triggers trigger.
            // FIXME: this could probably be optimized.
            gameManager.world.ActivateAllAreas();

        }

    }
}