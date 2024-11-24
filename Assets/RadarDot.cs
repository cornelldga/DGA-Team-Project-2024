using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadarDot : MonoBehaviour
{
    private CopModel cop;

    [SerializeField] private Animator blipAnim;

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

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        blipAnim.Play("RadarDotAnim", -1, 0.0f);
        //spriteMove = -0.1f;
    }
}
