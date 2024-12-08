using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Defines the behavior of a rotatable 2D sprite in a 3D environment.
/// Script can be attached to any prehab with 8-directional movement. 
/// </summary>
/// 
[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (transform.rotation.eulerAngles.x != Camera.main.transform.rotation.eulerAngles.x
            || transform.rotation.eulerAngles.y != Camera.main.transform.rotation.eulerAngles.y)
        {
            throw new System.Exception("This Billboard's rotation is not the same as the camera." +
                " The rotation must be the same to project correctly");
        }
    }
    /// <summary>
    /// Sets the "Moving North" boolean in the animator to the boolean arguement
    /// </summary>
    public void SetMovingNorth(bool isMovingNorth)
    {
        anim.SetBool("Moving North", isMovingNorth);
    }
    /// <summary>
    /// Sets the "Moving Sorth" boolean in the animator to the boolean arguement
    /// </summary>
    public void SetMovingSouth(bool isMovingSouth) 
    {
        anim.SetBool("Moving South", isMovingSouth);
    }
    /// <summary>
    /// Sets the "Moving East" boolean in the animator to the boolean arguement
    /// </summary>
    public void SetMovingEast(bool isMovingEast)
    {
        anim.SetBool("Moving East", isMovingEast);
    }
    /// <summary>
    /// Sets the "Moving West" boolean in the animator to the boolean arguement
    /// </summary>
    public void SetMovingWest(bool isMovingWest)
    {
        anim.SetBool("Moving West", isMovingWest);
    }
    /// <summary>
    /// Sets the "Facing North" boolean in the animator to the boolean arguement
    /// </summary>
    /// <param name="isFacingNorth"></param>
    public void SetFacingNorth(bool isFacingNorth)
    {
        anim.SetBool("Facing North", isFacingNorth);
    }
    /// <summary>
    /// Sets the "Facing South" boolean in the animator to the boolean arguement
    /// </summary>
    public void SetFacingSouth(bool isFacingSouth)
    {
        anim.SetBool("Facing South", isFacingSouth);
    }
    /// <summary>
    /// Sets the "Facing East" boolean in the animator to the boolean arguement
    /// </summary>
    public void SetFacingEast(bool isFacingEast)
    {
        anim.SetBool("Facing East", isFacingEast);
    }
    /// <summary>
    /// Sets the "Facing West" boolean in the animator to the boolean arguement
    /// </summary>
    public void SetFacingWest(bool isFacingWest)
    {
        anim.SetBool("Facing West", isFacingWest);
    }
    /// <summary>
    /// Sets all boolean parameters to false
    /// </summary>
    public void ResetConditions()
    {
        SetMovingNorth(false);
        SetMovingSouth(false);
        SetMovingEast(false);
        SetMovingWest(false);
        SetFacingNorth(false);
        SetFacingSouth(false);
        SetFacingEast(false);
        SetFacingWest(false);
    }
}
