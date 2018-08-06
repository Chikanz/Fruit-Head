using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class basicMove : MonoBehaviour {

    public Transform[] points;
    NavMeshAgent NM;
    int counter = 0;
 

    // Use this for initialization
    void Start () {

        NM = GetComponent<NavMeshAgent>();
        NM.SetDestination(points[counter].position);
    
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Vector3.Distance(transform.position, points[counter].position) < 0.5f)
        {
            
            counter++;
            if (counter > points.Length - 1) counter = 0;
            NM.SetDestination(points[counter].position);

          
        }
	}




}

