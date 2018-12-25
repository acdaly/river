using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{
    public Transform[] waypointArray;
    float percentsPerSecond = 0.02f; // %2 of the path moved per second
    float currentPathPercent = 0.0f; //min 0, max 1

    void Update()
    {
        currentPathPercent += percentsPerSecond * Time.deltaTime;
        iTween.PutOnPath(gameObject, waypointArray, currentPathPercent);
    }

    void OnDrawGizmos()
    {
        //Visual. Not used in movement
        iTween.DrawPath(waypointArray);
    }
    //void Start(){
    //iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
    //}
}

