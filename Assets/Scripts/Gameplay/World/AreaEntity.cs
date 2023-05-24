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
    }
}