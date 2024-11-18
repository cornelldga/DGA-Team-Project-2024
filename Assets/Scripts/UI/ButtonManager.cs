using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] int levelSelectIndex;
    [SerializeField] int alphaIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPressPlay(){
        SceneManager.LoadScene(levelSelectIndex);
    }

    public void onPressAlpha(){
        SceneManager.LoadScene(alphaIndex);
    }

    public void onPressBack(){
        SceneManager.LoadScene(0);
    }


}
