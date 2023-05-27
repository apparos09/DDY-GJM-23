using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The home base for the game world.
    public class HomeBase : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        // The collider for the home base.
        public new CircleCollider2D collider;

        // Start is called before the first frame update
        void Start()
        {
            // The game manager hasn't been set.
            if(gameManager == null)
                gameManager = GameplayManager.Instance;

            // Gets the box collider.
            if(collider == null)
                collider = GetComponent<CircleCollider2D>();
        }

        // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If the player has entered the base.
            if(collision.tag == Player.PLAYER_TAG)
            {
                // TODO: maybe do it one-by-one instead of all at once?

                // Add to the scrap total.
                gameManager.scrapsTotal += gameManager.player.scrapCount;
                gameManager.player.scrapCount = 0;

                // Heal player.
                gameManager.player.SetHealthToMax();
            }
        }

        //// OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
        //private void OnTriggerExit2D(Collider2D collision)
        //{
            
        //}

        // Checks if the player is in the base.
        public bool IsPlayerInBase()
        {
            bool result = gameManager.player.collider.IsTouching(collider);
            return result;
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}