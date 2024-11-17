using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarUI : MonoBehaviour
{
    // The prefab that represents a cop dot on the radar
    [SerializeField] private GameObject radarPrefab;

    private List<RadarDot> dots = new List<RadarDot>();
    private CopModel[] cops;
    private Player player;

    // Scaling variables
    [SerializeField] private float maxDistance = 50; // The maximum distance to track from the player
    private float radarRadius = 140;

    // Start is called before the first frame update
    void Start()
    {
        cops = FindObjectsOfType<CopModel>();
        player = GameManager.Instance.getPlayer();

        // Loop through all cops and instantiate a corresponding dot prefab for each
        foreach (CopModel c in cops)
        {
            RadarDot dotInstance = Instantiate(radarPrefab, transform).GetComponent<RadarDot>();
            dots.Add(dotInstance);
            dotInstance.SetCop(c);
        }


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Number of cops: " + cops.Length);
        //dots[0].transform.localPosition = new Vector3(10, 0, 0);

        // Find distances between each cop and the player
        foreach (RadarDot rd in dots)
        {
            CopModel cop = rd.GetCop();
            Vector3 distance = cop.transform.position - player.transform.position;
            //Debug.Log("Distance to cop: " + distance);

            //rd.transform.localPosition = distance;
            Vector3 ratioDistance = distance / maxDistance;
            float xPos = ratioDistance.x * radarRadius;
            xPos = Mathf.Clamp(xPos, -radarRadius, radarRadius);
            float yPos = ratioDistance.z * radarRadius;
            yPos = Mathf.Clamp(yPos, -radarRadius, radarRadius);
            //Debug.Log("xPos clamped: " + );
            rd.transform.localPosition = new Vector3(xPos, yPos, 0);
        }

    }
}
