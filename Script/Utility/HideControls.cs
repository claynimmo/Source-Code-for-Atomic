using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideControls : MonoBehaviour
{
    public GameObject[] controlUI;
    void Awake(){
        if(Variables.hideUI){
            foreach(GameObject obj in controlUI){
                obj.SetActive(false);
            }
        }
        if(Variables.hideAllUI){
            gameObject.SetActive(false);
        }
    }
}
