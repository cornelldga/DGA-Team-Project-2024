using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Defines the behavior of a rotatable 2D sprite in a 3D environment.
/// Script can be attached to any prehab with omnidirectional movement. 
/// Requires a Sprite Renderer Component.
/// 
/// Assumes that the sprite is symmetrical. Can be edited to support unqiue sprites in all directions. 
/// </summary>
public class Billboard : MonoBehaviour
{
    // NOTE: degrees of rotation are laid out in the same orientation as a clock,
    // with 12 o'clock being 0 degrees, and rotating clockwise.

    /** The sprite that should be drawn when facing north (0 degrees) */
    [SerializeField] Sprite north;
    /** The sprite that should be drawn when facing south (180 degrees) */
    [SerializeField] Sprite south;
    /** The sprite that should be drawn when facing west (270 degrees) */
    [SerializeField] Sprite west;
    /** The sprite that should be drawn when facing northwest (315 degrees) */
    [SerializeField] Sprite northwest;
    /** The sprite that should be drawn when facing southwest (225 degrees) */
    [SerializeField] Sprite southwest;

    // When true prints direction data to the log
    [SerializeField] bool showDebugLogs = false;


    private SpriteRenderer SR;
    private float cameraRotation;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        cameraRotation = Camera.main.transform.rotation.eulerAngles.y;
    }
    void Update()
    {
        // set sprite renderer plane parallel with the camera's plane. 
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    /** Sets the rendered sprite corresponding to the body's angle of rotation */
    public void UpdateSpriteToRotation(float angle)
    {

        // account for the rotation angle of the camera
        angle = (angle - cameraRotation) % 360;

        // check diagonal angles first
        // Facing NorthWest
        if (isFacingNorth(angle) && isFacingWest(angle))
        {
            if (showDebugLogs) Debug.Log("NW: " + angle);
            SR.sprite = northwest;
            SR.flipX = true;

        }
        // Facing NorthEast
        else if (isFacingNorth(angle) && isFacingEast(angle))
        {
            if (showDebugLogs) Debug.Log("NE: " + angle);
            SR.sprite = northwest;
            SR.flipX = false;
        }
        // Facing SouthWest
        else if (isFacingSouth(angle) && isFacingWest(angle))
        {
            if (showDebugLogs)  Debug.Log("SW: " + angle);
            SR.sprite = southwest;
            SR.flipX = true;

        }
        // Facing SouthEast
        else if (isFacingSouth(angle) && isFacingEast(angle))
        {
            if (showDebugLogs) Debug.Log("SE: " + angle);
            SR.sprite = southwest;
            SR.flipX = false;
        }
        // Facing North
        else if (isFacingNorth(angle))
        {
            if (showDebugLogs) Debug.Log("N: " + angle);
            SR.sprite = north;
            SR.flipX = false;
        }
        // Facing South
        else if (isFacingSouth(angle))
        {
            if (showDebugLogs) Debug.Log("S: " + angle);
            SR.sprite = south;
            SR.flipX = false;
        }
        // Facing East
        else if (isFacingEast(angle))
        {
            if (showDebugLogs) Debug.Log("E: " + angle);
            SR.sprite = west;
            SR.flipX = false;
        }
        // Facing West
        else if (isFacingWest(angle))
        {
            if (showDebugLogs) Debug.Log("W: " + angle);
            SR.sprite = west;
            SR.flipX = true;
        }
        
    }

    /** Returns true if the body is facing East */
    bool isFacingEast(float angle)
    {
        return angle >= 22.5 && angle < 157.5;
    }

    /** Returns true if the body is facing North */
    bool isFacingNorth(float angle)
    {
        return angle >= 292.5 && angle < 360 || angle >= 0 && angle < 67.5;
    }

    /** Returns true if the body is facing South */
    bool isFacingSouth(float angle)
    {
        return angle >= 157.5 && angle < 247.5;
    }

    /** Returns true if the body is facing West */
    bool isFacingWest(float angle)
    {
        return angle >= 202.5 && angle < 337.5;
    }

}
