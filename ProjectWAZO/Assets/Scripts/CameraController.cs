using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouvement")]
    public Vector3 offset;
   
    public float SmoothMoveFactor;
    public Quaternion normalRotation;
    
    [Header("Utilitaire")] 
    public GameObject player;
    private Vector3 velocity;
    private Camera camera;
    
    [Header("Experimental")]
    public float minZoom;
    public float maxZoom;
    private float zoomController = 15;
    
    public bool isTopDown;
    public float SmoothRotateFactor;
    public Vector3 topDownOffset;
    public Quaternion topDownRotation;
   

    private void Start()
    {
        transform.position = player.transform.position + offset;
        camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Zoom(int zoomAmount)  // zoomAmount est entre 0 et 1
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom,zoomAmount);
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, newZoom, Time.deltaTime);
    }
    
    void Move()
    {
        if (!isTopDown)
        {
            Vector3 newPosition = player.transform.position + offset;
            transform.localPosition = Vector3.SmoothDamp(transform.position,newPosition,ref velocity,SmoothMoveFactor);
            transform.rotation = Quaternion.Slerp(transform.rotation, normalRotation, Time.deltaTime/SmoothRotateFactor);
        }
        else
        {
            Vector3 newPosition = player.transform.position + topDownOffset;
            transform.localPosition = Vector3.SmoothDamp(transform.position,newPosition,ref velocity,SmoothMoveFactor);
            transform.rotation = Quaternion.Slerp(transform.rotation, topDownRotation,Time.deltaTime/ SmoothRotateFactor);
        }

    }
}
