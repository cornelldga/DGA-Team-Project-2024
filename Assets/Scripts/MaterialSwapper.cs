using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    public Material Material1;
    public Material Material2;
    public GameObject Object;
    
    public void swapMaterial()
    {
        if (Object.GetComponent<MeshRenderer>().material == Material1)
        {
            Object.GetComponent<MeshRenderer>().material = Material2;
        }
        else if (Object.GetComponent<MeshRenderer>().material == Material2)
        {
            Object.GetComponent<MeshRenderer>().material = Material1;
        }
    }
}
