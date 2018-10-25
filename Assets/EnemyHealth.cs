﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
	private CombatCharacter CC;
	
	// Use this for initialization
	void Start () 
	{
		CC = GetComponentInParent<CombatCharacter>();
		CC.OnHurt += RefreshUI;
		CC.OnDefeat += OnEnemyDefeated;
		
		RefreshUI(this, null);

		var text = GetComponentsInChildren<Text>();
		text[0].text = CC.friendlyName;
		text[1].text = "lvl. 1";
	}

	private void OnEnemyDefeated(object sender, EventArgs e)
	{
		Destroy(gameObject);
		CC.OnHurt -= RefreshUI;
	}

	private void RefreshUI(object sender, EventArgs e)
	{
		//Shamelessly copied from combat manager
		var slider = GetComponentInChildren<Slider>();
		slider.maxValue = CC.MaxHealth;
		slider.value = CC.Health;
		slider.image.color = Color.Lerp(Color.red, Color.green, (float) CC.Health / CC.MaxHealth);	
	}

	// Update is called once per frame
	void Update ()
	{
		transform.position = transform.parent.position + Vector3.up * 2; //REEEEEEEEEEEEEEEEEEEEEEEEEEEE
	}
}
