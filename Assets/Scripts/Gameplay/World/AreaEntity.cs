using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // An area entity - used to track eneties that belong to specific areas.
    public class AreaEntity : MonoBehaviour
    {
        // The area the enemy is part of.
        public WorldArea area;

        // The portal sprite.
        public new SpriteRenderer renderer;

        // The animator for the area entity (if applicable).
        public Animator animator;

        // If the animator isn't set.
        protected virtual void Start()
        {
            //// Tries to grab the area in the parent (probably won't work).
            //if(area == null)
            //    area = GetComponentInParent<WorldArea>(true);

            // The sprite
            if(renderer == null)
            {
                // Tries to get the component from the object.
                if(!TryGetComponent(out renderer))
                {
                    // If the renderer is still null, check the children.
                    renderer = GetComponentInChildren<SpriteRenderer>();
                }
            }                

            // The animator.
            if(animator == null)
                animator = GetComponent<Animator>();
        }
    }
}