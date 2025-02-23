using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] int levelSelectIndex;
    [SerializeField] int alphaIndex;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.Instance;
    }

    public void onPressPlay() {
        audioManager.PlaySound("sfx_MenuClick");
        SceneManager.LoadScene(levelSelectIndex);
    }

    public void onPressAlpha(){
        audioManager.PlaySound("sfx_MenuClick");
        SceneManager.LoadScene(alphaIndex);
    }

    public void onPressBack(){
        audioManager.PlaySound("sfx_MenuClick");
        SceneManager.LoadScene(0);
    }

    public void reloadCurrentScene(){
        audioManager.PlaySound("sfx_MenuClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayClickSound()
    {
        audioManager.PlaySound("sfx_MenuClick");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
