using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatCharacter))]
public class CoinGiver : MonoBehaviour {

    bool CanHit = true;
    //public float coinCoolDown = 1;
    public int CoinsToGive = 10;
    private float timer;
    CombatCharacter CC;
    CoinCounter CoinCount;

    // Use this for initialization
    void Start ()
    {        
        CoinCount = CoinCounter.GetInstance();

        CC = GetComponent<CombatCharacter>();
        CC.OnHurt += CC_OnHurt;
	}

    private void CC_OnHurt(object sender, System.EventArgs e)
    {
        if (!CanHit) return; //Make sure we can hit this obj

        CanHit = false;
        //timer = coinCoolDown;
        CoinCount.AddCoins(CoinsToGive);
        CC.ParticlesOnHit = CombatCharacter.HitParticles.SPARKS;
    }

    // Update is called once per frame
    void Update () 
    {
        if(!CoinCount) return;        
        
        /*
        if (timer > 0)
            timer -= Time.deltaTime;
        else if(!CanHit && timer <= 0 && !CoinCount.Finished()) //reset
        {
            CanHit = true;
            CC.ParticlesOnHit = CombatCharacter.HitParticles.COINS;
        }
        */

        //Set hit particles to sparks when we're done
        if(CoinCount.Finished() && CC.ParticlesOnHit == CombatCharacter.HitParticles.COINS)
        {
            CC.ParticlesOnHit = CombatCharacter.HitParticles.SPARKS;
        }
	}
}
