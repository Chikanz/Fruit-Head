using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Yarn.Unity
{
	public class MoveCharacter : MonoBehaviour
	{
		private Transform target;		
		private GameObject dialogue;
		private NavMeshAgent agent;
		private Animator myAnim;

        private bool shouldDestroy;

		private Transform ForceLookAtTarget;
		private NavMeshObstacle barrier;

		private const float destinationRange = 2.0f;

		// Use this for initialization
		void Start()
		{
			if (!dialogue) dialogue = GameObject.Find("Yarn");
			agent = gameObject.GetComponent<NavMeshAgent>();
			agent.enabled = false;

			barrier = GetComponent<NavMeshObstacle>();
			myAnim = GetComponent<Animator>();
		}

		// Update is called once per frame
		void Update()
		{
			//Turn on/off barrier when moving
			if (barrier) barrier.enabled = !IsMoving(); 

			//Look rotation
			if (ForceLookAtTarget)
			{
				var v = ForceLookAtTarget.transform.position - transform.position;   
				var rot = Quaternion.LookRotation(v.normalized); 
				var targetRot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
				transform.rotation = Quaternion.Lerp(targetRot, transform.rotation, 0.1f);
				
				if (Vector3.Dot(v, transform.forward) > 0.9f) ForceLookAtTarget = null; //If we're looking at our target, stop following
			}

			if (IsMoving())
			{					
				//Animator stuff
				if(GetComponent<ThirdPersonUserControl>()) myAnim.SetFloat("Forward", agent.velocity.sqrMagnitude);
				if (myAnim) myAnim.SetFloat("Walking", agent.velocity.sqrMagnitude);
				
				if (Vector3.Distance(transform.position, agent.destination) < destinationRange)
				{
					if (gameObject.name == "Kim" && target.GetComponent<OW_Character>().Name == "Charlie")
					{
						string startNode = gameObject.GetComponent<OW_NPC>().StartNode;
						//dialogue.GetComponent<DialogueRunner>().StartDialogue(startNode);
					}

                    if (shouldDestroy)
                    {
                        Destroy(gameObject, 1);
	                    Debug.Log("Destroying " + gameObject.name);
                    }

					stopMoving();
				}
			}
		}

		bool IsMoving()
		{
			return target != null;
		}

		[YarnCommand("move")]
		public void movetopoint(string destination, string toDestroy)
		{
			agent.enabled = true;
			
			print(destination);
			target = GameObject.Find(destination).transform;
			agent.SetDestination(target.transform.position);

			if (destination == "Rowboat")
			{
				agent.speed *= 2;
			}

            if (toDestroy == "destroy")
            {
                shouldDestroy = true;
            }
           

			//Transform location = target.gameObject.transform;
			//this.GetComponent<NavMeshAgent> ().destination = location.position;
		}



		[YarnCommand("stop")]
		public void stopMoving()
		{
			target = null;
			agent.enabled = false;
			myAnim.SetFloat("Walking",0.0f);
		}

		[YarnCommand("lookAt")]
		public void look(string temp)
		{
            if (agent)
            {
                agent.enabled = false;
            }
			ForceLookAtTarget = GameObject.Find(temp).transform;
		}
	}
}
