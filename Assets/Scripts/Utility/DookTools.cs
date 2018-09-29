using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DookTools
{

    public static float GetAnimationLength(Animator anim, string name)
    {
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                //For all animations
        {
            if (ac.animationClips[i].name == name)                        //If it has the same name as your clip
            {
                return ac.animationClips[i].length;
            }
        }

        Debug.Log("Couldn't find animation clip!");
        return -1;
    }
    
    public static float Map(float value, float OldMin, float OldMax, float NewMin, float NewMax)
    {
        float oldRange = (OldMax - OldMin);
        float newRange = (NewMax - NewMin);
        float newValue = (((value - OldMin) * newRange) / oldRange) + NewMin;

        return (newValue);
    }
}
