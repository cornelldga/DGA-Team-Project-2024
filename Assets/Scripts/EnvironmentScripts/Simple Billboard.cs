using UnityEngine;

/// <summary>
///  Simple billboard script to make 2d assets always face the camera
/// </summary>
public class SimpleBillboard : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Get the sprite renderer 
    /// </summary>
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Method to make the script sprite renderer face the player
    /// </summary>
    private void LateUpdate()
    {
        if (Camera.main != null && spriteRenderer != null)
        {
            // Only modify the sprite's billboarding property
            spriteRenderer.transform.forward = Camera.main.transform.forward;
        }
    }
}