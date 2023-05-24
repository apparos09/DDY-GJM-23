using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // Item spawner.
    public class ItemSpawn : AreaSpawn
    {
        // The item to be spawned.
        public WorldItem itemPrefab;

        // The position offset when spawning the item.
        public Vector3 posOffset = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Spawns the item.
        public override void Spawn()
        {
            // The item prefab has not been set.
            if (itemPrefab == null)
                return;

            // Instantiates the item.
            WorldItem item = Instantiate(itemPrefab);

            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}