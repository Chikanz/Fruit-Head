using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yarn.Unity
{
	public class MoveCharacter : MonoBehaviour
	{
		private Transform target;		
		private GameObject dialogue;
		private NavMeshAgent agent;
		private Animator myAnim;

		private const float destinationRange = 2.0f;

		// Use this for initialization
		void Start()
		{
			if (!dialogue) dialogue = GameObject.Find("Yarn");
			agent = gameObject.GetComponent<NavMeshAgent>();
			agent.enabled = false;

			myAnim = GetComponent<Animator>();
		}

		// Update is called once per frame
		void Update()
		{
			if (myAnim) myAnim.SetFloat("Walking", agent.velocity.sqrMagnitude);
			
			if (target != null)
			{				
				//AMYYYYYYYYYYYYYYYYYYYYYYYY AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
				if (Vector3.Distance(transform.position, agent.destination) < destinationRange)
				{
					if (gameObject.name == "Kim" && target.GetComponent<OW_Character>().Name == "Charlie")
					{
						string startNode = gameObject.GetComponent<OW_NPC>().StartNode;
						dialogue.GetComponent<DialogueRunner>().StartDialogue(startNode);
					}
					//print(target.gameObject.name);

//					if (target.gameObject.name == "CharlieTarget" && gameObject.name == "Charlie")
//					{
//						//dialogue.GetComponent<DialogueRunner>().StartDialogue("Hideout");
//					}
//
//					if (target.gameObject.name == "Debate" && gameObject.name == "Charlie")
//					{
//						target.position -= new Vector3(1.5f, 0, 0);
//					}

					if (gameObject.name == "Alvy" || gameObject.name == "Sam" ||
					    (gameObject.name == "Luca" && target.gameObject.name == "TownHallDoor") ||
					    gameObject.name == "Tam"
					    || gameObject.name == "Riley" ||
					    (gameObject.name == "Devon" && target.gameObject.name != "DevonTarget") ||
					    (gameObject.name == "Eden" && target.gameObject.name == "PoliceStationExt"))
					{
						Destroy(gameObject);
					}

					stopMoving();
				}
			}
		}


		[YarnCommand("move")]
		public void movetopoint(string destination)
		{
			agent.enabled = true;
			
			print(destination);
			target = GameObject.Find(destination).transform;
			agent.SetDestination(target.transform.position);

			if (destination == "Rowboat")
			{
				agent.speed *= 2;
			}

			if (destination == "Debate" && gameObject.name == "Eden")
			{
				//Vector3 temp = target.position + new Vector3(1, 0, 0);
				//target = temp;
				//temp.position += new Vector3(1, 0, 0);
				//target = temp;
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
			agent.enabled = false;
			
			Transform a = GameObject.Find(temp).transform;
			transform.LookAt(a);
		}
	}
}
