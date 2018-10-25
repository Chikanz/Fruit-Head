using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class OW_Character : MonoBehaviour {

    [SerializeField]
    private string name;

    public string Name
    {
        get {return name;}
        set {name = value;}
    }

    protected static DialogueRunner _DR;

    // Use this for initialization
    public virtual void Start ()
    {
        if(!_DR) _DR = SceneChanger.Yarn.GetComponent<DialogueRunner>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
