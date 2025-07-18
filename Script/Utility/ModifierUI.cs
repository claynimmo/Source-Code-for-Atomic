using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierUI : MonoBehaviour
{
    AudioSource source;
    public AudioClip clip;

    //class for the default variables so that a "reset to default" button could be used
    [System.Serializable]

    public class defaultVariables{
        //name for reference in editor, changing it has no impact. it is just for readability
        public string variableName;
        //value for the variable
        public float value; 

        public Slider variableSlider;

        // Constructor that takes 2 arguments
        public defaultVariables(string variableName, float value) {
            this.variableName = variableName;
            this.value = value;
            this.variableSlider = null;
        }
    }

    public defaultVariables[] defaultVar = new defaultVariables[]{
        new defaultVariables("enemyCount",5),
        new defaultVariables("playerCount", 3) ,
        new defaultVariables("AI_Tick_Max_Variance",7),
        new defaultVariables("AI_Tick_Min_Variance",3),
        new defaultVariables("healthModifyer",1),
        new defaultVariables("electronRechargeModifyer",1),
        new defaultVariables("shootTickModifyer",0.56f),
        new defaultVariables("swapModeTickModifyer",5),
        new defaultVariables("useAbilityTickModifyer",0.68f)
        
    };

    private float[] previousValues;
    private static bool firstRun = true;


    private void Awake() {
        CursorController.ShowCursor();
        source = GetComponent<AudioSource>();
        //only set the variables to the default if it is the first instance
        if(firstRun){
            firstRun = false;
            //set the variables to the default
            Variables.enemyCount = defaultVar[0].value;
            Variables.playerCount = defaultVar[1].value;
            Variables.AI_Tick_Max_Variance = defaultVar[2].value;
            Variables.AI_Tick_Min_Variance = defaultVar[3].value;
            Variables.healthModifyer = defaultVar[4].value;
            Variables.electronRechardModifyer = defaultVar[5].value;
            Variables.shootTickModifyer = defaultVar[6].value;
            Variables.swapModeTickModifyer = defaultVar[7].value;
            Variables.useAbilityTickModifyer = defaultVar[8].value;
        }
        previousValues = new float[]{Variables.enemyCount,Variables.playerCount,Variables.AI_Tick_Max_Variance,
            Variables.AI_Tick_Min_Variance,Variables.healthModifyer,Variables.electronRechardModifyer,
            Variables.shootTickModifyer,Variables.swapModeTickModifyer,Variables.useAbilityTickModifyer};

        for(int i=0; i< defaultVar.Length; i++){
            if(defaultVar[i].variableSlider!=null){
                defaultVar[i].variableSlider.value = previousValues[i];
            }
        }
    }


    public void SetModifyers(){
        Variables.enemyCount = defaultVar[0].variableSlider.value;
        Variables.playerCount = defaultVar[1].variableSlider.value;
        Variables.AI_Tick_Max_Variance = defaultVar[2].variableSlider.value;
        Variables.AI_Tick_Min_Variance = defaultVar[3].variableSlider.value;
        Variables.healthModifyer = defaultVar[4].variableSlider.value;
        Variables.electronRechardModifyer = defaultVar[5].variableSlider.value;

        source.PlayOneShot(clip,Variables.sfxVolume);
    }

    public void ResetToDefault(){
        Variables.enemyCount = defaultVar[0].value;
        Variables.playerCount = defaultVar[1].value;
        Variables.AI_Tick_Max_Variance = defaultVar[2].value;
        Variables.AI_Tick_Min_Variance = defaultVar[3].value;
        Variables.healthModifyer = defaultVar[4].value;
        Variables.electronRechardModifyer = defaultVar[5].value;
    }
}
