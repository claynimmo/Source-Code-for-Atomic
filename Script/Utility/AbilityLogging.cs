using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityLogging : MonoBehaviour
{
    public GameObject UI_Logg_Prefab;
    public GameObject UI_Parent;

    //a class for the values required in the log. to avoid multiple lists, a list of this class is used
    private class logValues{
        public string logName;
        public GameObject prefab;
        public Color logColour;
    }
    List<logValues> logs;

    //value to determine when log values should be removed;
    public float maxLog = 6; //value is the exact amount of values in the list


    public float AutoDecayTimer = 2;
    private float currentDecayTime;

    public float verticalDistanceOffset = 5;

    void Awake(){
        logs = new List<logValues>();
    }

    // Update is called once per frame
    void Update(){
        if(logs.Count==0){return;}

        //automatically decay logged values overtime, so the the screen is not always cluttered with logs
        currentDecayTime+=Time.deltaTime;
        if(currentDecayTime>=AutoDecayTimer){
            RemoveTopLog();
            currentDecayTime = 0;
        }
    }

    public void AppendLog(string value, Color? nullablecol = null){
        //log the time when the ability is logged for display
        string currentTime = DateTime.Now.ToString("h:mm:ss tt");
        nullablecol = nullablecol ?? Color.black;
        Color col = Color.black;
        if(nullablecol.HasValue){
            col = nullablecol.Value;
        }

        logValues log = new logValues {logName = "", prefab = null, logColour = Color.black};
        log.logName = value + " " + currentTime;
        log.prefab = Instantiate(UI_Logg_Prefab,UI_Parent.transform);

        TextMeshProUGUI logUI = log.prefab.GetComponentInChildren<TextMeshProUGUI>();
        logUI.text = log.logName;
        logUI.color = ContrastColor(col);
        log.prefab.GetComponentInChildren<Image>().color = col;

        float length = logs.Count;
        if(length<maxLog){
            logs.Add(log);
        }
        else{
            RemoveTopLog();
            logs.Add(log);
        }
        SetPositions();
    }

    private void DestroyLoggedValue(int index){
        Destroy(logs[index].prefab);
        logs[index].prefab = null;
        logs[index].logName = null;
    }

    private void SetPositions(){
        for(int i = (logs.Count-1); i>=0; i--){
            float y = (logs.Count - i);
            y *= verticalDistanceOffset;

            if(logs[i].prefab!=null)logs[i].prefab.transform.localPosition = new Vector3(0,y,0);
        }
    }

    //since the values are added in order, the oldest value has the lowest index. therefore the value at index 0 is destroyed
    private void RemoveTopLog(){
        DestroyLoggedValue(0);
        logs.RemoveAt(0);
        SetPositions();
    }

    //automatically get a colour for the text which contrasts with the background
    private Color ContrastColor(Color color){
        float grayScale = (color.r * 0.3f) + (color.g * 0.59f) + (color.b * 0.11f);
        return grayScale > 0.5f ? Color.black : Color.white;
    }
}
