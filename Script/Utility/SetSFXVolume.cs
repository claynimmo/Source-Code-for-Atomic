using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSFXVolume : MonoBehaviour
{
    void Awake(){
        GetComponent<AudioSource>().volume = Variables.sfxVolume;
    }
}
