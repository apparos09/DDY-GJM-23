using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DDY_GJM_23
{
    // The gameplay map.
    public class GameplayMap : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager = null;

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

        [Header("Other")]

        // The count for the scraps text.
        public TMP_Text scrapStatsText;

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            // Set the game manager.
            if(gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        // This function is called when the object becomes enabled and active
        public void OnEnable()
        {
            PlacePlayerMarker();
            UpdateScrapDisplay();
        }

        // Place the provided marker using the current world map cell.
        private void PlaceMarker(Image marker)
        {
            // Set  marker to zero pos.
            marker.transform.localPosition = Vector3.zero;

            // Gets the final position, starting off relative to cell0_0.
            Vector3 finalPos = cell0_0;

            // Gets the cell.
            int[] cell = GameplayManager.Instance.world.GetCurrentWorldMapCell();

            // Calculates the final position.
            // Remember that row (0) = y, and col(1) = x.
            finalPos.x += offsetDirec.x * offset.x * cell[1];
            finalPos.y += offsetDirec.y * offset.y * cell[0];

            // Set local position of the player marker.
            marker.transform.localPosition = finalPos;
        }


        // Places the player marker on the map.
        public void PlacePlayerMarker()
        {
            PlaceMarker(playerMarker);
        }

        // Updates the scrap display.
        public void UpdateScrapDisplay()
        {
            // If the game manager isn't set, set it.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;

            // If the game player isn't set.
            if (gameManager.player == null)
                gameManager.player = FindObjectOfType<Player>(true);

            // Set the text.
            scrapStatsText.text = 
                gameManager.player.scrapCount.ToString() + " | " + gameManager.scrapTotal.ToString();
        }

        // Update is called every frame, if the MonoBehaviour is enabled
        private void Update()
        {
            // Gets the gameplay manager.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;

            // Closes the map.
            if(Input.GetKeyDown(gameManager.player.mapKey))
            {
                // Closes the game map.
                gameManager.CloseMap();
            }
        }
    }
}