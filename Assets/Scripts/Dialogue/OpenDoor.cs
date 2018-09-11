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
        if (target) 
        {
            Vector3 targetDirection = target.position - transform.position;
            float step = 0.5f * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.position, targetDirection, step, 0);

            //transform.rotation = Quarternion.LookRotation(newDirection);
        }
    }



    [YarnCommand("open")]
    public void open()
    {
        //target = transform.position + new Vector3(0, 90, 0);
    }

}
