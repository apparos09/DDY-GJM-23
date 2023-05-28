using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The portal - only transports player.
    public class Portal : MonoBehaviour
    {
        // The object used to makr the destinati
        public GameObject destination;

        // Valid tags for the portal to use.
        public List<string> validTags = new List<string>() { Player.PLAYER_TAG };

        // The position offset.
        public Vector3 offset = Vector3.zero;

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

            // Move to the destination.
            user.transform.position = destination.transform.position + offset;
        }

    }
}