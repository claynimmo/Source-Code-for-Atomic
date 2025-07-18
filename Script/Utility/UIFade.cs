using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    //code to fade ui elements, as opposed to instantly turning them on, by modifying the alpha of a canvas group
    //this script must be placed on the ui that gets faded (child objects also get faded);
    private CanvasGroup UIGroup;
    private bool fadeIn = false;
    private bool fadeOut = false;

    [SerializeField] private bool StartOut = false; //the canvasgroup should be set with an alpha of 0 if this is true
    [SerializeField] private float timer = 3;

    [Range(0.1f,5f)]
    [SerializeField] private float fadeSpeed = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        UIGroup = GetComponent<CanvasGroup>();
        if(StartOut){
            Invoke("hideUI",timer);
        }

    }

    //these functions are called from another script
    public void ShowUI(){
        fadeIn= true;
        fadeOut = false;
    }
    public void hideUI(){
        fadeOut = true;
        fadeIn=false;
    }

    void Update(){
        if(fadeIn){
            if(UIGroup.alpha < 1){
                UIGroup.alpha += Time.deltaTime*fadeSpeed;
                if(UIGroup.alpha>=1){
                    fadeIn = false;
                }
            }
            else if(UIGroup.alpha==1){
                fadeIn=false;
            }
        }
        if(fadeOut){
            if(UIGroup.alpha > 0){
                UIGroup.alpha -= Time.deltaTime*fadeSpeed;
                if(UIGroup.alpha<=0){
                    fadeOut = false;
                }
            }
            else if(UIGroup.alpha==0){
                fadeOut=false;
            }
        }
    }

    
}
