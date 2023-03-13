using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using WeightSystem;
using WeightSystem.Detector;

public class Controller : MonoBehaviour
{
    
    #region Trucs Chelou pour les Input
    private PlayerControls inputAction;
    private void OnEnable()
    {
       
        inputAction.Player.Enable();
    }

    private void OnDisable()
    {
      
        inputAction.Player.Disable();
    }

    #endregion
   
    [Header("Métrics Controller")]
    public float moveSpeed;
    public float airControlSpeed;
    public float jumpForce;
    public float gravityScale;
    public float planingGravity;
    public float DASHSPEED;

    [Header("Tracker Controller")] 
    public bool isGrounded;
    public bool canPlaner;
    public bool isPressing;

    [Header("Utilitaire")] 
    public float rotationSpeed;
    public float globalGravity;
    public Vector3 moveInput;
    public LayerMask groundMask;
    private Rigidbody rb;
    private bool DoOnce = true;
    public bool isEchelle;

    [Header("Autre")] 
    public GameObject flyIndicator;
    public static Controller instance;
    public TrailRenderer trail;
    public TrailRenderer trail2;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        if (instance != default && instance!=this)
        {
            DestroyImmediate(this);
        }
        instance = this;
        
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();

        inputAction = new PlayerControls();
        inputAction.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector3>();
        inputAction.Player.Jump.performed += ctx => Sauter();
        inputAction.Player.Jump.performed += ctx => isPressing = true;
        inputAction.Player.Jump.canceled += ctx => isPressing = false;
        inputAction.Player.Dash.performed += ctx => Dash();
    }

     void FixedUpdate ()
     {
         if (!isEchelle)
         {
             Vector3 gravity = globalGravity * gravityScale * Vector3.up;
             rb.AddForce(rb.mass*gravity, ForceMode.Force);
         }
     }
    
    void Update()
    {
        if (moveInput != Vector3.zero && !isEchelle)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation,rotationSpeed*Time.deltaTime);
        }
        
        if (Physics.Raycast(transform.position, Vector3.down, 1.2f, groundMask))  //si le personnage est au sol
        {
            trail.emitting = false;
            trail2.emitting = false;
            if (DoOnce)
            {
                gravityScale = -4;
                globalGravity = 9.81f;
                isGrounded = true;
                canPlaner = true;
                //StopPlaner();
                DoOnce = false;
            }
            moveInput = moveInput.normalized;
            if (!isEchelle)
            {
                rb.velocity += (new Vector3(moveInput.x, moveInput.y, moveInput.z) * (moveSpeed * Time.deltaTime));
            }
        }
        else  //si le personnage n'est pas au sol
        {
            Planer();
            DoOnce = true;
            isGrounded = false;
            moveInput = moveInput.normalized;
            if (!isEchelle)
            {
                rb.velocity +=(new Vector3(moveInput.x,moveInput.y,moveInput.z) * (airControlSpeed * Time.deltaTime));
                gravityScale -= 5f * Time.deltaTime;
            }
        }

        if(isEchelle)
        {
            rb.velocity +=(new Vector3(0,-moveInput.x,0) * (airControlSpeed * Time.deltaTime));
        }
        
        RaycastHit hit;   // L'indication de la trajectoire de chute pendant le planage
        if (!isGrounded && isPressing)
        {
            if(Physics.Raycast(transform.position, Vector3.down, out hit,Mathf.Infinity, groundMask))
            {
                var transformLocalScale = new Vector3(0.4f*hit.distance, 0.1f,0.4f*hit.distance);
                transformLocalScale.x = Mathf.Clamp( transformLocalScale.x, 0.5f,3);
                transformLocalScale.z = Mathf.Clamp(transformLocalScale.z, 0.5f,3);
                flyIndicator.transform.localScale = transformLocalScale;
                flyIndicator.SetActive(true);
                flyIndicator.transform.position = hit.point;
            }
            else
            {
                flyIndicator.SetActive(false);
            }
        }
        else
        {
            flyIndicator.SetActive(false);
        }
    }
    
    
    private void Sauter()
    {
        
        if (isGrounded)
        {
            Debug.DrawRay(transform.position, Vector3.down*1.2f, Color.green,2);
            rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.VelocityChange);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down*1.2f, Color.red,2);
        }
    }
    
    private void Planer()
    {
        if (canPlaner)
        {
            if (isPressing)
            {
                trail.emitting = true;
                trail2.emitting = true;
                globalGravity = planingGravity;
            }
            else
            {
                trail.emitting = false;
                trail2.emitting = false;
                globalGravity = 9.81f;
            }
          
        }

    }

    public void Dash()
    {
        rb.AddForce(moveInput*DASHSPEED,ForceMode.Impulse);
    }
    
    //WeightSystem
    private WeightDetector _currentDetector;

    public void SetDetector(WeightDetector detector)
    {
        _currentDetector = detector;
    }

    public void ResetWeightOnDetector()
    {
        if (_currentDetector == default) return;
        _currentDetector.ResetWeight();
    }
}


