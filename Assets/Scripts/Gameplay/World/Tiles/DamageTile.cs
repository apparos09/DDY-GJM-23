using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A tile that causes damage to the entities on it.
    public class DamageTile : WorldTile
    {
        [Header("Damage Tile")]

        // Applies damage when an entity is on the tile.
        public bool applyDamage = true;

        // Instantly kills entities on the tile.
        [Tooltip("Instantly kills combatants that go on this tile. This only applies if 'applyDamage' is true.")]
        public bool instantKill = false;

        // The amount of damage applied by this tile.
        public float damageAmount = 1.0F;

        // Gets the damage amount.
        public float GetDamage()
        {
            return damageAmount;
        }
    }
}