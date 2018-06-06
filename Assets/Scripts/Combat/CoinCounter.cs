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

	// Use this for initialization
	void Start ()
    {
        //Enforce singleton
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

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
        myChild.text = totalCoins() > 0 ? "$" + totalCoins() : "u broke";
    }

    public static CoinCounter GetInstance()
    {
        return instance;
    }

    int totalCoins()
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
        while (displayCoins < totalCoins())
        {
            displayCoins += 1;
            myChild.text = "$" + displayCoins;
            yield return new WaitForSeconds(delay);
        }
        updatingDisplay = false;
    }

    private void Coin_OnSceneChange(object sender, System.EventArgs e)
    {
        VS.SetValue("$money", new Yarn.Value(coins + startCoins));
    }
}
