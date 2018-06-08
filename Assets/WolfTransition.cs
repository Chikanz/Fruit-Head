using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTransition : MonoBehaviour
{
	private string _key;

	private static Dictionary<int, bool> activeMask = new Dictionary<int, bool>();

	public int Num;
	
	// Use this for initialization
	void Start ()
	{
		if (!activeMask.ContainsKey(Num))
		{
			activeMask.Add(Num, true);
		}

		bool state;
		if(activeMask.TryGetValue(Num, out state))
		
		transform.parent.gameObject.SetActive(state);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.transform.tag.Equals("Player")) return;

		activeMask[Num] = false; //updat value
		
		SceneChanger.instance.Change(4);
	}
}
