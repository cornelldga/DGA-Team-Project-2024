using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will change the opacity of this gameObject's material whenever it
/// is in front of the game camera
/// </summary>

public class ObjectFade : MonoBehaviour
{
    [SerializeField] Material opaqueMaterial;
    [SerializeField] Material fadeMaterial;

    [SerializeField] float fadeSpeed;
    [Tooltip("The value the alpha of the material will reach when fading")]
    [SerializeField] float fadeAmount;
    Renderer r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
    }
    /// <summary>
    /// Smoothly lower the alpha to the fadeAmount
    /// </summary>
    public void Fade()
    {
        if(r.material != fadeMaterial)
        {
            r.material = fadeMaterial;
        }
        Color currentColor = r.material.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed));
        r.material.color = smoothColor;
        print(r.material.color.a);
    }

    public void Reset()
    {
        r.material = opaqueMaterial;
    }
}
