using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDY_GJM_23
{
    // The gameplay map.
    public class GameplayMap : MonoBehaviour
    {
        // The player marker
        public Image playerMarker;

        // The top left corner of the map, which is considered [0, 0] on the array.
        [Tooltip("The position on the map array for index (0, 0).")]
        public Vector2 cell0_0 = new Vector2(0, 0);

        // The offset for moving something along the map.
        [Tooltip("The offset for moving something along the map.")]
        public Vector2 offset = new Vector2(1.0F, 1.0F);

        // The movement direction for increasing the 
        [Tooltip("The offset direction for increasing the row and colum")]
        public Vector2 offsetDirec = new Vector2(1, -1);


        // This function is called when the object becomes enabled and active
        public void OnEnable()
        {
            PlacePlayerMarker();
        }

        // Places the player marker on the map. (TODO: NOT GETTING CALLED FOR SOME REASON).
        public void PlacePlayerMarker()
        {
            // Set player marker to zero pos.
            playerMarker.transform.localPosition = Vector3.zero;

            // Gets the final position, starting off relative to cell0_0.
            Vector3 finalPos = cell0_0;

            // Gets the cell.
            int[] cell = GameplayManager.Instance.world.GetCurrentWorldMapCell();

            // Calculates the final position.
            // Remember that row (0) = y, and col(1) = x.
            finalPos.x += offsetDirec.x * offset.x * cell[1];
            finalPos.y += offsetDirec.y * offset.y * cell[0];

            // Set local position of the player marker.
            playerMarker.transform.localPosition = finalPos;
        }
    }
}