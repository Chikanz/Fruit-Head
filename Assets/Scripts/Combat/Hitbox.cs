using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public delegate void EnemyHit(GameObject enemy);
    public EnemyHit OnHit;

    HashSet<GameObject> BeenHit = new HashSet<GameObject>(); //List of who's been hit

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        var objs = Physics.OverlapBox(transform.position,
            GetComponent<BoxCollider>().bounds.extents, transform.rotation, ~9);
        
        foreach(Collider c in objs)
        {
            if (BeenHit.Contains(c.gameObject)) continue;                           //Already been hit!
            if (!c.GetComponentInParent<CombatCharacter>()) continue;               //Only care about characters
            if (c.gameObject.CompareTag("Player")) continue;                       //Stop hitting yourself! 

            if (OnHit != null)
            {
                OnHit(c.gameObject);
            }

            BeenHit.Add(c.gameObject); //Add to hit list
        }
	}

    //Clear list if we're a hitbox
    private void OnDisable()
    {
        if (BeenHit != null)
        {
            BeenHit.Clear();
        }
    }
}
