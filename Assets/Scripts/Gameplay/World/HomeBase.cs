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

        // The number of visits the player's made to the base.
        public int visits = 0;

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
                // Increase the visits count.
                visits++;

                // Add to the scrap total.
                gameManager.scrapsTotal += gameManager.player.scrapCount;
                gameManager.player.scrapCount = 0;

                // Heal player and restore uses.
                gameManager.player.SetHealthToMax();
                gameManager.player.RestoreAllWeaponUsesToMax();
            }
        }

        //// OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
        //private void OnTriggerExit2D(Collider2D collision)
        //{
            
        //}

        // Checks if the player is in the base (white) area.
        public bool IsPlayerInBase()
        {
            bool result;

            // Old (Removed)
            // result = gameManager.player.collider.IsTouching(collider);

            // New - checks sector category. If white (base), then the player is in the base.
            result = gameManager.world.GetCurrentArea().sector == WorldArea.worldSector.white;

            return result;
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}