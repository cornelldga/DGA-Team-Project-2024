using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Starts a tutorial at the beginning of the game. When the player hits any key,
/// it starts the game
/// </summary>
public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialUI;
    bool toggle = false;
    void Start()
    {
        GameManager.Instance.PauseGame();
    }
    private void Update()
    {
        if (toggle)
        {
            return;
        }
        if (Input.anyKey) { 
            tutorialUI.SetActive(false);
            toggle = true;
            GameManager.Instance.ResumeGame();
        }
    }
}
