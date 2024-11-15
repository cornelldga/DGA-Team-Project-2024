using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will change the opacity of this gameObject's material whenever it
/// is in front of the game camera
/// </summary>

public class ObjectFade : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    [Tooltip("The value the alpha of the material will reach when fading")]
    [SerializeField] float fadeAmount;
    float originalOpacity;
    Material mat;
    public bool doFade = false;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalOpacity = mat.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (doFade)
        {
            Fade();
        }
        else
        {
            ResetFade();
        }
    }
    /// <summary>
    /// Smoothly lower the alpha to the fadeAmount
    /// </summary>
    void Fade()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed));
        mat.color = smoothColor;
    }
    /// <summary>
    /// Smoothly increase the alpha to the originalOpacity
    /// </summary>
    void ResetFade()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed));
        mat.color = smoothColor;
    }
}
