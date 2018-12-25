using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person : MonoBehaviour {
    public int id;
    public float speed = 3; // how fast simulated person moves
    public PathNode nextNode;
    public PathNode lastNode;
    public bool AutoMove = true;

    Vector3 position;
    Vector3 oldPosition;
    Vector3 nextPosition;
    PathManager pathManager;
    private Vector3 currVectorPos;
    private float offset = 2f;

    private void Start()
    {
        position = transform.position;
        //get the corresponding vector point's position in world space
        nextPosition = nextNode.position;
        pathManager = GameObject.Find("PathManagerContainer").transform.GetChild(0).GetComponent<PathManager>();
        Vector2 index = pathManager.GetVectorIndex(transform.position);
        currVectorPos = pathManager.GetVectorPos(index);
            
        //InvokeRepeating("UpdatePosition", 1f, 0.5f);
    }


    bool PersonNearTarget(Vector3 target, float range)
    {
        return position.x < target.x + range && position.x > target.x - range
            && position.z < target.z + range && position.z > target.z - range;
    }

    // Update VectorField based on person's position
    void UpdatePosition()
    {
        if (nextNode != null)
        {
            oldPosition = position;
            position = transform.position;
            pathManager.GetComponent<PathManager>().UpdateVectorField(position, oldPosition, id);
        }
        
    }

    private void Update()
    {
        if (nextNode != null)
        {
            if (AutoMove)
            {
                // move towards nextNode
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);

            }

            UpdatePosition();
            // if at nextNode, activate nextNode and deactive last node
            if (PersonNearTarget(nextNode.position, offset)) 
            {
                if (nextNode.branches != 0)
                {
                    int nextIndex = Random.Range(0, nextNode.branches);
                    lastNode = nextNode;
                    nextNode = nextNode.nextNode[nextIndex];
                    lastNode.Deactivate(id);
                    nextNode.Activate(id);
                    //update pathManager

                    nextPosition = nextNode.position + new Vector3(Random.Range(-offset, offset), 
                        Random.Range(-offset, offset), Random.Range(-offset, offset));
                }
                else // reached end, no more nodes in path
                {
                    lastNode = nextNode;
                    nextNode = null;
                }
                
            }
        }
        
    }
}
