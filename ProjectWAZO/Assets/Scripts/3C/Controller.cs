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
    public float slopeSpeed = 10;
    public float jumpForce;
    public float gravityScale;
    public float planingGravity;
    public float coyoteTime;

    [Header("Tracker Controller")] 
    public bool isGrounded;
    public bool canPlaner;
    public bool canJump;
    public bool canMove;
    public bool isPressing;
    public bool isWind;
    public bool isCoyote;

    [Header("Utilitaire")] 
    public float rotationSpeed;
    public float globalGravity;
    public Vector3 moveInput;
    public LayerMask groundMask;
    [HideInInspector] public Rigidbody rb;
    public bool DoOnce = true;
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
    }

     void FixedUpdate () // Gravité
     {
         if (!isEchelle)
         {
             Vector3 gravity = globalGravity * gravityScale * Vector3.up;
             rb.AddForce(rb.mass*gravity, ForceMode.Force);
         }
     }
    
    void Update()
    {
        if (moveInput != Vector3.zero && !isEchelle && canMove) // mouvements et rotations
        {
            Quaternion newRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation,rotationSpeed*Time.deltaTime);
        }
        else if(!isEchelle)
        {
            rb.useGravity = true;
            canPlaner = true;
        }

        if (isEchelle)
        {
            canPlaner = false;
            rb.useGravity = false;
            Debug.DrawRay(transform.position, Vector3.down*0.5f, Color.yellow,2);
            if (Physics.Raycast(transform.position, Vector3.down, 0.5f, groundMask))
            {
                isEchelle = false;
                PickUpObjects.instance.QuitEchelle();
            }
        }
        
        if (Physics.Raycast(transform.position, Vector3.down, 0.2f, groundMask) && !isEchelle)  //si le personnage est au sol
        {
            trail.emitting = false;
            trail2.emitting = false;
            StopAllCoroutines();
            
            isCoyote = false;
            if (DoOnce)
            {
                canJump = true;
                gravityScale = -4;
                globalGravity = 9.81f;
                isGrounded = true;
                canPlaner = true;
                //StopPlaner();
                DoOnce = false;
            }
            moveInput = moveInput.normalized;
            if (!isEchelle && canMove)
            {
                FixSpeedOnSlope();
            }
        }
        else if (!Physics.Raycast(transform.position, Vector3.down, 0.2f, groundMask)) //si le personnage n'est pas au sol
        {
            //StartCoroutine(coyote);
            Planer();
            DoOnce = true;
            isGrounded = false;
            moveInput = moveInput.normalized;
            if (!isEchelle && !isWind)
            {
                gravityScale -= 5f * Time.deltaTime;
            }
            
            if (!isEchelle) 
            { 
                rb.velocity +=(new Vector3(moveInput.x,moveInput.y,moveInput.z) * (airControlSpeed * Time.deltaTime));
            }
        }

        
        
        RaycastHit hit;   // L'indication de la trajectoire de chute pendant le planage
        if (!isGrounded && isPressing && !isEchelle)
        {
            if(Physics.Raycast(transform.position, Vector3.down, out hit,Mathf.Infinity, groundMask))
            {
                var transformLocalScale = new Vector3(0.4f*hit.distance, 0.1f,0.4f*hit.distance);
                transformLocalScale.x = Mathf.Clamp( transformLocalScale.x, 0.5f,3);
                transformLocalScale.z = Mathf.Clamp(transformLocalScale.z, 0.5f,3);
                flyIndicator.transform.localScale = transformLocalScale;
                flyIndicator.transform.rotation =
                    Quaternion.LookRotation(Vector3.Cross(Vector3.left,
                        hit.normal));
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
        if (canJump)
        {
            if (isEchelle)
            {
                PickUpObjects.instance.QuitEchelle();
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            canJump = false;
            isCoyote = false;
            StopAllCoroutines();
            Debug.DrawRay(transform.position, Vector3.down*0.2f, Color.green,2);
            rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.VelocityChange);
        }
        else if (!canJump)
        {
            Debug.DrawRay(transform.position, Vector3.down*0.2f, Color.red,2);
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

    public void ResetWeightOnDetector(Transform pickedObject)
    {
        pickedObject.GetComponent<Spirit>()?.ResetWeightOnDetector();
    }

    private void FixSpeedOnSlope() // Gérer les déplacements sur les pentes
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit,2,groundMask);
        Debug.DrawRay(hit.point,hit.normal,Color.green);
            
        if (Vector3.Dot(hit.normal, Vector3.up) < 1)
        {
            if(canJump)rb.constraints = moveInput.magnitude == 0
                ? RigidbodyConstraints.FreezeRotation|RigidbodyConstraints.FreezePositionY
                : RigidbodyConstraints.FreezeRotation;
            
            var direction = Vector3.Cross(hit.normal, moveInput);
            Debug.DrawRay(hit.point,direction.normalized,Color.blue);
            direction = Vector3.Cross(direction,hit.normal);
            Debug.DrawRay(hit.point,direction.normalized,Color.red);

            rb.velocity = direction.normalized * slopeSpeed;
        }
        else
        {
            rb.velocity += moveInput * (moveSpeed * Time.deltaTime);
        }
    }
}


