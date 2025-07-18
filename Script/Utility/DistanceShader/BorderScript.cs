using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderScript : MonoBehaviour
{
    public Material material;
    public Transform otherObject;

    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;

    void Start(){
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    void Update(){
        _propBlock.SetVector("_OtherObjectPosition", otherObject.position);
        _renderer.SetPropertyBlock(_propBlock);
    }
}