using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    [Tooltip("How often a fade check occurs")]
    [SerializeField] float checkUpdateRate;
    float updateWaitTime;
    [SerializeField] LayerMask fadeCheckMask;

    private void Update()
    {
        updateWaitTime -= Time.deltaTime;
        if (updateWaitTime <= 0)
        {
            FadeCheck();
            updateWaitTime = checkUpdateRate;
        }
    }

    void FadeCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(
                new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // 2. Perform the Raycast
        if (Physics.Raycast(ray, out RaycastHit hit, fadeCheckMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out ObjectFade fade))
            {
                fade.Fade();
            }
        }
    }
}
