using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    private int coins;
    private int displayCoins;
    Text myChild;
    bool updatingDisplay = false;

    int maxCoins = 100;

    static CoinCounter instance;

	// Use this for initialization
	void Start ()
    {
        //Enforce singleton
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        myChild = GetComponentInChildren<Text>();
    }

    public static CoinCounter GetInstance()
    {
        return instance;
    }

	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddCoins(int mons)
    {
        if (!Finished())
        {
            coins += mons;
            if (!updatingDisplay) StartCoroutine(DisplayLag(0.1f));
        }
    }

    public bool Finished()
    {
        return coins >= maxCoins;
    }

    IEnumerator DisplayLag(float delay)
    {
        updatingDisplay = true;
        while (displayCoins < coins)
        {
            displayCoins += 1;
            myChild.text = "$" + displayCoins;
            yield return new WaitForSeconds(delay);
        }
        updatingDisplay = false;
    }
}
