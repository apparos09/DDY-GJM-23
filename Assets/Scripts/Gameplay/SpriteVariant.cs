using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DDY_GJM_23
{
    // Provides variants for the tile's sprite.
    public class SpriteVariant : MonoBehaviour
    {
        // THe tile's sprite.
        public SpriteRenderer sprite;

        // If 'true', the children are checked for the sprite if the sprite isn't already set.
        [Tooltip("Checks the children for the sprite if the sprite isn't already set.")]
        public bool checkChildrenForSprite = true;

        // The list of variants.
        public List<Sprite> variants;

        // Start is called before the first frame update
        void Start()
        {
            // Tries to set sprite normally.
            if (sprite == null)
                sprite = GetComponent<SpriteRenderer>();

            // Checks the children for the component.
            if(sprite == null && checkChildrenForSprite)
                sprite = GetComponentInChildren<SpriteRenderer>();

        }

        // Sets hte sprite to the variant using the provided index.
        public void SetSpriteToVariant(int index)
        {
            // Invalid index.
            if (index < 0 || index >= variants.Count)
                return;

            // Sprites the sprite.
            sprite.sprite = variants[index];
        }
    }
}