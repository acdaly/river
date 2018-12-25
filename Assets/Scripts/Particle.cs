using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    public PathManager pM;
    public Vector3 position;
    public float speed = 10f;      // the speed the particle trails move
    float smoothing = 1.5f; // the amount the last vector point visited by a particle affects the next one
    Vector3 target;         // the target the particle moves towards
    bool finished = false;  // whether vector hit a dead end, freezes particle when true
    Vector2 lastVectorOffset = new Vector2(0, 0); // the amount the last vector moved the particle
    Vector3 currVectorPos;  // the vector point's position in real world space
    private float t = 0;

    // returns true when particle is still near the last vector point it visited within a given range
    bool nearLastVectorPoint(float range)
    {
        return position.x < currVectorPos.x + range && position.x > currVectorPos.x - range
            && position.z < currVectorPos.z + range && position.z > currVectorPos.z - range;
    }

    // Use this for initialization
    void Start () {
        pM = GameObject.Find("PathManagerContainer").transform.GetChild(0).GetComponent<PathManager>();
        position = transform.position;

    }

    // get the target's offset / distance away from the particle
    Vector2 GetTargetOffset()
    {
        // use non-negative particle position to find the corresponding index in the vector field
        float xPos = position.x + PathManager.FIELD_WIDTH / 2;
        float yPos = position.z + PathManager.FIELD_HEIGHT / 2;
        int xIndex = (int)((xPos / PathManager.FIELD_WIDTH) * PathManager.NUM_BINS_X);
        int yIndex = (int)((yPos / PathManager.FIELD_HEIGHT) * PathManager.NUM_BINS_Y);
          
        //get the corresponding vector point's position in world space
        currVectorPos = new Vector3(pM.GetBinW() * xIndex - PathManager.FIELD_WIDTH / 2, 0,
            (pM.GetBinH() * yIndex) - (PathManager.FIELD_WIDTH / 2));

        // index is out of bounds-- prevents unsafe indexing into an array from crashing program
        if (xIndex >= PathManager.NUM_BINS_X || yIndex >= PathManager.NUM_BINS_Y
                || xIndex < 0 || yIndex < 0)
        {
            Debug.LogError("index is out of bounds");
            return new Vector2();
        }

        // get the target's x and y distance from the particle's position
        float xOffset = pM.GetBinW() * pM.VectorField[xIndex][yIndex].Vel.x;
        float yOffset = pM.GetBinH() * pM.VectorField[xIndex][yIndex].Vel.y;

        // if the particle gets to a vector point of 0, set finished to true so it won't move
        if (xOffset == 0 && yOffset == 0)
        {
            finished = true;
            return new Vector2();
        }
        return new Vector2(xOffset, yOffset);
    }

    void Update()
    {   
        // don't update particle position
        if (finished) return;

        // change target if far away enough from an old vector point / close to a new one
        if (!nearLastVectorPoint(pM.GetBinH() * 0.95f))
        {
            //Debug.Log("switching target");
            Vector2 offset = GetTargetOffset();

            // add smoothing to the offset amount
            
            offset.x = (offset.x + lastVectorOffset.x * smoothing) / (smoothing + 1);
            offset.y = (offset.y + lastVectorOffset.y * smoothing) / (smoothing + 1);
            
            //float xRatio= offset.x / (offset.x + offset.y);
            //float yRatio = offset.y / (offset.x + offset.y);
            

            // update the target and last vector offset
            target = new Vector3(position.x + offset.x,
                transform.position.y, position.z + offset.y);
            lastVectorOffset = new Vector2(offset.x, offset.y);
        }
        // move towards nextVectorPoint
        float step = speed * Time.deltaTime;

       float noise = Mathf.PerlinNoise(t + (Time.deltaTime * 2), 0f);
        float deltaNoiseX = Mathf.PerlinNoise(t + (Time.deltaTime * 2), 0f) - Mathf.PerlinNoise(t, 0f);
        float deltaNoiseY = Mathf.PerlinNoise(0f, t + (Time.deltaTime * 2)) - Mathf.PerlinNoise(0f, t);
        t += Time.deltaTime * 2;
        Vector3 endPosition = Vector3.MoveTowards(transform.position, target, step);

        endPosition.x += (deltaNoiseX);
        endPosition.z += deltaNoiseY;
        
        
        transform.position = endPosition;
        position = transform.position;
    }
}
