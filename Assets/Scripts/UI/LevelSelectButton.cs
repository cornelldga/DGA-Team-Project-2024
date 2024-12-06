using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    //need a reference to the level select modal
    public LevelSelectModal levelSelectModal;

    [SerializeField] private Sprite selectedTexture;
    private Sprite defaultTexture;


    //by default the level should be unlocked; this will be changed when save data is implemented
    [SerializeField] private Boolean isLocked = false;
    [SerializeField] private Boolean isSelected = false;
    [SerializeField] private string levelDescription;

    //the level number is just the number of the level that will be displayed
    [SerializeField] private int levelNumber;

    //the level index is the index of the level in the build settings
    [SerializeField] private int levelIndex;


    void Awake(){
        defaultTexture = GetComponent<UnityEngine.UI.Image>().sprite;
        //in the save file grab whether or not this level is locked
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isLocked){
            //change the color tone of the button to be slightly darker
            GetComponent<UnityEngine.UI.Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        //set this object's text to the level number
        // GetComponentInChildren<TMPro.TextMeshProUGUI>().text = levelNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //when the button is clicked, set the level data in the level select modal
    public void OnClick()
    {
        if(isLocked){
            return;
        }
        if(isSelected){
            //deselect the button
            isSelected = false;
            GetComponent<UnityEngine.UI.Image>().sprite = defaultTexture;
            //get the image component of this button, and set the texture to the 
        } else {
            FindObjectOfType<LevelSelectOrderer>().DeselectAllButtons(this);

            //select the button
            isSelected = true;
            GetComponent<UnityEngine.UI.Image>().sprite = selectedTexture;
        }
        //again, this is the incorrect SetLevelData method, but until I get save data I'm using this
        levelSelectModal.SetLevelData(levelNumber, levelIndex);

        FindAnyObjectByType<AudioManager>().PlaySound("click");
        PanContainerTowardsDirectionOfButton();
    }

    public int GetLevelNumber(){
        return levelNumber;
    }

    public Boolean GetIsSelected(){
        return isSelected;
    }

    public void PanContainerTowardsDirectionOfButton(){
        //container
        GameObject parent = transform.parent.gameObject;
        //container transform
        RectTransform parentRectTransform = parent.GetComponent<RectTransform>();
        //button transform
        RectTransform buttonRectTransform = GetComponent<RectTransform>();
        //container position
        Vector3 parentPosition = parentRectTransform.position;
        //button position
        Vector3 buttonPosition = buttonRectTransform.position;
        //difference between the two
        Vector3 difference = parentPosition - buttonPosition;
        //set the local position of the parent to the difference

        //careful, clamp the x to not exceed 310, or go below -310
        difference.x = Mathf.Clamp(difference.x, -310, 310);
        parentRectTransform.localPosition = difference;
    }

    public void SetIsSelected(Boolean isSelected){
        this.isSelected = isSelected;
        if(isSelected){
            GetComponent<UnityEngine.UI.Image>().sprite = selectedTexture;
        } else {
            GetComponent<UnityEngine.UI.Image>().sprite = defaultTexture;
        }
    }
}
