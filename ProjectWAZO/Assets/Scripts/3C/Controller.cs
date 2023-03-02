using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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

    [Header("Autre")] 
    public static Controller instance;
    public TrailRenderer trail;
    private MeshRenderer meshRenderer;
    public Material planingMaterial;
    public Material nonPlaningMaterial;
    
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
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
    
    void Update()
    {
        if (moveInput != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation,rotationSpeed*Time.deltaTime);
        }
        
        if (Physics.Raycast(transform.position, Vector3.down, 1.2f, groundMask))  //si le personnage est au sol
        {
            trail.emitting = false;
            meshRenderer.material = nonPlaningMaterial;
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
            rb.velocity +=(new Vector3(moveInput.x,moveInput.y,moveInput.z) * (moveSpeed * Time.deltaTime));
        }
        else  //si le personnage n'est pas au sol
        {
            Planer();
            DoOnce = true;
            isGrounded = false;
            moveInput = moveInput.normalized;
            rb.velocity +=(new Vector3(moveInput.x,moveInput.y,moveInput.z) * (airControlSpeed * Time.deltaTime));
            gravityScale -= 5f * Time.deltaTime;
        }
        
        
    }
    
    
    private void Sauter()
    {
        
        if (isGrounded)
        {
            Debug.DrawRay(transform.position, Vector3.down*1.2f, Color.green,2);
            rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
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
                meshRenderer.material = planingMaterial;
                globalGravity = planingGravity;
                Debug.Log("je crois que je peux voler");
            }
            else
            {
                trail.emitting = false;
                meshRenderer.material = nonPlaningMaterial;
                globalGravity = 9.81f;
            }
          
        }

    }

    public void Dash()
    {
        rb.AddForce(moveInput*DASHSPEED,ForceMode.Impulse);
    }
}


