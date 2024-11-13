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
        

        angle = angle % 360;
        // check diagonal angles first
        if (isFacingNorth(angle) && isFacingWest(angle))
        {

            Debug.Log("NW: cop rotation angle: " + angle);

            SR.sprite = northwest;
            SR.flipX = true;

        }
        else if (isFacingNorth(angle) && isFacingEast(angle))
        {
            Debug.Log("NE: cop rotation angle: " + angle);

            SR.sprite = northwest;
            SR.flipX = false;
        }
        else if (isFacingSouth(angle) && isFacingWest(angle))
        {
            Debug.Log("SW: cop rotation angle: " + angle);

            SR.sprite = southwest;
            SR.flipX = true;

        }
        else if (isFacingSouth(angle) && isFacingEast(angle))
        {

            Debug.Log("SE: cop rotation angle: " + angle);

            SR.sprite = southwest;
            SR.flipX = false;
        }
        else if (isFacingNorth(angle))
        {
            Debug.Log("NORTH: cop rotation angle: " + angle);

            SR.sprite = north;
            SR.flipX = false;
        }
        else if (isFacingSouth(angle))
        {
            Debug.Log("SOUTH: cop rotation angle: " + angle);

            SR.sprite = south;
            SR.flipX = false;
        }
        else if (isFacingEast(angle))
        {
            Debug.Log("EAST: cop rotation angle: " + angle);

            SR.sprite = west;
            SR.flipX = false;
        }
        else if (isFacingWest(angle))
        {
            Debug.Log("WEST: cop rotation angle: " + angle);

            SR.sprite = west;
            SR.flipX = true;
        }
        
    }

    // rotation starts at 90* in a unit circle. and goes clockwise. 0 degrees is 12 o'clock
    bool isFacingEast(float angle)
    {
        return angle >= 0 && angle < 180;
    }

    bool isFacingNorth(float angle)
    {
        return angle >= 270 && angle < 360 || angle >= 0 && angle < 90;
    }

    bool isFacingSouth(float angle)
    {
        return angle >= 90 && angle < 270;
    }

    bool isFacingWest(float angle)
    {
        return angle >= 180 && angle < 360;
    }

}
