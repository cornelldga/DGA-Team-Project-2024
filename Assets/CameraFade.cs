using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("hello");
        if (other.gameObject.TryGetComponent(out ObjectFade fade))
        {
            Debug.Log("FADE");
            fade.Fade();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ObjectFade fade))
        {
            fade.Reset();
        }
    }
}
