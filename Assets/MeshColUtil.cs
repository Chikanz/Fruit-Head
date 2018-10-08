using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshColUtil : MonoBehaviour
{

	public Color[] Colors;
	
	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter>().mesh.RecalculateNormals(180);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	[ExecuteInEditMode]
	[ContextMenu("Get cols")]
	void getCols()
	{
		var vertcols = GetComponent<MeshFilter>().mesh.colors;
		Colors = vertcols.Distinct().ToArray();
	}

	[ContextMenu("fuck sdhit")]
	void fuckmyshitup()
	{
		var mf = GetComponent<MeshFilter>();
		for (int i = 0; i < mf.mesh.vertexCount; i++)
		{
			mf.mesh.vertices[i] = Vector3.back;        
		}
	}
}
