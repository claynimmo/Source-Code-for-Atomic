using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public void EndGame(bool win){
        if(win){
            StartCoroutine("win");
        }
        else{
            StartCoroutine("end");
        }
    }
    IEnumerator end(){
        yield return new WaitForSeconds(5f);
        loadscene.scene = 1;
        SceneManager.LoadScene(0);

    }
    IEnumerator win(){
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
