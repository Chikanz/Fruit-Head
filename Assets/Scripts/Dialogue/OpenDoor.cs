using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class OpenDoor : MonoBehaviour {

    Transform target;
    

	// Use this for initialization
	void Start () {
        target = null;
	}

    // Update is called once per frame
    void Update()
    {
        //if (target) 
        //{
        //    Vector3 targetDirection = target.position - transform.position;
        //    float step = 0.01f * Time.deltaTime;

        //    Vector3 newDirection = Vector3.RotateTowards(transform.position, targetDirection, step, 0);

        //    transform.rotation = Quaternion.LookRotation(newDirection);
        //}
    }



    [YarnCommand("open")]
    public void open()
    {
        //target = transform.position + new Vector3(0, 90, 0);
        //transform.rotation = new Quaternion(0, 90, 0, 0);
        transform.Rotate(0, -90, 0);
        //target = GameObject.Find("Charlie").transform;
    }

    [YarnCommand("close")]
    public void close()
    {
        //target = transform.position + new Vector3(0, 90, 0);
        //transform.rotation = new Quaternion(0, 90, 0, 0);
        transform.Rotate(0, 90, 0);
        //target = GameObject.Find("Charlie").transform;
    }

}
