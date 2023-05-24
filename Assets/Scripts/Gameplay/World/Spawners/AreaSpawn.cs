using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // Spawns elements when an area is entered.
    public abstract class AreaSpawn : AreaEntity
    {
        // Start is called before the first frame update
        private void Start()
        {
            // If the spawner's area has not been set, try to search for it in the parent.
            if (area == null)
                area = GetComponentInParent<WorldArea>(true);
        }

        // TriggerEnter2D - check what area the spawner is in.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // The area hasn't been set.
            if(area == null)
            {
                // Tries to grab the area component.
                WorldArea colArea;

                if (collision.TryGetComponent(out colArea))
                {
                    // Sets the area.
                    area = colArea;
                }
            }
        }

        // Call to spawne the entity tied to this spawner.
        public abstract void Spawn();
    }
}