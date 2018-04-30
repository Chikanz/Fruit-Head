using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MADHAX : MonoBehaviour
{
    // Use this for initialization
    bool isOverWorld = true;
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("BattleChairs");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("Overworld");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("BattleFlies");
        }
    }
}
