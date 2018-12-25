using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Point in branching path that simulated person heads towards
public class PathNode : MonoBehaviour {
    public int id;
    public int branches
    {
        get { return nextNode.Length; }
    }
    public PathNode[] nextNode;
    public Vector3 position;
    List<int> activeVisitors = new List<int>();

    private void Start()
    {
        position = transform.position;
    }

    // Makes the path node green when no one is moving toward it
    public void Deactivate(int personID)
    {                                                                                     
        activeVisitors.Remove(personID);
        if (activeVisitors.Count == 0)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.green;
        }
    }

    // Makes the path node turn red when someone is moving toward it
    public void Activate(int personID)
    {
        // activate only if person is not already recorded as being at the node
        if (!activeVisitors.Contains(personID)) 
        {
            activeVisitors.Add(personID);
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.red;
        }
    }
}
