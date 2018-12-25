using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    /*
     * void UpdateParticles()
    {
        for (int i = 0; i < PARTICLE_COUNT; i++)
        {
            Particle P = particleArray[i];
            float xPos = P.position.x + FIELD_WIDTH / 2;
            float yPos = P.position.z + FIELD_HEIGHT / 2;
            int xIndex = (int)((xPos / FIELD_WIDTH) * NUM_BINS_X);
            int yIndex = (int)((yPos / FIELD_HEIGHT) * NUM_BINS_Y);
            //Debug.Log("yIndex: " + xIndex);
 
            if (xIndex >= NUM_BINS_X || yIndex >= NUM_BINS_Y || xIndex < 0 || yIndex < 0) return;
            
            float xOffset = binW * vectorField[xIndex][yIndex].x;
            float yOffset = binH * vectorField[xIndex][yIndex].y;
            Debug.Log(xOffset);
            P.transform.position = new Vector3(P.position.x + xOffset, 
                P.transform.position.y, P.position.z + yOffset);
        }
    }
     * 
     * public Transform[] waypointArray;
     float percentsPerSecond = 0.02f; // %2 of the path moved per second
     float currentPathPercent = 0.0f; //min 0, max 1
         
     void Update () 
     {
         currentPathPercent += percentsPerSecond * Time.deltaTime;
         iTween.PutOnPath(gameObject, waypointArray, currentPathPercent);
     }
     
     void OnDrawGizmos()
     {
         //Visual. Not used in movement
         iTween.DrawPath(waypointArray);
     }*/

    /*
        vectorField = new Vector2[4][] {
            new Vector2[]{new Vector2(.01f, 0), new Vector2(.01f, 0), new Vector2(.01f, 0), new Vector2(.01f, 0)},
            new Vector2[]{new Vector2(.01f, 0), new Vector2(.01f, 0), new Vector2(.01f, 0), new Vector2(.01f, 0)},
            new Vector2[]{new Vector2(.01f, 0), new Vector2(.01f, 0), new Vector2(.01f, 0), new Vector2(.01f, 0)},
            new Vector2[]{new Vector2(.01f, 0), new Vector2(.01f, 0), new Vector2(.01f, 0), new Vector2(.01f, 0)}
        };*/

    /*
    //iterate through left and right
    for (int checkX = -1; checkX <= 1; checkX++)
    {
        if (y + 1 < NUM_BINS_Y)
        {
            totalVectors++;
            vectorPointValue += VectorField[x + checkX][y + 1];
            SmoothVectorField(x + checkX, y + 1, layer + 1);
            //Debug.Log(VectorField[x][y + 1]);
        }
        vectorPointValue += VectorField[x + checkX][y];
        if (y - 1 > 0)
        {
            totalVectors++;
            vectorPointValue += VectorField[x + checkX][y - 1];
            SmoothVectorField(x + checkX, y + 1, layer + 1);
        }
    }*/

    //VectorField[x][y] = vectorPointValue / totalVectors;

    /*
     *     void SmoothVectorField(int x, int y, Vector2 original, int layerWidth, float percent, int layer) 
    {
        if (layer > (int)(riverRange / GetBinW()) + 2) return;

        for (int checkX = x - (layerWidth/2); checkX <= x + (layerWidth/2); checkX++)
        {
            if (y + (layerWidth / 2) < NUM_BINS_Y && checkX < NUM_BINS_X && checkX > 0)
            {   //top
                VectorField[checkX][y + (layerWidth/2)] = 
                    (VectorField[checkX][y + (layerWidth / 2)] * (1-percent)) + original * percent;
            }

            if (y - (layerWidth / 2) > 0 && checkX < NUM_BINS_X && checkX > 0)
            {   //bottom
                VectorField[checkX][y - (layerWidth / 2)] =
                    (VectorField[checkX][y - (layerWidth / 2)] * (1 - percent)) + original * percent;
            }
        }

        for (int checkY = y - (layerWidth / 2) + 1; checkY < y + (layerWidth / 2) ; checkY++)
        {
            Debug.Log("x0: " + x + " y0: " + y);
            if (x + (layerWidth / 2) < NUM_BINS_X && checkY < NUM_BINS_Y && checkY > 0)
            {   //left
                //Vector2 test = (VectorField[x - (layerWidth / 2)][checkY] * (1 - percent)) + (original * percent);
                //Debug.Log("Vector2: " + test + " original: " + original + " percent: " + percent);
                Debug.Log("x: " + (x - (layerWidth / 2)) + " y: " + checkY);
                VectorField[x - (layerWidth / 2)][checkY] =
                    (VectorField[x - (layerWidth / 2)][checkY] * (1 - percent)) + (original * percent); 
            }

            if (x - (layerWidth / 2) > 0 && checkY < NUM_BINS_Y && checkY > 0)
            {   //right
                VectorField[x + (layerWidth / 2)][checkY] =
                    (VectorField[x + (layerWidth / 2)][checkY] * (1 - percent)) + original * percent; 
            }
        }

        SmoothVectorField(x, y, original, layerWidth + 2, (percent / smoothBase), layer+1);



    }

        using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PathManager : MonoBehaviour {
        public Particle ParticlePrefab;         // river particle trail GameObject
        public Transform StartPos;              // river origin point
        public bool DebugOn;                    // shows the vector points in world space
        public GameObject DebugVectorPoint;     // object representation of the vector points

        public const int NUM_BINS_X = 80;       // denisty of VectorField / amount of horizontal points
        public const int NUM_BINS_Y = 80;       // denisty of VectorField / amount of vertical points
        public const float FIELD_WIDTH = 80f; 
        public const float FIELD_HEIGHT = 80f;
        public const int PARTICLE_COUNT = 100;   // amount of particles created of x seconds

        public Vector2[][] VectorField;
        public float riverRange = 5f;           // range of vectors affected by moving person
        private float pRandomOffset = 5f;
        private float pCreationInterval = 2f;
        private float smoothBase = 1.5f;
        private float streamSpeed = 200f;

        public float GetBinW()
        { return FIELD_WIDTH / NUM_BINS_X; }

        public float GetBinH()
        { return FIELD_HEIGHT / NUM_BINS_Y; }

        // Initialize VectorField and create particles at set interval
        void Start() {
            VectorField = new Vector2[NUM_BINS_Y][];
            for (int i = 0; i < NUM_BINS_Y; i++) {
                VectorField[i] = new Vector2[NUM_BINS_X];
                for (int z = 0; z < NUM_BINS_X; z++)
                {
                    float xCoord = (GetBinW() * i) - (FIELD_WIDTH / 2);
                    float zCoord = (GetBinH() * z) - (FIELD_HEIGHT / 2);
                    if (DebugOn) Instantiate(DebugVectorPoint, 
                        new Vector3(xCoord, 0, zCoord), StartPos.rotation);
                    VectorField[i][z] = new Vector2(0f, 0f);
                }
            }
            // create new particles every x seconds
            InvokeRepeating("CreateParticles", 0f, pCreationInterval); 
        }

        void CreateParticles()
        {
            for (int i = 0; i < PARTICLE_COUNT; i++)
            {
                Vector3 pPos = StartPos.position;
                pPos += new Vector3(Random.Range(-pRandomOffset, pRandomOffset),
                    0, Random.Range(-pRandomOffset, pRandomOffset));
                Instantiate(ParticlePrefab, pPos, StartPos.rotation);
            }
        }

        public Vector2 GetVectorIndex(Vector2 targetPos)
        {
            int xIndex = (int)((targetPos.x / FIELD_WIDTH) * NUM_BINS_X);
            int yIndex = (int)((targetPos.y / FIELD_HEIGHT) * NUM_BINS_Y);
            return new Vector2(xIndex, yIndex);
        }

        public Vector3 GetVectorPos(Vector2 index)
        {
            return new Vector3(GetBinW() * index.x - FIELD_WIDTH / 2, 0,
                (GetBinW() * index.y) - (FIELD_WIDTH / 2));
        }

        public void UpdateVectorField(Vector3 personPos,Vector3 oldPos)
        {
            float xPos = personPos.x + FIELD_WIDTH / 2;
            float yPos = personPos.z + FIELD_HEIGHT / 2;
            Vector2 pos = new Vector2(xPos, yPos);
            Vector2 index = GetVectorIndex(pos);
            int xIndex = (int)index.x;
            int yIndex = (int)index.y;
            if (xIndex >= NUM_BINS_X || yIndex >= NUM_BINS_Y || xIndex < 0 || yIndex < 0){
                Debug.LogError("personIndex out of bounds");
                return;
            }
            Vector2 offset = new Vector2((personPos.x - oldPos.x)/Time.deltaTime, (personPos.z - oldPos.z)/Time.deltaTime);
            offset = offset * streamSpeed;
            VectorField[xIndex][yIndex] = offset;

            //update surrounding points 

            int rangeOffset = (int)(riverRange/GetBinW());
            for (int xI = -rangeOffset; xI < rangeOffset; xI++)
            {    
                for (int yI = -rangeOffset; yI < rangeOffset; yI++)
                {
                    VectorField[xIndex+xI][yIndex+yI].x = offset.x;
                    VectorField[xIndex+xI][yIndex+yI].y = offset.y;  
                }
            }
        }


    }


    
    void SmoothVectorField(int x, int y, Vector2 original, int layerWidth, float percent, int layer)
    {
        if (layer > (int)(riverRange / GetBinW()) + 1) return;

        for (int checkX = x - (layerWidth / 2); checkX <= x + (layerWidth / 2); checkX++)
        {
            if (y + (layerWidth / 2) < NUM_BINS_Y && checkX < NUM_BINS_X && checkX > 0)
            {   //top
                VectorField[checkX][y + (layerWidth / 2)].Vel =
                    (VectorField[checkX][y + (layerWidth / 2)].Vel * (1 - percent)) + original * percent;
            }

            if (y - (layerWidth / 2) > 0 && checkX < NUM_BINS_X && checkX > 0)
            {   //bottom
                VectorField[checkX][y - (layerWidth / 2)].Vel =
                    (VectorField[checkX][y - (layerWidth / 2)].Vel * (1 - percent)) + original * percent;
            }
        }

        for (int checkY = y - (layerWidth / 2) + 1; checkY < y + (layerWidth / 2); checkY++)
        {
            Debug.Log("x0: " + x + " y0: " + y);
            if (x + (layerWidth / 2) < NUM_BINS_X && checkY < NUM_BINS_Y && checkY > 0)
            {   //left
                //Vector2 test = (VectorField[x - (layerWidth / 2)][checkY] * (1 - percent)) + (original * percent);
                //Debug.Log("Vector2: " + test + " original: " + original + " percent: " + percent);
                Debug.Log("x: " + (x - (layerWidth / 2)) + " y: " + checkY);
                VectorField[x - (layerWidth / 2)][checkY].Vel =
                    (VectorField[x - (layerWidth / 2)][checkY].Vel * (1 - percent)) + (original * percent);
            }

            if (x - (layerWidth / 2) > 0 && checkY < NUM_BINS_Y && checkY > 0)
            {   //right
                VectorField[x + (layerWidth / 2)][checkY].Vel =
                    (VectorField[x + (layerWidth / 2)][checkY].Vel * (1 - percent)) + original * percent;
            }
        }

        SmoothVectorField(x, y, original, layerWidth + 2, (percent / smoothBase), layer + 1);



    }

                //if person near new vectorpoint, update position
            /*if (!PersonNearTarget(currVectorPos, pathManager.GetBinW() * 0.9f))
            {
                Debug.Log("Far!");
                //UpdatePosition();
                Vector2 currVectorIndex = pathManager.GetVectorIndex(transform.position);
                currVectorPos = pathManager.GetVectorPos(currVectorIndex);
            }*/
    
}


