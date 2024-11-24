using UnityEngine;

/// <summary>
///  Simple billboard script to make 2d assets always face the camera
/// </summary>
public class SimpleBillboard : MonoBehaviour
{
    /// <summary>
    /// Method to make the script sprite renderer face the player
    /// </summary>
    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}