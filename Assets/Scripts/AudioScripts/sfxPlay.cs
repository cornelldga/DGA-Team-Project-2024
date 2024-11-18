using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class sfxPlay : MonoBehaviour
{
    [SerializeField] string soundName;
    // Start is called before the first frame update
    void Start()
    {
        //Play the music
        FindObjectOfType<AudioManager>().Play(soundName);
    }
}
