using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yarn.Unity {

public class ActivateCharacter : MonoBehaviour {

		public bool startActive;
		SkinnedMeshRenderer[] meshes;
		Transform originalPosition;
		static GameObject dialogue;
        SpriteRenderer spriteRend;



    // Use this for initialization
    void Start () {
            if (!dialogue) dialogue = GameObject.Find("Yarn");
            meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            spriteRend = gameObject.GetComponentInChildren<SpriteRenderer>();

            Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
            float stage = biStage.AsNumber;
            if (stage == 0)
            {
                //activate avery on blossom island
                Yarn.Value forestComplete = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$foundDog");
                bool temp = forestComplete.AsBool;
                if (!temp)
                {
                    setMesh(true);
                }
            }
            else
            {
                if (!startActive && gameObject.name != "Avery")
                {
                    setMesh(false);
                }

            }

            originalPosition = gameObject.transform;

	}
	
	


		[YarnCommand("show")]
		public void show() {
			//print (gameObject.name);
			if (gameObject.name == "Kim") {
				Vector3 temp = Camera.main.ViewportToWorldPoint (new Vector3 (1.1f, 0.5f, 15.0f));
				gameObject.transform.position = temp;
				gameObject.transform.position = new Vector3 (gameObject.transform.position.x, originalPosition.position.y, gameObject.transform.position.z);
			} 
			else if (gameObject.name == "Avery") {
				gameObject.transform.position += new Vector3(10, 0, 0);
			}

            setMesh(true);
            

        }

        //doesn't work with the thumb cam for some reason??
        //[YarnCommand("activate")]
        //public void makeActive(string who)
        //{
            
        //    if (who == "debate")
        //    {
        //        for (int i = 0; i < transform.childCount; i++)
        //        {
        //            print(transform.GetChild(i).gameObject.tag);
        //            if (transform.GetChild(i).gameObject.tag == "Townspeople" && transform.GetChild(i).gameObject.name != "Eden")
        //            {
        //                transform.GetChild(i).gameObject.SetActive(true);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < transform.childCount; i++)
        //        {
        //            if (transform.GetChild(i).gameObject.name == who)
        //            {
        //                transform.GetChild(i).gameObject.SetActive(true);
        //            }
        //        }
        //    }
        //}

        public void setMesh(bool isVisible)
        {

            if (!isVisible)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
                
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
            }

            foreach (var m in meshes)
            {
                m.enabled = isVisible;
            }

            spriteRend.enabled = isVisible;
        }

    }
}