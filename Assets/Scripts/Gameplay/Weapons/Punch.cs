using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The punch weapon.
    public class Punch : Weapon
    {
        // The collider for the punch.
        public new BoxCollider2D collider;

        // Constructor
        public Punch(Player owner, BoxCollider2D collider) : base(weaponType.punch, owner)
        {
            // The punch is infinite use.
            infiniteUse = true;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Use the weapon.
        public override void UseWeapon()
        {
            // TODO: move collider relative to player, active it, and play attack animation.
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}