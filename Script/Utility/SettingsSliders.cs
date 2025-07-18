using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSliders : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider xsenSlider;
    public Slider ysenSlider;
    public Toggle UIToggle;
    public Toggle UIToggleFull;
    
    public AudioSource source;
    public AudioClip clip;
    void Awake(){
        source = GetComponent<AudioSource>();
    }
    public void SetValues(){
        Variables.musicVolume = musicSlider.value;
        Variables.sfxVolume = sfxSlider.value;
        Variables.xsensitivity = xsenSlider.value;
        Variables.ysensitivity = ysenSlider.value;
        Variables.hideUI = UIToggle.isOn;
        Variables.hideAllUI = UIToggleFull.isOn;

        source.PlayOneShot(clip,Variables.sfxVolume);
    }
}
