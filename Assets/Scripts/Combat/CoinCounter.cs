using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    private int coins;
    private int startCoins;
    private int displayCoins;
    Text myChild;
    bool updatingDisplay = false;

    public int maxCoins = 20;

    static CoinCounter instance;

    ExampleVariableStorage VS;

    private void Awake()
    {
        //Enforce singleton
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start ()
    {
        VS = SceneChanger.instance.GetComponentInChildren<ExampleVariableStorage>();

        //Get coin count from yarn variable
        var m = VS.GetValue("$money");
        startCoins = (int)m.AsNumber;
        displayCoins = startCoins;

        //Add ill gotten gains to yarn var
        SceneChanger.instance.OnSceneChange += Coin_OnSceneChange;

        myChild = GetComponentInChildren<Text>();

        //Set initial coins
        if(myChild)
        myChild.text = TotalCoins() > 0 ? "$" + TotalCoins() : "u broke";
    }

    public static CoinCounter GetInstance()
    {
        return instance;
    }

    private int TotalCoins()
    {
        return startCoins + coins;
    }

	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddCoins(int mons)
    {
        //if (!myChild) return; //No update coins when we're not showing it

        if (!Finished())
        {
            if (mons + coins > maxCoins) //Don't go over max
                coins += maxCoins - coins;
            else
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
        while (displayCoins < TotalCoins())
        {
            displayCoins += 1;
            myChild.text = "$" + displayCoins;
            yield return new WaitForSeconds(delay);
        }
        updatingDisplay = false;
    }

    private void Coin_OnSceneChange(int scene)
    {
        VS.SetValue("$money", new Yarn.Value(coins + startCoins));
    }
}
