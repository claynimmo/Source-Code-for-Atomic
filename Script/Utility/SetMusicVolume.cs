using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicVolume : MonoBehaviour
{
    void Awake(){
        GetComponent<AudioSource>().volume = Variables.musicVolume;
    }
}
