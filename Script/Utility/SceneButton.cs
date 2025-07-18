using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneButton : MonoBehaviour
{
    public int sceneToChange;


    public UIFade fade;
    public bool UIvisible = true;
    public void ChangeScene(){
        SceneManager.LoadScene(sceneToChange);
    }
    public void fadeUI(){
        UIvisible = !UIvisible;
        if(UIvisible){fade.ShowUI();} else{fade.hideUI();}

    }
}
