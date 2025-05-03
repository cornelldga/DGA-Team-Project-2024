using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFadeCheck : MonoBehaviour
{
    [SerializeField] LayerMask fadeCheckMask;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent(out ObjectFade fade))
        {
            fade.Fade();
        }
    }
}
