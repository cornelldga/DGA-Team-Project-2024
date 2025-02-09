using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// This class spawns pedestrians at random waypoints at the start of the game.
/// </summary>
public class PedSpawner : MonoBehaviour
{
    [SerializeField] GameObject pedestrianPrefab;
    [SerializeField] int pedestriansToSpawn = 10;

    // Update is called once per frame
    void Start()
    {
        // Wait for some time before spawning pedestrians to prevent spawning them before the waypoints or buildings are initialized
        StartCoroutine(WaitForSomeTime(0.01f));
        SpawnPedestriansInstantly();
    }

    private IEnumerator WaitForSomeTime(float time)
    {
        yield return new WaitForSeconds(time);
    }

    private void SpawnPedestriansInstantly()
    {
        int count = 0;
        while (count < pedestriansToSpawn)
        {
            Transform draw = transform.GetChild(Random.Range(0, transform.childCount - 1));
            // Note the change here: if we don't pass in position, the pedestrian will spawn at the origin of the prefab for unknown reason, even if we set the position later and we actually correctly set it (I've looked at it by adding Debug.Logs to Start methods, and they print the correct positions. However, as long as Update() is called, the pedestrian will be teleported to the origin of the prefab.)
            GameObject pedestrian = Instantiate(pedestrianPrefab, draw.position, Quaternion.identity);
            pedestrian.GetComponent<WaypointNavigator>().currentWaypoint = draw.GetComponent<Waypoint>();
            pedestrian.GetComponent<PedNavigationController>().movementSpeed = Random.Range(3, 7);
            count++;
        }
    }

    // private IEnumerator SpawnPedestrians()
    // {
    //     int count = 0;
    //     while (count < pedestriansToSpawn)
    //     {
    //         yield return new WaitForSeconds(0.01f);
    //         GameObject pedestrian = Instantiate(pedestrianPrefab);
    //         Transform draw = transform.GetChild(Random.Range(0, transform.childCount - 1));
    //         pedestrian.GetComponent<WaypointNavigator>().currentWaypoint = draw.GetComponent<Waypoint>();
    //         pedestrian.transform.position = draw.position;
    //         pedestrian.GetComponent<PedestrianNavigationController>().movementSpeed = Random.Range(3, 7);

    //         count++;
    //     }
    // }
}
