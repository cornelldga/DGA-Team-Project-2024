using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private string[] levels;
    [SerializeField] private GameObject buttonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //for every level name in the levels array, try to create a button for it, and set the button's text to the level name
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = levels[i];
            //add a listener to the button that will load the level when the button is clicked
            //can't really do this yet because we don't have the levels in the build settings
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
