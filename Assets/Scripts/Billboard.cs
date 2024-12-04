using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Defines the behavior of a rotatable 2D sprite in a 3D environment.
/// Script can be attached to any prehab with omnidirectional movement. 
/// Requires an Animator and its movement fields to be changed in the Controller script.
/// </summary>
/// 
[RequireComponent(typeof(Animator))]
public class Billboard : MonoBehaviour
{

    [HideInInspector] public bool movingNorth;
    [HideInInspector] public bool movingSouth;
    [HideInInspector] public bool movingWest;
    [HideInInspector] public bool movingEast;
    [HideInInspector] public bool facingNorth;
    [HideInInspector] public bool facingSouth;
    [HideInInspector] public bool facingWest;
    [HideInInspector] public bool facingEast;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if(transform.rotation.eulerAngles.x != Camera.main.transform.rotation.eulerAngles.x
            || transform.rotation.eulerAngles.y != Camera.main.transform.rotation.eulerAngles.y)
        {
            throw new System.Exception("This Billboard's rotation is not the same as the camera." +
                " The rotation must be the same to project correctly");
        }
    }

    
    private void Update()
    {
        anim.SetBool("Moving North", movingNorth);
        anim.SetBool("Moving South", movingSouth);
        anim.SetBool("Moving East", movingEast);
        anim.SetBool("Moving West", movingWest);
        anim.SetBool("Facing North", facingNorth);
        anim.SetBool("Facing South", facingSouth);
        anim.SetBool("Facing East", facingEast);
        anim.SetBool("Facing West", facingWest);
    }

}
