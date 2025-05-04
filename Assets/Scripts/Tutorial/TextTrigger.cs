using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{

    [SerializeField] string tutorialMessage;
    [SerializeField] string objectiveMessage;
    [SerializeField] TutorialScript tutorial;
    bool activated = false; // message only appears once



    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            //Debug.Log(tutorialMessage);
            tutorial.ShowMessage(tutorialMessage);
            tutorial.setObjectiveMessage(objectiveMessage);
            activated = true;
        }
    }
}
