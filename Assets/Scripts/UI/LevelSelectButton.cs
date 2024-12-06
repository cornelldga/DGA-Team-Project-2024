using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
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

    void FixedUpdate(){
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

    //panning container explanation:
    //ok so, every button exists within a parent container. This container is by default at the position of 0,0,0
    //when a button is pressed, we want that button to be in the center of the screen. To do this, we need to move the whole container such that the button gets to 0,0,0
    //how do we do this?
    //we get the position of the button, and subtract it from the position of the container. This gives us the difference between the two
    //we then set the local position of the container to the difference
    //this will move the button to the center of the screen

    public void PanContainerTowardsDirectionOfButton(){
        //start the coroutine to pan the container towards the pressed button
        //we need a couroutine because we want to move the container over time, not instantly
        StartCoroutine(SmoothPan());
    }

    /// <summary>
    /// Smoothly pans the parent towards this button's position
    /// over a duration of 0.5 seconds using a smooth step interpolation, with the x-axis movement
    /// clamped between -310 and 310 units.
    /// 
    /// returns IEnumerator for use in a coroutine
    /// </summary>
    private IEnumerator SmoothPan(){
        GameObject parent = transform.parent.gameObject;
        RectTransform parentRectTransform = parent.GetComponent<RectTransform>();
        RectTransform buttonRectTransform = GetComponent<RectTransform>();
        Vector3 parentPosition = parentRectTransform.position;
        Vector3 buttonPosition = buttonRectTransform.position;
        Vector3 difference = parentPosition - buttonPosition;

        difference.x = Mathf.Clamp(difference.x, -310, 310);

        Vector3 targetPosition = difference;
        Vector3 startPosition = parentRectTransform.localPosition;

        //the elapsed time of the pan
        float elapsedTime = 0f;

        //the duration of the pan
        float duration = 0.5f;

        //while the elapsed time is less than the duration
        //we want to move the parent container towards the button
        while (elapsedTime < duration){
            //smooth step interpolation
            //it should be clarified that I didn't write this interpolation code, I have no idea how this works
            //I just know that it works
            //Source: ChatGPT
            float t = elapsedTime / duration;
            //t is the time variable, and the smooth step interpolation is a cubic function
            t = t * t * (3f - 2f * t);
            parentRectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            //increment the elapsed time
            elapsedTime += Time.deltaTime;
            //wait for the next frame
            yield return null;
        }
        //set the position to the target position to ensure that the position is exactly at the target position
        parentRectTransform.localPosition = targetPosition;
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
