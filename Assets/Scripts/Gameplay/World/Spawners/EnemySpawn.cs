using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The enemy spawn.
    public class EnemySpawn : AreaSpawn
    {
        // The prefab of the enemy to be instantiated.
        public Enemy enemyPrefab;

        // The list of spawned enemies.
        public List<Enemy> spawnedEnemies = new List<Enemy>();

        // Spawns an enemy.
        public override void Spawn()
        {
            // No enemy to instantiate, so don't do anything.
            if (enemyPrefab == null)
                return;

            // Instantiates an enemy.
            Enemy enemy = Instantiate(enemyPrefab);

            // Give the enemy its position.
            enemy.transform.position = GetSpawnPosition();

            // Adds the enemy to the area.
            if(area != null)
                area.AddEnemyToArea(enemy);
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