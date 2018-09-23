using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;

        private CombatCharacter CC;
        private Targeter targeter;

        public bool CanMove = true;

        
        private void Start()
        {
            Screen.lockCursor = true;

            CC = GetComponent<CombatCharacter>();
            targeter = GetComponent<Targeter>();            
            

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            //Get input for moves
            if (!CC) return;
            if(Input.GetMouseButtonDown(0))
            {
                GetComponent<CombatCharacter>().UseMove(0);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                GetComponent<CombatCharacter>().UseMove(1);
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs if not using move
            float v = 0, h = 0;
            if ((!CC || CC.CanMove()) && CanMove)
            {
               h = CrossPlatformInputManager.GetAxis("Horizontal");
               v = CrossPlatformInputManager.GetAxis("Vertical");                           
            }

            if (!targeter.cameraSnapped)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
                m_Character.Move(m_Move); // pass all parameters to the character control script
            }
            else
            {
                var vec = targeter.CurrentTarget().transform.position - transform.position;
                //Don't move if we're not facing
                if (Vector3.Dot(vec.normalized, transform.forward) > 0.7f)
                    m_Character.Strafe(h, v); //Strafe
                
                //Face target
                var rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vec.normalized), 0.1f).eulerAngles;
                rot.x = 0;
                rot.z = 0;
                transform.rotation = Quaternion.Euler(rot);
            }
        }
    }
}
