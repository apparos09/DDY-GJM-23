using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DDY_GJM_23
{
    // The results manager.
    public class ResultsManager : MonoBehaviour
    {
        // The results data.
        public ResultsData results;

        // [Header("UI")]


        // Start is called before the first frame update
        void Start()
        {
            // Finds the results data.
            if(results == null)
                results = FindObjectOfType<ResultsData>();

            // Results were found, so load them up.
            if (results != null)
                LoadResults();
        }

        // Loads the results.
        public void LoadResults()
        {
            // ...

            // Destroys the results game object.
            Destroy(results.gameObject);
        }

        // Loads the title scene.
        public void ToTitleScene()
        {
            SceneManager.LoadScene("TitleScene");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}