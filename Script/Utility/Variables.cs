using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables : MonoBehaviour
{
    //class of static variables that effect every object in the scene
    //variables are not in arrays for better readability, at the expense for less maintainble code
    public static bool inputAble = true;
    public static bool hardStop = false;

    public static float enemyCount = 20;
    public static float playerCount = 0;

    public static float AI_Tick_Max_Variance = 7;

    public static float AI_Tick_Min_Variance = 3;

    public static float shootTickModifyer = 0.56f;
    public static float swapModeTickModifyer = 5;
    public static float useAbilityTickModifyer = 0.68f;
    public static bool randomiseTickModifyers;

    public static float healthModifyer = 1;

    public static float electronRechardModifyer = 1;

    public static float musicVolume = 0.5f;
    public static float sfxVolume = 0.5f;
    public static bool hideUI;
    public static bool hideAllUI;
    public static float xsensitivity = 1000;
    public static float ysensitivity = 1000;
}
