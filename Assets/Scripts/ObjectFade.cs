using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
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
            StartFade();
        }
        else
        {
            ResetFade();
        }
    }

    void StartFade()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed));
        mat.color = smoothColor;
        print(mat.color.a);
    }

    void ResetFade()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed));
        mat.color = smoothColor;
    }
}
