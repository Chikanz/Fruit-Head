using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;

using UnityEngine;

public class Targeter : MonoBehaviour
{
	private EnemyManager EM;
	public Camera cam;
	public CinemachineTargetGroup CTG;
	
	[Tooltip("The gameobject that will follow the targeted enemy")]
	public GameObject TargetPrebab; 
	private GameObject targetObj; //Spawned target reticle

	//detirmines which enemy is being targeted, -1 for no enemy
	private int targetInt;

	private bool cameraSnapped;
	private List<DotTarget> Targets;	
	
	// Use this for initialization
	void Start ()
	{
		EM = GameObject.Find("Enemies").GetComponent<EnemyManager>();
		Debug.Assert(EM, "enemy manager not found. Tell zac he's a lazy piece of shit");
		Targets = new List<DotTarget>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Snap
		if (Input.GetKey(KeyCode.LeftShift))  
		{
			if (!cameraSnapped)
			{
				Snap();
			}
		}
		else if(cameraSnapped) //Key up
		{
			ResetSnap();
		}
		
		//Cycle
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Snap();
		}
		
		//Update reticle
		if (cameraSnapped)
		{
			if (CurrentTarget())
			{
				targetObj.transform.position = CurrentTarget().transform.position;
			}
			else
			{
				Snap(); //Lost current target, cycle snap
				Targets = GetTargetsByDot(); //Refresh targets list (maybe bad idea?)
			}
		}
	}

	//Snap the camera or cycle target
	void Snap()
	{
		//First snap
		if (!cameraSnapped)
		{
			Targets = GetTargetsByDot();	
			if(Targets.Count == 0) return; //Can't target nothing
			
			cameraSnapped = true;
			targetInt = 0;
			
			//Spawn target Reticle
			targetObj = Instantiate(TargetPrebab, CurrentTarget().transform.position, Quaternion.identity);
		}
		else //Already snapped, so cycle
		{
			targetInt++;
			if (targetInt > Targets.Count - 1) targetInt = 0; //loop de loop scoop de poop
		}		

		//Set the target in the cinemachine target group
		if(CTG.m_Targets.Length > 1)
			CTG.m_Targets[1] = MakeCMTarget(CurrentTarget().transform);
		else
		{
			Debug.Log("Array machine broke");
		}
	}

	void ResetSnap()
	{		
		cameraSnapped = false;
		Destroy(targetObj);
		CTG.m_Targets[1] = MakeCMTarget(null);
		Targets.Clear();
		targetInt = -1;
	}
	
	/// <summary>
	/// Returns a sorted list of enemies based on their angle from the current camera position
	/// </summary>	
	private List<DotTarget> GetTargetsByDot()
	{
		List<DotTarget> dtList = new List<DotTarget>();
		
		//Get dot for each enemy
		foreach (var enemy in EM.Enemies)
		{
			var v = enemy.transform.position - cam.transform.position;
			var dt = new DotTarget
			{
				dotFromCamera = Vector3.Dot(cam.transform.forward, v.normalized),
				enemy = enemy.gameObject
			};

			dtList.Add(dt);
		}
		
		//Sort that bad boy with some linq magic
		//https://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object		
		return dtList.OrderByDescending(e=>e.dotFromCamera).ToList();
	}
	
	//ehhhh
	struct DotTarget
	{
		public float dotFromCamera;
		public GameObject enemy;
	}
	
	#region Helper Methods

	GameObject CurrentTarget()
	{
		if(targetInt < Targets.Count) return Targets[targetInt].enemy;
		return null;
	}

	CinemachineTargetGroup.Target MakeCMTarget(Transform t)
	{
		return new CinemachineTargetGroup.Target
		{
			target = t,
			radius = 1,
			weight = 0.5f
		};
	}

	#endregion
}
