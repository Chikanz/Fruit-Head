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
                if (!startActive && gameObject.name != "Avery" /*&& gameObject.name != "Eden" && gameObject.name != "Maison"*/)
                {
                    setMesh(false);
                }
                //else if ((gameObject.name == "Eden" && stage < 3) || (gameObject.name == "Maison" && stage < 3)) {
                //    setMesh(false);
                //} 

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

            if (spriteRend)
            {
                spriteRend.enabled = isVisible;
            }
        }

    }
}