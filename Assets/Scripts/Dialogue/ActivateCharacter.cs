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
                    foreach (var m in meshes)
                    {
                        m.enabled = startActive;
                    }
                    spriteRend.enabled = startActive;
                }
            }
            else
            {
                if (!startActive && gameObject.name != "Avery")
                {
                    disableMesh();
                }

            }


			originalPosition = gameObject.transform;


	}
	
	


		[YarnCommand("activate")]
		public void activate() {
			//print (gameObject.name);
			if (gameObject.name == "Kim") {
				Vector3 temp = Camera.main.ViewportToWorldPoint (new Vector3 (1.1f, 0.5f, 15.0f));
				gameObject.transform.position = temp;
				gameObject.transform.position = new Vector3 (gameObject.transform.position.x, originalPosition.position.y, gameObject.transform.position.z);
			} 
			else if (gameObject.name == "Avery") {
				gameObject.transform.position += new Vector3(10, 0, 0);
			}

            enableMesh();

        }

        [YarnCommand("start")]
        public void makeActive()
        {
            //ameObject temp = GameObject.Find("Townspeople");
            print("activate");
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public void disableMesh()
        {
            foreach (var m in meshes)
            {
                m.enabled = false;
            }

            spriteRend.enabled = false;
        }


        public void enableMesh()
        {
            foreach (var m in meshes)
            {
                m.enabled = true;
            }

            spriteRend.enabled = true;
        }

    }
}