using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A gun that fires a single shot.
    public class GunSingle : Weapon
    {
        // The prefab for the bullet.
        public Bullet bulletPrefab;

        // TODO: set up bullet pool.
        // The pool for the bullets.
        // public Queue<Bullet> bulletPool;

        // // Awake is called when the script is being loaded
        // protected override void Awake()
        // {
        //     base.Awake();
        // }
        // 
        // // Start is called before the first frame update
        // protected override void Start()
        // {
        //     base.Start();
        // }

        // Use the weapon.
        public override void UseWeapon()
        {
            // TODO: use bullet pool.

            // Generates a new bullet.
            Bullet newBullet = Instantiate(bulletPrefab);

            // Give base position.
            newBullet.transform.position = owner.transform.position;

            // TODO: maybe change how bullet direction is set.
            // TODO: calculate the rotation properly for where the player should be facing.

            // Set bullet's rotation for its direction.
            // TODO: is this working?
            newBullet.transform.eulerAngles = new Vector3(0, 0, owner.GetFacingDirectionAsRotation());
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}