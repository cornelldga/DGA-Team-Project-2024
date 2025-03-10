using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will change the opacity of this gameObject's material whenever it
/// is in front of the game camera
/// </summary>

public class ObjectFade : MonoBehaviour
{
    Material opaqueMaterial;
    [SerializeField] Material fadeMaterial;

    [SerializeField] float fadeSpeed;
    [Tooltip("The value the alpha of the material will reach when fading")]
    [SerializeField] float fadeAmount;
    [Tooltip("How long before the fade resets")]
    [SerializeField] float fadeResetTime = .25f;
    float resetTime;
    Renderer r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        opaqueMaterial = r.material;
    }
    private void Update()
    {
        if (resetTime > 0)
        {
            resetTime -= Time.deltaTime;
            if (resetTime <= 0)
            {
                ResetFade();
            }
        }
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
        resetTime = fadeResetTime;
    }

    void ResetFade()
    {
        r.material = opaqueMaterial;
    }
}
