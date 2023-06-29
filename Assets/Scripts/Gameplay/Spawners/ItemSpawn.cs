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

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Spawns the item.
        public override void Spawn()
        {
            // The item prefab has not been set.
            if (itemPrefab == null)
                return;

            // Instantiates the item.
            WorldItem item = Instantiate(itemPrefab);

            // Activate item.
            item.gameObject.SetActive(true);

            // Set position.
            item.transform.position = GetSpawnPosition();

            // Set the area.
            if (area != null)
                area.AddItemToArea(item);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}