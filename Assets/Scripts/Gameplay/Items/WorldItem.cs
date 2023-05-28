using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // An item in the game world.
    public abstract class WorldItem : AreaEntity
    {
        // Item identification.
        public enum itemId { none, scrap, weapon, key, health };

        // The id of the world item.
        public itemId id;

        // Destroys the item upon being gotten by the player.
        [Tooltip("Destroys the item upon it being received if true. Disables the object if false.")]
        public bool destroyOnGet = true;

        // The timer for having items destroy themselves.
        [Header("Timer")]
        // The item timer.
        public float itemTimer = 0.0F;

        // The item timer max.
        public const float ITEM_TIMER_MAX = 10.0F;

        // If 'true', the item timer is used.
        public bool useTimer = false;

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Trigger2D - checks the trigger collision.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Only the player can pick things up.
            if(collision.gameObject.tag == Player.PLAYER_TAG)
            {
                // Give the player the item.
                GiveItem();   
            }
        }

        // // Returns the item ID.
        // public itemId Id
        // {
        //     get { return id; }
        // }


        // Called to give the item to the player.
        protected abstract void GiveItem();


        // Called when the item is gotten by the player.
        protected virtual void OnItemGet()
        {
            // Checks if the item should be destroyed upon being received.
            if(destroyOnGet)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // Sets the area the item belongs to.
        public void SetArea(WorldArea newArea)
        {
            // The area has been set.
            if (area != null)
            {
                // Remove from the list.
                if (area.items.Contains(this))
                    area.items.Remove(this);
            }

            // Set the area, and puts the item in the list.
            area = newArea;
            area.items.Add(this);
        }


        // Update is called once per frame
        protected virtual void Update()
        {
            // If the item timer should be used.
            if(useTimer)
            {
                // Time remaining.
                if(itemTimer > 0.0F)
                {
                    // Reduce timer.
                    itemTimer -= Time.deltaTime;

                    // Time over.
                    if(itemTimer <= 0.0F)
                    {
                        // The timer.
                        itemTimer = 0.0F;

                        // Destroy the item.
                        OnItemGet();
                    }

                }
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            
        }
    }
}