using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;

using UnityEngine;

public class Targeter : MonoBehaviour
{
	private EnemyManager EM;
	private Camera cam;
	public CinemachineTargetGroup CTG;
	
	[Tooltip("The gameobject that will follow the targeted enemy")]
	public GameObject TargetPrebab; 
	private GameObject targetObj; //Spawned target reticle

	//detirmines which enemy is being targeted, -1 for no enemy
	private int targetInt;

	public bool cameraSnapped { get; private set; }
	private List<DotTarget> Targets = new List<DotTarget>();
	
	// Use this for initialization
	void Start ()
	{
		cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		
		EM = GameObject.Find("Enemies").GetComponent<EnemyManager>();
		Debug.Assert(EM, "enemy manager not found. Tell zac he's a lazy piece of shit");
		
		//Set charlie as camera target
		CTG.m_Targets[0] = MakeCMTarget(transform.Find("Cam Point"));
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
			if (CurrentTarget() != null)
			{
				targetObj.transform.position = 
					CurrentTarget().GetComponentInChildren<BoxCollider>() ? 
					CurrentTarget().GetComponentInChildren<BoxCollider>().bounds.center : 
					CurrentTarget().transform.position;
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
			if(!targetObj)
				targetObj = Instantiate(TargetPrebab, CurrentTarget().transform.position, Quaternion.identity);
			else
				targetObj.SetActive(true);
		}
		else //Already snapped, so cycle
		{
			targetInt++;
			if (targetInt > Targets.Count - 1) targetInt = 0; //loop de loop scoop de poop
		}		

		//Set the target in the cinemachine target group
		if(CTG.m_Targets.Length > 1 && CurrentTarget())
			CTG.m_Targets[1] = MakeCMTarget(CurrentTarget().transform);
		else
		{
			ResetSnap(); //Stahp snap if we have no targets left
		}
	}

	void ResetSnap()
	{		
		cameraSnapped = false;
		//Destroy(targetObj);
		targetObj.SetActive(false);
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

	public GameObject CurrentTarget()
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
