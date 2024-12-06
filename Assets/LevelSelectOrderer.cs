using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectOrderer : MonoBehaviour
{
    public GameObject[] levelSelectButtons;
    // Start is called before the first frame update
    void Start()
    {
        //get every level select button in the scene
        levelSelectButtons = GameObject.FindGameObjectsWithTag("LevelSelectButton");

        //sort the level select buttons by level number
        for(int i = 0; i < levelSelectButtons.Length; i++){
            for(int j = 0; j < levelSelectButtons.Length - 1; j++){
                if(levelSelectButtons[j].GetComponent<LevelSelectButton>().GetLevelNumber() > levelSelectButtons[j+1].GetComponent<LevelSelectButton>().GetLevelNumber()){
                    GameObject temp = levelSelectButtons[j];
                    levelSelectButtons[j] = levelSelectButtons[j+1];
                    levelSelectButtons[j+1] = temp;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeselectAllButtons(LevelSelectButton button){
        foreach(GameObject b in levelSelectButtons){
            if(b != button.gameObject){
                b.GetComponent<LevelSelectButton>().SetIsSelected(false);
            }
        }
    }
}
