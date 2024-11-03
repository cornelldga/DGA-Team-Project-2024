using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OilMask : MonoBehaviour
{
    private RectMask2D mask;
    [SerializeField] private Player playerScript;
    private int oilAmount;
    private int oilUIHeight;
    // Start is called before the first frame update
    void Start()
    {
        mask = GetComponent<RectMask2D>();
        mask.enabled = true;
        //the height of this gameobject is the height of the oil UI
        oilUIHeight = (int)GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        oilAmount = (int)playerScript.GetOil();
        Vector4 padding = mask.padding;
        //set the top padding to the oil amount, but invert the value so the mask will shrink as the oil amount increases
        int topPadding = oilUIHeight - oilAmount;
        padding.w = topPadding;
        mask.padding = padding;
    }
}
