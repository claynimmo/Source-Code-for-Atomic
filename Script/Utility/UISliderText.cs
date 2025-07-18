using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UISliderText : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI txt;
    void Awake(){
        UpdateText();
    }
    public void UpdateText(){
        txt.text = $"{slider.value}";
    }
}
