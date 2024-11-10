using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionalIndicator : MonoBehaviour
{
    //[SerializeField] private Transform target;
    private Transform target;
    Transform player;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;

    [SerializeField] private RectTransform rect = null;
    private Image arrow = null;

    // Start is called before the first frame update
    void Start()
    {
        arrow = this.transform.GetComponentInChildren<Image>();
        arrow.enabled = false;
        player = GameManager.Instance.getPlayer().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            RotateToTheTarget();
            // Debug.Log("Target pos: " + target.position);
            // Debug.Log("Player pos: " + player.position);
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



    void RotateToTheTarget(){
        Vector3 direction = (target.position - player.position);
        direction.y = 0;
        direction.Normalize();

        float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);

        rect.localRotation = Quaternion.Euler(0, 0, -angle);

    }
}
