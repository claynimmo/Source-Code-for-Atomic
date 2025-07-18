using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneButton : MonoBehaviour
{
    public int scenenum;
    public void ButtonPressed(){
        SceneManager.LoadScene(scenenum);
    }
}
