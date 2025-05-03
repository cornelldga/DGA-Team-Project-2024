using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private GameObject cop;

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.GetCustomers().Count > 0)
        {
            cop.SetActive(true);
        }

    }
}
