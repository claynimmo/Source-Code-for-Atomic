using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    //script with static functions relating to the cursor
    public static void HideCursor(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void ShowCursor(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
