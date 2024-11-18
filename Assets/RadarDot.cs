using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadarDot : MonoBehaviour
{
    private CopModel cop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCop(CopModel c)
    {
        cop = c;
    }

    public CopModel GetCop()
    {
        return cop;
    }
}
