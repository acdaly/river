using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPoint : MonoBehaviour {

    public Vector2 Vel = new Vector2(0, 0);
    public float Speed = 5f;
    public Vector3 position;
    public int IndexX;
    public int IndexY;
    public int VisitCount = 0;
    public Particle P;
    public int ParticleCount = 1;
    public bool createParticleOn = false;   // person intercepted with this node
    public bool particleCreator = false;    // vector point that creates particles
    public bool longTermOn = false;         // river constant flow down path 
    public bool longTermCreator = false;    // creates particles for minimum river flow

    public int activeID = 0;
    public Gradient normalGradient;
    public Gradient[] GradientList;
    public float particleOffset = 0.4f;
    private TrailRenderer tr;

    public void SetVelocity(Vector2 velocity)
    {
        Vel = velocity;
    }

    void InstantiateParticle(float offset)
    {
        Vector3 pPos = position;
        pPos += new Vector3(Random.Range(-offset, offset),
            0, Random.Range(-offset, offset));
        if (activeID != 0) pPos.y = 0.5f;
        Particle newP = Instantiate(P, pPos, transform.rotation);
        tr = newP.GetComponent<TrailRenderer>();
        tr.material = new Material(Shader.Find("Sprites/Default"));
        if (activeID == 0) tr.colorGradient = normalGradient;
        else tr.colorGradient = GradientList[activeID % GradientList.Length];
    }

    void CreateParticles()
    {
        if (longTermCreator && longTermOn && Vel != new Vector2(0, 0))
        {
            for (int i = 0; i < ParticleCount; i++)
            {
                InstantiateParticle(particleOffset);
            }
        }
        else if (Vel != new Vector2(0, 0) && particleCreator && createParticleOn)
        {
            for (int i = 0; i < ParticleCount+VisitCount; i++)
            {
                float visitorOffset = VisitCount / 5 + particleOffset;
                InstantiateParticle(visitorOffset);
            }
        }
    }

    IEnumerator turnOffLongTermFlow()
    {
        yield return new WaitForSeconds(10);
        if (VisitCount <= 0) longTermOn = false;

    }


    void DecrementVisits()
    {
        if (VisitCount > 0)
        {
            VisitCount--;
        }

    }

    IEnumerator useNormalGradient()
    {
        yield return new WaitForSeconds(1f);
        activeID = 0;

    }

    // Use this for initialization
    void Start () {
        
        tr = P.GetComponent<TrailRenderer>();
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.colorGradient = normalGradient;
        position = transform.position;
        InvokeRepeating("DecrementVisits", 1f, 5f);
        InvokeRepeating("CreateParticles", 1f, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
        if (activeID != 0)
        {
            StartCoroutine(useNormalGradient());
            
        }
        if (createParticleOn && VisitCount <= 0)
        {
            StartCoroutine(turnOffLongTermFlow());
            createParticleOn = false;
        }
			}
}
