using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionalIndicator : MonoBehaviour
{
    //[SerializeField] private Transform target;
    private Transform target;
    [SerializeField] private Transform player;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;

    [SerializeField] private RectTransform rect = null;
    private Image arrow = null;

    // Start is called before the first frame update
    void Start()
    {
        arrow = this.transform.GetComponentInChildren<Image>();
        arrow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            RotateToTheTarget();
        }
        
    }

    public void SetIndicator(Transform t)
    {
        target = t;
        arrow.enabled = true;

    }

    public void RemoveIndicator()
    {
        target = null;
        arrow.enabled = false;
    }



    void RotateToTheTarget()
    {

        // Calculate the direction from A to B
        Vector3 direction = target.position - player.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the arrow around its Z-axis
        rect.localRotation = Quaternion.Euler(0, 0, angle + 90);


    }
}
