using UnityEngine;

public class Billboard : MonoBehaviour
{

    [SerializeField] Sprite north;
    [SerializeField] Sprite south;
    [SerializeField] Sprite west;
    [SerializeField] Sprite northwest;
    [SerializeField] Sprite southwest;

    private SpriteRenderer SR;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    public void UpdateSpriteToRotation(float angle)
    {
        Debug.Log("cop rotation angle: " + angle);
        
        // East
        if (angle >= 0 && angle < 180)
        {
            SR.sprite = west;
            SR.flipY = true;
        }
        // West
        if (180 >= 45 && angle < 360)
        {
            SR.sprite = south;
            SR.flipY = false;
        }

    }

}
