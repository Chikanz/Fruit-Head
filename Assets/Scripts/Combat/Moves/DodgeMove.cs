using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class DodgeMove : Move
{
	private bool manhandle = false; //is overriding thirdperson controller
	public float distance = 1.5f;
	public float stopMag = 2;
	private Vector3 moveVec;
	private Transform mainCam;
	private Animator myAnim;
	
	private float dodgeX;
	private float dodgeY;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		myAnim = daddy.GetComponent<Animator>();
	}

	// Update is called once per frame
	protected override void Update() 
	{
		base.Update();

		if (manhandle)
		{		
			daddy.transform.position += moveVec * Time.deltaTime * distance;
			moveVec -= (moveVec * (Time.deltaTime * 5) );
			if (moveVec.sqrMagnitude < Mathf.Pow(stopMag, 2))
			{
				Reset();
			}
		}
		else
		{
			float v = 0, h = 0;
			moveVec = Vector3.zero;	
			if (Input.GetKey(KeyCode.W))
			{
				v += 1;
			}
			if (Input.GetKey(KeyCode.S))
			{
				v -= 1;
			}
			if (Input.GetKey(KeyCode.A))
			{
				h -= 1;
			}
			if (Input.GetKey(KeyCode.D))
			{
				h += 1;
			}
			// calculate camera relative direction to move:
			var camFwd = Vector3.Scale(mainCam.forward, new Vector3(1, 0, 1)).normalized;
			moveVec = v * camFwd +  h * mainCam.right;
			moveVec.Normalize();

			dodgeX = h;
			dodgeY = v;
		}
	}

	public override void Execute()
	{
		base.Execute();

		Invoke("dodge",0.12f);
		myAnim.SetTrigger("Dodge");
		myAnim.SetFloat("DodgeX",dodgeX);
		
		//daddy.GetComponent<Rigidbody>().AddForce(Vector3.left * 3000);
		//Invoke("Reset",0.5f);
		
	}

	void dodge()
	{
		toggleMoveSystems(false);

		moveVec *= 10; 
		
		//Set anim state		
	}

	void Reset()
	{
		toggleMoveSystems(true);
	}

	void toggleMoveSystems(bool e)
	{
		manhandle = !e;
		daddy.GetComponent<ThirdPersonUserControl>().enabled = e;
		daddy.GetComponent<ThirdPersonCharacter>().enabled = e;
	}
}
