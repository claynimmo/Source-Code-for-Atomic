using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBackground : MonoBehaviour
{
    public Material[] skyboxes;
    void Awake(){
        RenderSettings.skybox=skyboxes[(int)Random.Range(0,skyboxes.Length)];
    }
}
