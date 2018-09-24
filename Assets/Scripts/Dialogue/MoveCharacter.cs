using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yarn.Unity {
	public class MoveCharacter : MonoBehaviour {

		Transform target;
		float speed;
		static GameObject dialogue;
		NavMeshAgent agent;
        Animator m_Animator;


        // Use this for initialization
        void Start () {
			target = null;
			speed = 2.0f;

            if (!dialogue) dialogue = GameObject.Find("Yarn");
            //agent = gameObject.GetComponent<NavMeshAgent> ();

            m_Animator = GetComponent<Animator>();
        }

		// Update is called once per frame
		void Update () {
			if (target != null ) {

			float step = speed * Time.deltaTime;
			//Transform destination = target.gameObject.transform;
                transform.LookAt(target);
                Vector3 move = Vector3.MoveTowards(transform.position, target.position, step);
                transform.position = move;
                if (m_Animator)
                {
                    UpdateAnimator(move);
                }

                if (Vector3.Distance (transform.position, target.position) < 2.0f) {

                    if (gameObject.name == "Kim" && target.GetComponent<OW_Character>().Name == "Charlie") {
						string startNode = gameObject.GetComponent<OW_NPC> ().StartNode;
						dialogue.GetComponent<DialogueRunner>().StartDialogue (startNode);
					}
                    //print(target.gameObject.name);

                    if (gameObject.name == "Alvy" || gameObject.name == "Sam" || (gameObject.name == "Luca" && target.gameObject.name == "TownHallExt") || gameObject.name == "Tam"
                        || gameObject.name == "Riley" || gameObject.name == "Devon" || (gameObject.name == "Eden" && target.gameObject.name == "PoliceStationExt"))
                    {
                        Destroy(gameObject);
                    }

                    stopMoving();

                }
			}

		}


		[YarnCommand("move")]
		public void movetopoint(string destination) {

			print (destination);
			target = GameObject.Find (destination).transform;

            if (destination == "Rowboat")
            {
                speed = 5.0f;
            }

            if (destination == "Debate" && gameObject.name == "Eden")
            {
                
                //Transform temp = target;
                //temp.position += new Vector3(1, 0, 0);
                //target = temp;
            }

			//Transform location = target.gameObject.transform;
			//this.GetComponent<NavMeshAgent> ().destination = location.position;
		}


        //from ThirdPersonCharacter, so the animations play when it moves (with some stuff removed as I assume NPCs arent jumping)
        void UpdateAnimator(Vector3 move)
        {
            float m_TurnAmount = Mathf.Atan2(move.x, move.z);
            float m_ForwardAmount = move.z;
            // update the animator parameters
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            
            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            /*if (m_IsGrounded && move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }*/
            
        }

        [YarnCommand("stop")]
        public void stopMoving()
        {
            target = null;
            //stop walking animation
            m_Animator.SetFloat("Forward", 0, 0, 0);
            m_Animator.SetFloat("Turn", 0, 0, 0);
        }

        [YarnCommand("lookat")]
        public void look(string temp)
        {
            Transform a = GameObject.Find(temp).transform;
            transform.LookAt(a);
        }

    }
}
