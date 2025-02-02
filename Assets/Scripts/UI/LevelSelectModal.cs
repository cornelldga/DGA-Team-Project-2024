using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectModal : MonoBehaviour
{
    [SerializeField] private GameObject levelNumber;
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;
    private GameObject[] stars;
    [SerializeField] private GameObject bestTimeText;
    [SerializeField] private GameObject customesServedText;

    string levelName;

    void Awake(){
        stars = new GameObject[3];
        stars[0] = star1;
        stars[1] = star2;
        stars[2] = star3;
    }

    //set the level number, star count and description text
    public void SetLevelData(int levelNumber, int starCount, int bestTime, int customesServed,string levelName)
    {
        this.levelNumber.GetComponent<Text>().text = levelNumber.ToString();
        SetStarCount(starCount);
        //bestTime is in seconds, convert it to 0:00 format
        int minutes = bestTime / 60;
        int seconds = bestTime % 60;
        this.bestTimeText.GetComponent<TMP_Text>().text = minutes.ToString() + ":" + seconds.ToString("00");
        
        //TODO: Adjust the customers served placeholder text

        this.levelName = levelName;
        playUpAnimation();
    }

    //this is just for testing because we don't have save data yet
    public void SetLevelData(int levelNumber, string levelName){
        this.levelNumber.GetComponent<TMPro.TMP_Text>().text = levelNumber.ToString();
        SetStarCount(0);
        this.levelName = levelName;
        playUpAnimation();
    }

    private void SetStarCount(int starCount)
    {
        //based on the star count just hide the stars
        for (int i = 0; i < 3; i++)
        {
            if (i < starCount)
            {
                stars[i].SetActive(true);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
    }

    public void playUpAnimation()
    {
        //play the up animation
        this.GetComponent<Animator>().SetTrigger("Up");
    }

    public void playDownAnimation()
    {
        //play the down animation
        this.GetComponent<Animator>().SetTrigger("Down");
    }

    //when the play button is clicked, load the level
    public void OnPlayButtonClicked()
    {
        Debug.Log("Loading level: " + levelName);
        FindObjectOfType<AudioManager>().PlaySound("sfx_MenuClick");
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
}
