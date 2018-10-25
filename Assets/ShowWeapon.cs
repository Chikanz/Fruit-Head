using System.Collections;
using System.Collections.Generic;
using Luminosity.IO;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class ShowWeapon : MonoBehaviour
{
	public GameObject Weapon;
	private ThirdPersonUserControl _TPUC;
		
	public bool RotateOnHit;
	private bool isHitting = false;

	public float lerpSpeed = 0.1f;
	//public float 

	private Quaternion startRot;
	
	// Use this for initialization
	void Start ()
	{
		_TPUC = GetComponent<ThirdPersonUserControl>();
		startRot = Weapon.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Weapon.SetActive(_TPUC.CanAttack);

		if (InputManager.GetButton("Move0") && _TPUC.CanAttack && RotateOnHit && !isHitting)
		{
			StartCoroutine(LerpRot(0.1f, Quaternion.Euler(0, 0, 0)));
		}
	}

	IEnumerator LerpRot(float speed, Quaternion target)
	{
		isHitting = true;
		float timeElapsed = 0;
		
		//Lerp on
		while(Quaternion.Angle(Weapon.transform.localRotation, target) > 1)
		{
			Weapon.transform.localRotation = Quaternion.Lerp(Weapon.transform.localRotation, target, speed);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		
		//Wait for swing to finish
		var t = 1.25f - timeElapsed;
		print(t);
		yield return new WaitForSeconds(t);
		
		//Lerp off
		while(Quaternion.Angle(Weapon.transform.localRotation, startRot) > 1)
		{
			Weapon.transform.localRotation = Quaternion.Lerp(Weapon.transform.localRotation, startRot, speed);
			yield return null;
		}
		

		isHitting = false;
		
	}
}
