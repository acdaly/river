using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PathManager : MonoBehaviour {
    public Transform StartPos;              // river origin point
    public bool DebugOn;                    // shows the vector points in world space
    public GameObject DebugVectorPoint;     // object representation of the vector points

    public const int NUM_BINS_X = 100;       // denisty of VectorField / amount of horizontal points
    public const int NUM_BINS_Y = 100;       // denisty of VectorField / amount of vertical points
    public const float FIELD_WIDTH = 80f; 
    public const float FIELD_HEIGHT = 80f;
    public const int PARTICLE_COUNT = 100;  // amount of particles created of x seconds
    [SerializeField]
    public VectorPoint[][] VectorField;
    [SerializeField]
    public VectorPoint VP;
    public float riverRange = 5f;           // range of vectors affected by moving person
    private float pRandomOffset = 5f;
    private float pCreationInterval = 0.5f;
    private float smoothBase = 2.5f;
    [SerializeField]
    public float streamSpeed = 200f;

    public float GetBinW()
    { return FIELD_WIDTH / NUM_BINS_X; }

    public float GetBinH()
    { return FIELD_HEIGHT / NUM_BINS_Y; }

    private string gameDataFileName = "data.json";
    private string json;

    // Initialize VectorField and create particles at set interval
    void Start() {
        VectorField = new VectorPoint[NUM_BINS_Y][];
        int count = 0;
        for (int i = 0; i < NUM_BINS_Y; i++) {
            VectorField[i] = new VectorPoint[NUM_BINS_X];
            for (int z = 0; z < NUM_BINS_X; z++)
            {
                float xCoord = (GetBinW() * i) - (FIELD_WIDTH / 2);
                float zCoord = (GetBinH() * z) - (FIELD_HEIGHT / 2);
                 
                VectorPoint point = Instantiate(VP, new Vector3(xCoord, 0, zCoord), StartPos.rotation);
                point.IndexX = i;
                point.IndexY = z;
                if (count % 4 == 0) point.particleCreator = true;
                if (count % 6 == 0) point.longTermCreator = true;
                count++;
                VectorField[i][z] = point;

                if (DebugOn) VectorField[i][z].GetComponent<MeshRenderer>().enabled = true;
                else VectorField[i][z].GetComponent<MeshRenderer>().enabled = false;
                
            }
        }
        //LoadRiver();
        // create new particles every x seconds
        //InvokeRepeating("CreateParticles", 0f, pCreationInterval); 
    }

    private void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            Debug.Log("save!");
            SaveRiver();
        }
        if (Input.GetKeyDown("l"))
        {
            Debug.Log("load!");
            LoadRiver();
        }
    }
    [System.Serializable]
    public class VectorPointData
    {
        public Vector3 position;
        public Vector2 Vel = new Vector2(0, 0);
        public float Speed = 5f;
        public int IndexX;
        public int IndexY;
        public int VisitCount = 0;
        public int ParticleCount = 1;
        public bool createParticleOn = false;
        public bool particleCreator = false;    
        public bool longTermOn = false;         
        public bool longTermCreator = false;
        public int activeID = 0;
    }

        [System.Serializable]
    public class VectorFieldInitializer
    {
        public VectorPointData[] FlatVectorField;
    }



    void SaveRiver()
    {
        VectorPointData[] flatArray = new VectorPointData[NUM_BINS_X*NUM_BINS_Y];
        for (int x = 0; x < NUM_BINS_X; x++)
        {
            for (int y = 0; y < NUM_BINS_Y; y++)
            {
                VectorPointData vpD = new VectorPointData();
                vpD.position = VectorField[x][y].position;
                vpD.Vel = VectorField[x][y].Vel;
                vpD.Speed = VectorField[x][y].Speed;
                vpD.IndexX = VectorField[x][y].IndexX;
                vpD.IndexY = VectorField[x][y].IndexY;
                vpD.VisitCount = VectorField[x][y].VisitCount;
                vpD.ParticleCount = VectorField[x][y].ParticleCount;
                vpD.createParticleOn = VectorField[x][y].createParticleOn;
                vpD.particleCreator = VectorField[x][y].particleCreator;
                vpD.longTermCreator = VectorField[x][y].longTermCreator;
                vpD.longTermOn = VectorField[x][y].longTermOn;
                vpD.activeID = VectorField[x][y].activeID;
                flatArray[x * NUM_BINS_X + y] = vpD;
            }
        }
        VectorFieldInitializer myObject = new VectorFieldInitializer();
        myObject.FlatVectorField = flatArray;
        json = JsonUtility.ToJson(myObject);
        Debug.Log(json);
        
        //string filePath = Application.dataPath + gameDataProjectFilePath;
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        File.WriteAllText(filePath, json);
    }

    void LoadRiver()
        // From Unity Documentation
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            VectorFieldInitializer loadedData = JsonUtility.FromJson<VectorFieldInitializer>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            
            
            for (int x = 0; x < NUM_BINS_X; x++)
            {
                for (int y = 0; y < NUM_BINS_Y; y++)
                {
                    VectorPointData vpD = loadedData.FlatVectorField[x * NUM_BINS_X + y];
                    VectorField[x][y].position = vpD.position;
                    VectorField[x][y].Vel = vpD.Vel;
                    VectorField[x][y].Speed = vpD.Speed;
                    VectorField[x][y].IndexX = vpD.IndexX;
                    VectorField[x][y].IndexY = vpD.IndexY;
                    VectorField[x][y].VisitCount = vpD.VisitCount;
                    VectorField[x][y].ParticleCount = vpD.ParticleCount;
                    VectorField[x][y].activeID = vpD.activeID;



                    VectorField[x][y].particleCreator = vpD.particleCreator;
                    VectorField[x][y].longTermCreator = vpD.longTermCreator;
                    VectorField[x][y].longTermOn = vpD.longTermOn;
                    if (vpD.longTermCreator) VectorField[x][y].createParticleOn = true;
                    //flatArray[x * NUM_BINS_X + y] = vpD;
                    //VectorField[x][y] = loadedData.FlatVectorField[x * NUM_BINS_X + y];
                }
            }
            //VectorField = loadedData;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
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

    public void UpdateVectorField(Vector3 personPos,Vector3 oldPos, int id)
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
        VectorField[xIndex][yIndex].SetVelocity(offset);
        VectorField[xIndex][yIndex].createParticleOn = true;
        VectorField[xIndex][yIndex].longTermOn = true;
        VectorField[xIndex][yIndex].activeID = id;
        //if (GetVectorIndex(oldPos))
        VectorField[xIndex][yIndex].VisitCount++;

        //update surrounding points 

        int rangeOffset = (int)(riverRange/GetBinW());
        for (int xI = -rangeOffset; xI < rangeOffset; xI++)
        {    
            for (int yI = -rangeOffset; yI < rangeOffset; yI++)
            {
                if (xIndex + xI < NUM_BINS_X && xIndex + xI > 0 
                    && yIndex + yI < NUM_BINS_Y && yIndex + yI > 0)
                {
                    VectorField[xIndex + xI][yIndex + yI].SetVelocity(offset);
                }
            }
        }

    }



}
