using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MoveBoat : MonoBehaviour {

    public Transform target;
    public GameObject Luca;
    private bool isMoving = false;
    private float speed = 2.0f;
    private float minDistance = 3.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!isMoving) return;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        Luca.transform.position = transform.position;

        if (Vector3.Distance(transform.position, target.position) < minDistance)
        {
            isMoving = false;
        }

	}


    [YarnCommand("moveBoat")] 
    public void moveTheBoat()
    {
        Luca.GetComponent<PositionCharacter>().setAtPoint("Rowboat");
        isMoving = true;

    }


}
