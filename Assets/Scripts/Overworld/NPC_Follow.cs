using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC_Follow : MonoBehaviour {

    GameObject Charlie;
    float targetDistance;
    float followSpeed;
    Animator m_Animator;

    bool follow;

    // Use this for initialization
    void Start() {
        targetDistance = 4.0f;
        followSpeed = 3.8f;

        Charlie = GameObject.Find("Charlie");

        m_Animator = GetComponent<Animator>();

        if (gameObject.name == "Eden")
        {
            follow = false;
        }
        else
        {
            follow = true;
        }
    }

    // Update is called once per frame
    void Update() {

        if (follow)
        { 
            float distance = Vector3.Distance(transform.position, Charlie.transform.position);
            if (distance > targetDistance)
            {
                transform.LookAt(Charlie.transform.position);
                //transform.position = Vector3.MoveTowards(transform.position, Charlie.transform.position, followSpeed * Time.deltaTime);
                Vector3 move = Vector3.MoveTowards(transform.position, Charlie.transform.position, followSpeed * Time.deltaTime);
                transform.position = move;
                if (m_Animator)
                {
                    UpdateAnimator(move);
                }
                
            }
            else
            {
                if (m_Animator)
                {
                    if (m_Animator.GetFloat("Forward") > 0)
                    {
                        stopMoving();
                    }
                }
            }
        }

    }


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

    public void stopMoving()
    {
        //stop walking animation
        m_Animator.SetFloat("Forward", 0, 0, 0);
        m_Animator.SetFloat("Turn", 0, 0, 0);
    }

    [YarnCommand("setFollow")]
    public void setFollow(string toFollow)
    {
        if (toFollow == "true")
        {
            follow = true;
        }
        else
        {
            follow = false;
        }
    }
}
