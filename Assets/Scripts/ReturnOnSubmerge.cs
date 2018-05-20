using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnOnSubmerge : MonoBehaviour
{
    Vector3 lastSafePoint;
    public float waterPlaneY;

    // Use this for initialization
    void Start ()
    {
        InvokeRepeating("FindSafePoint", 2, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FindSafePoint()
    {
        RaycastHit hitInfo;
        if (transform.position.y < waterPlaneY) return;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.3f))
        {
            if(hitInfo.collider.tag.Equals("Terrain"))
            {
                lastSafePoint = transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in water");
        if (other.tag.Equals("Water"))
        {
            Invoke("ReturnToLand", 2);
        }
    }

    void ReturnToLand()
    {
        transform.position = lastSafePoint;
        Debug.Log("land");
    }
}
