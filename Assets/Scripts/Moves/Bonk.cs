using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A physical A T T A C that hurts everyone in the hitbox
public class Bonk : Move
{
    //Object to spawn
    public GameObject BonkBox; 

    //How long the box lingers
    public int destroyTime;

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

}
