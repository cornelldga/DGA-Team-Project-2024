using UnityEngine;
/// <summary>
/// Interface used to mark GameObjects that can be crashed into
/// </summary>
public interface ICrashable
{
    /// <summary>
    /// Called when an object "crashes" into this object
    /// </summary>
    /// <param name="speedVector">The speed vector of the object that crashed into this object. Should usually be rb.velocity vector</param>
    /// <param name="position">The position of the object that crashed into this object</param>
    public void Crash(Vector3 speedVector, Vector3 position);
}
