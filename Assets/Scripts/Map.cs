using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private float CellSize;
    [SerializeField] private Vector3 Origin;

    public Grid<int> MapGrid;

    public static Map Instance;
    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        MapGrid = new Grid<int>(Width, Height, CellSize, Origin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
