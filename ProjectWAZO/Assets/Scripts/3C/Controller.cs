using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Utilitaire;
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
    public float slopeSpeed;
    public float jumpForce;
    public float onMoveJumpForce = 7f;
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
    public bool onHeightChangingPlatform;

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
    public Animator anim;
    public static Controller instance;
    public TrailRenderer trail;
    public TrailRenderer trail2;
    

    private void Awake()
    {
        if (instance != default && instance!=this)
        {
            DestroyImmediate(this);
        }
        instance = this;
        
        rb = GetComponent<Rigidbody>();

        inputAction = new PlayerControls();
        inputAction.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector3>();
        inputAction.Player.Jump.performed += ctx => Sauter();
        inputAction.Player.Jump.performed += ctx => NarrationMenuManager.instance.CloseMenu();
        inputAction.Player.Jump.performed += ctx => MapManager.instance.ReturnMap();
        inputAction.Player.Jump.performed += ctx => isPressing = true;
        inputAction.Player.Jump.canceled += ctx => isPressing = false;
        inputAction.Player.MenuCarte.performed += ctx => MapManager.instance.OpenCloseMap();
        inputAction.Player.MenuPause.performed += ctx => PauseMenu.instance.PauseUnPause();
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
        
        if (moveInput.x != 0 && moveInput.z != 0) // Limite la vitesse lors des déplacements en diagonale
        {
            Vector2 groundMovement = Vector2.ClampMagnitude(new Vector2(rb.velocity.x, rb.velocity.z), 7.8f);
            rb.velocity = new Vector3(groundMovement.x, rb.velocity.y, groundMovement.y);
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SceneManager.LoadScene("Ethan");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SceneManager.LoadScene("Temple Test");
        }
        if (Input.GetKeyDown(KeyCode.Keypad3)) 
        {
            SceneManager.LoadScene("Dev_Paulin"); 
        }
                
        if (CinématiqueManager.instance.isCinématique)
        {
            anim.SetBool("isIdle",true);
            anim.SetBool("isFlying",false);
            anim.SetBool("isWalking",false);
            anim.speed = 1;
        }
        if (moveInput != Vector3.zero && !isEchelle && canMove) //rotations
        {
            Quaternion newRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation,rotationSpeed*Time.deltaTime);
        }
        else if(!isEchelle)
        {
            rb.useGravity = true;
            canPlaner = true;
        }

        if (isEchelle) // Si le perso est sur une echelle
        {
            anim.SetBool("isClimbing",true);
            anim.SetBool("isFlying",false);
            anim.SetBool("isWalking",false);
            anim.SetBool("isIdle",false);
            canPlaner = false;
            rb.useGravity = false;
            Debug.Log(anim.speed);
            Debug.Log(rb.velocity.magnitude);
            if (rb.velocity.magnitude == 0)
            {
                anim.enabled = false;
                //anim.speed = 0;
            }
            else
            {
                anim.enabled = true;
                //anim.speed = 1;
            }
            
            Debug.DrawRay(transform.position, Vector3.down*0.5f, Color.yellow,2);
            if (Physics.Raycast(transform.position, Vector3.down, 0.2f, groundMask))
            {
                anim.SetBool("isClimbing",false);
                isEchelle = false;
                PickUpObjects.instance.QuitEchelle();
            }
        }

        if (Physics.Raycast(transform.position, Vector3.down, 0.2f, groundMask) && !isEchelle)  //si le personnage est au sol
        {
            trail.emitting = false;
            trail2.emitting = false;
            anim.SetBool("isFlying",false);
            anim.speed = 1;
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
            //moveInput = moveInput.normalized;
            if (!isEchelle && canMove)
            {
                FixSpeedOnSlope();
                if (moveInput != Vector3.zero) // Modifie la vitesse de l'animation selon la vitesse du perso
                {
                    anim.SetBool("isWalking",true);
                    anim.SetBool("isIdle",false);
                    float animationSpeed = moveInput.magnitude;
                    animationSpeed = Mathf.Clamp(animationSpeed,0f, 1f);
                    anim.speed = animationSpeed;
                }
                else
                {
                    anim.speed = 1;
                    anim.SetBool("isIdle",true);
                    anim.SetBool("isWalking",false);
                }
            }
        }
        else // Si le personnage n'est pas au sol
        {
            anim.speed = 1;
            anim.SetBool("isWalking",false);
            anim.SetBool("isIdle",false);
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
                rb.velocity +=(new Vector3((float)moveInput.x,moveInput.y,moveInput.z) * (airControlSpeed * Time.deltaTime));
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
            rb.AddForce(new Vector3(0,onHeightChangingPlatform?onMoveJumpForce:jumpForce,0),ForceMode.VelocityChange);
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
                anim.SetBool("isFlying",true);
                anim.SetBool("isIdle",false);
                anim.SetBool("isWalking",false);
                anim.speed = 1;
            }
            else
            {
                anim.speed = 1;
                anim.SetBool("isFlying",false);
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
        var detector = pickedObject.GetComponent<Spirit>()?.ResetWeightOnDetector();
        if(detector!=_currentDetector) ResetWeightOnDetector();
    }

    //BUG fix
    [HideInInspector] public bool onMovingPlank;
    private void FixSpeedOnSlope() // Gérer les déplacements sur les pentes
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit,2,groundMask);
        Debug.DrawRay(hit.point,hit.normal,Color.green);
            
        if (Vector3.Dot(hit.normal, Vector3.up) < 0.97)
        {
            if(canJump && !onMovingPlank) rb.constraints = moveInput.magnitude == 0
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


