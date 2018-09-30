using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
[RequireComponent(typeof(Animator))] //use require component if a component is mandatory (animator definitely should be)
[RequireComponent(typeof(Rigidbody))]
public class NPC_Follow : MonoBehaviour 
{
    public float targetDistance = 4.0f; //Serialize fields instead of hard coded magic numbers
    public float followSpeed = 3.8f;
    
    //good practice to specify access modifiers even if your fields are private and don't need them
    private Animator myAnim;
    private GameObject Charlie;
    private bool follow;
    private Terrain terrain;

    // Use this for initialization
    void Start() 
    {
        Charlie = NPCman.instance.GetCharacter("Charlie").gameObject; //Get charlie ref from NPC man
        terrain = FindObjectOfType<Terrain>(); //this is hilariously slow never use this shhhhh 
        myAnim = GetComponent<Animator>();
        //highly recomend navmesh instead
        
        //Debug.Assert(RB,"needs rigidbody");
        //RB = GetComponent<Rigidbody>();
        //RB.isKinematic = true;        
        
        //require component will only add animators to new NPC_follow components, so chuck in an assert just in case with a friendly message
        Debug.Assert(myAnim, gameObject.name + " is missing an animator component! Add it even if you don't have anims yet uwu");

        follow = gameObject.name != "Eden"; //don't follow if eden (Boolean expressions are much cleaner than if else)
    }

    // Update is called once per frame
    void Update() 
    {
        if (!follow) return; //Don't bother updating if we don't need to follow
        
        float distance = Vector3.Distance(transform.position, Charlie.transform.position);

        Vector3 followPos = Vector3.zero;
        //if (distance > targetDistance) //this is great, but will create jumpy behaviour. we can ease the move vector instead        
        
        //Move vector
        var followVec = ((Charlie.transform.position - transform.position).normalized * followSpeed * Time.deltaTime) 
                        * Mathf.Clamp(distance - targetDistance, 0,1); //simple easing
        followPos = transform.position + followVec; //Final vector        
                    
        //We can sample the terrain height so we don't have to use physics to keep the NPC on the floor
        //Actually using a RB was a lot cleaner than I thought lol
        float y = terrain.SampleHeight(followPos);
        transform.position = new Vector3(followPos.x, followPos.y, followPos.z); 
                
        //Look at is great, but this will rotate on all axis (axises?) even though we only want 1  
        //transform.LookAt(Charlie.transform.position); //you can put this on a lerp to make it smoother
        
        var lookVec = Charlie.transform.position - transform.position; //Get the vector along where we want to look  
        var rot = Quaternion.LookRotation(lookVec.normalized); //get the rotation that's looking at our target
        transform.rotation = Quaternion.Euler(0,rot.eulerAngles.y, 0); //only apply it to the Y rotation axis
        
        //Update the animator if we have one (which we should!)
        if (myAnim) UpdateAnimator(followVec);
    }

    //This is way too complicated for what you're trying to do, have a look at the unity animation tutorials if you're confused 
    /*void UpdateAnimator(Vector3 move)
    {
        float m_TurnAmount = Mathf.Atan2(move.x, move.z); //We don't need a turn amount since you're using a LookAt in update
        float m_ForwardAmount = move.z; //Forward amount is (was) always a boolean value (walking or stopped) so you didn't need a variable blend state
        
        //blah blah who cares about the other shit here
    }*/
   
    void UpdateAnimator(Vector3 move)
    {
        myAnim.SetFloat("Walking", move.magnitude * 20); //Multiply magnitude by enough to make it 1 while walking 
    }

    //Update animator handles stopping and starting so we don't need this
//    public void stopMoving()
//    {
//        //stop walking animation
//        myAnim.SetFloat("Forward", 0, 0, 0);
//        myAnim.SetFloat("Turn", 0, 0, 0);
//    }

    [YarnCommand("setFollow")]
    public void setFollow(string toFollow)
    {
        follow = toFollow == "true";
    }
}
