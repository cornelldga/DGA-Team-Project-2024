using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    private ObjectFade fade;
    private Transform player;

    private void Start()
    {
        player = GameManager.Instance.getPlayer().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);
        float distance = dir.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(ray, distance);

        // Reset fade at the start of each frame
        if (fade != null)
        {
            fade.doFade = false;
        }
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && !hit.collider.CompareTag("Player"))
            {
                fade = hit.collider.gameObject.GetComponent<ObjectFade>();
                if (fade != null)
                {
                    fade.doFade = true;
                }
            }
        }
    }
}
