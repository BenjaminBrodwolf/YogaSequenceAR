using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHint : MonoBehaviour
{
    
    Camera cameraToLookAt;
 
    // Use this for initialization 
    void Start()
    {
        cameraToLookAt = Camera.main;
         
    }
 
    // Update is called once per frame 
    void LateUpdate()
    {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }
    // Quaternion rotation;
    //
    // void Awake()
    // {
    //     rotation = transform.rotation;
    //     Debug.Log(rotation);
    // }
    // void LateUpdate()
    // {
    //     FaceTextMeshToCamera();
    //     transform.rotation = rotation;
    //
    // }
    // private Camera arCamera;
    //
    // private void Start()
    // {
    //     
    // }
    //
    // void FaceTextMeshToCamera(){
    //     Vector3 origRot = gameObject.transform.eulerAngles;
    //     gameObject.transform.LookAt(Camera.main.transform);
    //     Vector3 desiredRot = gameObject.transform.eulerAngles;
    //     origRot.y = desiredRot.y;
    //     gameObject.transform.eulerAngles = origRot;
    // }
}
