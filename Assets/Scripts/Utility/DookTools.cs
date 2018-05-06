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
}
