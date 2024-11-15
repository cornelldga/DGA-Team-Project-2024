using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// This class spawns pedestrians at random waypoints at the start of the game.
/// </summary>
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
            yield return new WaitForSeconds(0.5f);
            GameObject pedestrian = Instantiate(pedestrianPrefab);
            Transform draw = transform.GetChild(Random.Range(0, transform.childCount - 1));
            pedestrian.GetComponent<WaypointNavigator>().currentWaypoint = draw.GetComponent<Waypoint>();
            pedestrian.transform.position = draw.position;
            pedestrian.GetComponent<PedestrianNavigationController>().movementSpeed = Random.Range(3, 7);

            count++;
        }
    }
}
