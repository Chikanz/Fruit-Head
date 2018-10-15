using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity
{
    public class ActivateCharacter : MonoBehaviour
    {
        public bool startActive;
        SkinnedMeshRenderer[] meshes;
        MeshRenderer[] moreMeshes;
        Transform originalPosition;
        GameObject dialogue; //Probably avoid using static vars where you can
        SpriteRenderer spriteRend;

        // Use this for initialization
        void Start()
        {            
            //Use scene changer's singleton reference instead of slow gameobject.find
            dialogue = SceneChanger.instance.transform.GetChild(0).gameObject; 
            meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            moreMeshes = gameObject.GetComponentsInChildren<MeshRenderer>();
            spriteRend = gameObject.GetComponentInChildren<SpriteRenderer>();

            Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
            int stage = (int) biStage.AsNumber; //avoid comparison of floating point numbers by casting to int 
            if (stage == 0)
            {
                //hide avery and kim on blossom island
                if (gameObject.name != "Spot")
                {
                    Yarn.Value forestComplete = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$foundDog");
                    bool temp = forestComplete.AsBool;

                    if (!temp)
                    {
                        setMesh(false, false);
                    }
                }
                else
                {
                    setMesh(false, true);
                }
            }
            else
            {
                if (!startActive && gameObject.name != "Avery")
                {
                    if ((stage > 2 && gameObject.name == "Maison") || (stage == 10 && gameObject.name == "Eden"))
                    {
                        setMesh(true, false);
                    }
                    else
                    {
                        setMesh(false, true);
                    }
                }
            }

            originalPosition = gameObject.transform;
        }

        [YarnCommand("show")]
        public void show()
        {
            //print (gameObject.name);
            if (gameObject.name == "Kim")
            {
                //placing kim just outside camera view
                //Vector3 temp = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 15.0f));
                Vector3 temp = Camera.main.ViewportToWorldPoint(new Vector3(5.0f, 0.5f, 1.1f));
                gameObject.transform.position = temp;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                    originalPosition.position.y, gameObject.transform.position.z);
                setMesh(true, false);
            }
            else if (gameObject.name == "Avery")
            {
                gameObject.transform.position += new Vector3(10, 0, 0);
                setMesh(true, false);
            }
            else
            {
                setMesh(true, true);
            }

            
        }

        public void setMesh(bool isVisible, bool move)
        {
            if (move)
            {
                if (!isVisible)
                {
                    transform.position =
                        new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
                }
                else
                {
                    transform.position =
                        new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
                }
            }

            foreach (var m in meshes)
            {
                m.enabled = isVisible;
            }

            foreach (var m in moreMeshes)
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