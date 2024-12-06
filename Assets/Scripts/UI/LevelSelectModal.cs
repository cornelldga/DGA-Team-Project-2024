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

    //the level index to go to when the play button is clicked
    //-1 means no level is selected
    private int levelIndex = -1;

    void Awake(){
        stars = new GameObject[3];
        stars[0] = star1;
        stars[1] = star2;
        stars[2] = star3;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //set the level number, star count and description text
    public void SetLevelData(int levelNumber, int starCount, int bestTime, int customesServed,int levelIndex)
    {
        this.levelNumber.GetComponent<Text>().text = levelNumber.ToString();
        SetStarCount(starCount);
        //bestTime is in seconds, convert it to 0:00 format
        int minutes = bestTime / 60;
        int seconds = bestTime % 60;
        this.bestTimeText.GetComponent<TMP_Text>().text = minutes.ToString() + ":" + seconds.ToString("00");
        this.customesServedText.GetComponent<TMP_Text>().text = "Customers Served: " + customesServed.ToString();
        this.levelIndex = levelIndex;
        playUpAnimation();
    }

    //this is just for testing because we don't have save data yet
    public void SetLevelData(int levelNumber, int levelIndex){
        this.levelNumber.GetComponent<TMPro.TMP_Text>().text = levelNumber.ToString();
        SetStarCount(0);
        this.levelIndex = levelIndex;
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

    //when the play button is clicked, load the level
    public void OnPlayButtonClicked()
    {
        if (levelIndex != -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
        }
    }
}
