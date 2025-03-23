using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarUI : MonoBehaviour
{
    // The prefab that represents a cop dot on the radar
    [SerializeField] private GameObject radarPrefab;

    private List<RadarDot> dots = new List<RadarDot>();
    //private CopModel[] cops;
    private Customer[] customers;
    private Player player;

    // Scaling variables
    [SerializeField] private float maxDistance = 50; // The maximum distance to track from the player
    [SerializeField] private float radarRadius = 140;

    private float borderPad = 10;

    // Start is called before the first frame update
    void Start()
    {
        //cops = FindObjectsOfType<CopModel>();
        customers = FindObjectsOfType<Customer>();
        player = GameManager.Instance.getPlayer();

        // Loop through all cops and instantiate a corresponding dot prefab for each
        foreach (Customer c in customers)
        {
            RadarDot dotInstance = Instantiate(radarPrefab, transform).GetComponent<RadarDot>();
            dots.Add(dotInstance);
            dotInstance.SetCustomer(c);
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
            Customer customer = rd.GetCustomer();
            if (customer == null) continue;
            Vector3 distance = customer.transform.position - player.transform.position;
            //Debug.Log("Distance to cop: " + distance);

            //rd.transform.localPosition = distance;
            Vector3 ratioDistance = distance / maxDistance;
            float xPos = ratioDistance.x * radarRadius;
            float yPos = ratioDistance.z * radarRadius;
            float bound = radarRadius + borderPad;
            if (xPos > bound || yPos > bound)
            {
                rd.GetComponent<Image>().enabled = false;
            }
            else
            {
                rd.GetComponent<Image>().enabled = true;
                xPos = Mathf.Clamp(xPos, -radarRadius, radarRadius);
                yPos = Mathf.Clamp(yPos, -radarRadius, radarRadius);
                //Debug.Log("xPos clamped: " + xPos);
                rd.transform.localPosition = new Vector3(xPos, yPos, 0);
            }
            

        }

    }
}
