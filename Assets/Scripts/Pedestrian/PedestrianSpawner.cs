using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject pedestrianPrefab;
    public int pedestriansToSpawn = 10;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(SpawnPedestrians());
    }

    private IEnumerator SpawnPedestrians()
    {
        int count = 0;
        while (count < pedestriansToSpawn)
        {
            GameObject pedestrian = Instantiate(pedestrianPrefab);
            Transform draw = transform.GetChild(Random.Range(0, transform.childCount - 1));
            pedestrian.GetComponent<WaypointNavigator>().currentWaypoint = draw.GetComponent<Waypoint>();
            pedestrian.transform.position = draw.position;
            pedestrian.GetComponent<PedestrianNavigationController>().movementSpeed = Random.Range(1, 3);
            yield return new WaitForSeconds(1);

            count++;
        }
    }
}
