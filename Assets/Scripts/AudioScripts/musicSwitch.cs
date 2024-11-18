using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] string musicName;
    // Start is called before the first frame update
    void Start()
    {
        //Play the music
        FindObjectOfType<AudioManager>().PlayMusic(musicName);
    }
}
