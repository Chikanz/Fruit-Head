using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public delegate void EnemyHit(GameObject enemy);
    public EnemyHit OnHit;

    HashSet<GameObject> BeenHit = new HashSet<GameObject>(); //List of who's been hit

    private Transform daddy; //our root character combat script

    // Use this for initialization
    void Start ()
    {
        daddy = GetComponentInParent<Move>().daddy;
    }
	
	// Update is called once per frame
	void Update ()
	{	    
        var objs = Physics.OverlapBox(transform.position,
            GetComponent<BoxCollider>().bounds.extents, transform.rotation, ~9); //Cast only on hitbox layer
        
        foreach(Collider c in objs)
        {
            if (BeenHit.Contains(c.gameObject)) continue;                           //Already been hit!
            if (!c.GetComponentInParent<CombatCharacter>()) continue;               //Only care about characters
            if (c.transform.parent && c.transform.parent.name == daddy.name) continue;   //Stop hitting yourself!

            if (c.transform.parent && c.transform.parent.name != daddy.name)
                Debug.Log("hit " + c.transform.parent.name);

            if (OnHit != null) //Send out hit event
            {
                OnHit(c.gameObject);
            }

            BeenHit.Add(c.gameObject); //Add to hit list
        }
	}

    //Clear list if we're a hitbox    
    private void OnEnable()
    {
        if (BeenHit != null)
        {
            BeenHit.Clear();
        }
    }
}
