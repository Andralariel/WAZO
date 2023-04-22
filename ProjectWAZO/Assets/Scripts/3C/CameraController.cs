using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouvement")]
    public Vector3 offset;
   
    public float SmoothMoveFactor;
    public float walkingLookFactor;
    public Quaternion normalRotation;
    private Vector3 savePosition;

    [Header("Utilitaire")] 
    public bool canMove;
    public bool canRotate;
    public GameObject player;
    private Vector3 velocity;
    private Camera camera;
    
    [Header("Zoom")]
    public float minZoom;
    public float maxZoom;
    private float zoomFactor = 15;
    private float rotationFactor = 15;
    
    [Header("Focused")]
    public Transform focusedObject;
    public float SmoothRotateFactor;
    
    [Header("TopDown")]
    public Vector3 topDownOffset;
    public Quaternion topDownRotation;
    
    [Header("Lerp")]
    public GameObject target1;
    public GameObject target2;
    public Vector3 lerpGoal;
    
    [Header("Camera State")]
    public bool isIso;
    public bool isTopDown;
    public bool isFocused;
    public bool isVerticalLerp;

    public static CameraController instance;
    [HideInInspector] public bool StartToPlayer;


    private void Awake()
    {
        if (instance != default && instance!=this)
        {
            DestroyImmediate(this);
        }
        instance = this;
    }


    private void Start()
    {
        if (StartToPlayer)
        {
            transform.position = player.transform.position + offset;
        }
        camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
        else if(canRotate)
        {
            RotateStill();
        }
    }

    public void Zoom(float zoomAmount)  // zoomAmount est entre 0 et 1
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom,zoomAmount);
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, newZoom, Time.deltaTime);
    }

    void RotateStill()
    {
        var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationFactor * Time.deltaTime);

    }
    
    public void Move()
    {
        if (isIso)
        {
            if (Controller.instance.canMove)
            {
                Vector3 newPosition = player.transform.position + offset + (Controller.instance.moveInput * walkingLookFactor);
                transform.localPosition = Vector3.SmoothDamp(transform.position,newPosition,ref velocity,SmoothMoveFactor);
                transform.rotation = Quaternion.Slerp(transform.rotation, normalRotation, Time.deltaTime/SmoothRotateFactor);
            }
            else
            {
                Vector3 newPosition = player.transform.position + offset;
                transform.localPosition = Vector3.SmoothDamp(transform.position,newPosition,ref velocity,SmoothMoveFactor);
                transform.rotation = Quaternion.Slerp(transform.rotation, normalRotation, Time.deltaTime/SmoothRotateFactor);
            }
           
        }
        else if(isTopDown)
        {
            Vector3 newPosition = player.transform.position + topDownOffset;
            transform.localPosition = Vector3.SmoothDamp(transform.position,newPosition,ref velocity,SmoothMoveFactor);
            transform.rotation = Quaternion.Slerp(transform.rotation, topDownRotation,Time.deltaTime/ SmoothRotateFactor);
        }
        else if (isFocused)
        {
            Vector3 newPosition = GetCenterPoint() + offset;
            transform.localPosition = Vector3.SmoothDamp(transform.position,newPosition,ref velocity,SmoothMoveFactor);
            transform.rotation = Quaternion.Slerp(transform.rotation, normalRotation, Time.deltaTime/SmoothRotateFactor);
        }
        else if(isVerticalLerp)
        {
            float yValue = target2.transform.position.y - target1.transform.position.y;
            float relativePosition = (player.transform.position.y - target1.transform.position.y) / yValue;
            Vector3 objectif = player.transform.position + offset + lerpGoal;
            Debug.Log(relativePosition);
            var toGo = Vector3.Lerp(savePosition,objectif,relativePosition);
            transform.position = toGo;
        }

    }

    public void SavePosition()
    {
        savePosition = transform.position+new Vector3(0,0,1);
    }
    Vector3 GetCenterPoint()
    {
        var bounds = new Bounds(player.transform.position, Vector3.zero);
        bounds.Encapsulate(focusedObject.transform.position);
        return bounds.center;
    }
}
