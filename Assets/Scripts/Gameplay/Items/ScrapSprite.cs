using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A scrap sprite changer.
    public class ScrapSprite : MonoBehaviour
    {
        // The scrap item.
        public ScrapItem scrap;

        // The list of sprites.
        public List<Sprite> sprites = new List<Sprite>();

        // The amounts that align with each sprite.
        public List<int> amounts = new List<int>();

        // Start is called before the first frame update
        void Start()
        {
            // If the scrap isn't set, try to grab the component.
            if(scrap == null)
                scrap = GetComponent<ScrapItem>();

            // Set sprite by amount.
            SetSpriteByAmount();
        }

        // Sets the sprite by the index.
        public void SetSpriteByIndex(int index)
        {
            // Invalid index.
            if (index < 0 || index >= sprites.Count)
                return;

            // Sets the new sprite.
            scrap.renderer.sprite = sprites[index];
        }

        // Sets the sprite by the amount.
        public void SetSpriteByAmount()
        {
            // If no sprites are set, do nothing.
            if (sprites.Count == 0)
                return;

            // If the lists aren't the same length, do nothing.
            if (sprites.Count != amounts.Count)
                return;

            
            // Goes through all amounts.
            for(int i = 0; i < sprites.Count; i++)
            {
                // If the amounts are equal.
                if (scrap.scrapAmount == amounts[i])
                {
                    // Changes the sprite.
                    scrap.renderer.sprite = sprites[i];
                    break;
                }
            }
        }
    }
}