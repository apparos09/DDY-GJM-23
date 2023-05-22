using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A combat entity in the game world.
    public class CombatEntity : MonoBehaviour
    {
        // The rigidbody for the player.
        public new Rigidbody2D rigidbody;

        // The player's collider.
        public new BoxCollider2D collider;

        // The player's health.
        public float health = 100.0F;

        // The player's max health.
        public float maxHealth = 100.0F;

        // Gets set to 'true' if the entity is killable.
        public bool killable = true;

        // The tile the combat entity is currently on.
        public FloorTile currentTile;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}