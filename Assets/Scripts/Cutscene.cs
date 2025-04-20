using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string mainMenu = "Main Menu";
    public bool canSkip = true;


    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.time = 0;
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSkip && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
        LoadMainMenu();
        }
        
    }
    void EndReached(VideoPlayer vp)
    {
        LoadMainMenu();
    }
    void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
