using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour {

    public Transform person;
    public Transform start;
    public PathNode startNode;

    int personCount = 1;

    void CreateNewPerson()
    {
        Transform newPerson = Instantiate(person, start.position, Quaternion.identity);
        newPerson.GetComponent<Person>().nextNode = startNode;
        newPerson.GetComponent<Person>().id = personCount;
        personCount++;
    }

	// Use this for initialization
	void Start () {
        //CreateNewPerson();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) CreateNewPerson();
    }
}
