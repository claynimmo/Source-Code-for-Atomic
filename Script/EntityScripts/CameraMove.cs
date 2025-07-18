using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
 
public class CameraMove : MonoBehaviour
{
 
    public float YMin = -80f;
    private const float YMax = 80.0f;
 
    public Transform lookAt;
 
    public Transform Player;
 
    public float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float xsensitivity = 4.0f;
    public float ysensitivity= 4.0f;

    public bool invertx;
    public bool inverty;

    public bool camMoveAble=true;
    public float[] distances;


    Vector3 relativePos;
    public Transform target;
    public float distanceOffset;
    public float zoomSpeed = 2f;
    public float xSpeed = 300f;
    public float ySpeed = 300f;
    public float yMinLimit = 50f;
    public float yMaxLimit = 180f;
    public LayerMask raycastMask; 

    public float verticalOffset = 0.3f;
    public float bonusDistance;

    void Start(){
        UpdateInversion();
        Time.timeScale = 1;
        CursorController.HideCursor();
    }
    void Awake(){
        xsensitivity = Variables.xsensitivity;
        ysensitivity = Variables.ysensitivity;
    }


    private float refrerenc = 0.0f;

    void Update(){
        if(Variables.hardStop){return;}

        relativePos = transform.position - (target.position);
        RaycastHit hit;
        if(Physics.Raycast(target.position, relativePos, out hit, distance + 0.5f,raycastMask)){
            float offset = distance - hit.distance;
            offset = Mathf.Clamp(offset, 0, distance-2f);
            distanceOffset = Mathf.SmoothDamp(distanceOffset,offset,ref refrerenc,0.1f);
        }
        else{
            distanceOffset = 0;
        }
    }
    // Update is called once per frame
    void LateUpdate(){
        if(Variables.hardStop || !camMoveAble){return;}

        currentX += Input.GetAxis("Mouse X") * xsensitivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * ysensitivity * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, YMin, YMax);
        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        //transform.position = lookAt.position + rotation * Direction;

        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance + distanceOffset) + lookAt.position;
        transform.position = position;

        Vector3 Look = new Vector3(lookAt.position.x,lookAt.position.y+verticalOffset,lookAt.position.z);

        transform.LookAt(Look);
    }

    private void UpdateInversion(){
        if(invertx){
            xsensitivity = -xsensitivity;
        }
        if(inverty){
            ysensitivity = -ysensitivity;
        }
    }
}