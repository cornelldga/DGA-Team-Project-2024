using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// Represents a waypoint in a navigation path for a pedestrian or object.
/// </summary>
/// <remarks>
/// A waypoint is a point in space with references to the previous and next waypoints, allowing for linked navigation along a path. Each waypoint also has a width range, within which a random position can be calculated for movement or spawning purposes.
/// </remarks>
public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    [Range(0, 5)]
    public float width = 1f;

    public List<Waypoint> branches;
    [Range(0, 1)]
    public float branchRatio = 0.5f;

    /// <summary>
    /// Returns a random position along the waypoint's width range.
    /// </summary>
    /// <returns>
    /// A <see cref="Vector3"/> representing a position within the specified width range.
    /// </returns>
    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2;
        Vector3 maxBound = transform.position - transform.right * width / 2;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }

}
