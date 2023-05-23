using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The enemy spawn.
    public class EnemySpawn : MonoBehaviour
    {
        // The ID of the enemy to be spawned by this spawner.
        public Enemy.enemyId enemyId;

        // The position offset of the spawned enemy.
        public Vector3 posOffset = Vector3.zero;

        // Spawns an enemy at the spawner's location.
        public Enemy Spawn()
        {
            // TODO: implement.
            return null;
        }

    }
}