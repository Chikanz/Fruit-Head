using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// everyone knows tater is short for rotater 
/// </summary>
public class TreeTater : EditorWindow 
{
    [MenuItem("ZacTools/TreeTater")]
    public static void tate()
    {
        foreach (Transform t in GameObject.Find("GreenStuff").transform)
        {
            t.rotation = Quaternion.Euler(0,Random.Range(0,360), 0);
        }
    }
}
