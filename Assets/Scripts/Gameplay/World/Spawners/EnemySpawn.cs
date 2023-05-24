using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The enemy spawn.
    public class EnemySpawn : AreaSpawn
    {
        // The ID of the enemy to be spawned by this spawner.
        public Enemy.enemyId enemyId;

        // The position offset of the spawned enemy.
        public Vector3 posOffset = Vector3.zero;

        // The list of spawned enemies.
        public List<Enemy> spawnedEnemies = new List<Enemy>();

        // Spawns an enemy.
        public override void Spawn()
        {
            // Instantiates an enemy.
            Enemy enemy = EnemyPrefabs.Instance.InstantiateEnemyByType(enemyId);

            // Give the enemy its position.
            enemy.transform.position = transform.position + posOffset;

            // Set the enemy's spawn.
            enemy.AddToSpawn(this);
        }

        // Destroys all spawned enemies.
        public void DestroyAllSpawnedEnemies()
        {
            // Goes through each enemy - Goes backwards through the list to avoid errors.
            for(int i = spawnedEnemies.Count - 1; i >= 0; i--) 
            {
                // Destroys the enemy, which also removes it from the spawn list.
                if (spawnedEnemies[i] != null)
                    Destroy(spawnedEnemies[i]);
            }

            // Clears out the enemies.
            spawnedEnemies.Clear();


        }

    }
}