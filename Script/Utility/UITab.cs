using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITab : MonoBehaviour
{
    public GameObject hide;
    public GameObject show;

    AudioSource source;
    public AudioClip clip;

    void Awake(){
        source = GetComponent<AudioSource>();
    }
    
    public void ButtonPressed(){
        hide.SetActive(false);
        show.SetActive(true);
        source.PlayOneShot(clip,Variables.sfxVolume);
    }
}
