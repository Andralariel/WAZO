using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;
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
    [SerializeField] private int stuckBuffer = 10;

    [Header("Tracker Controller")] 
    public bool ultraBlock;
    public bool isGrounded;
    public bool canPlaner;
    public bool canJump;
    public bool canMove;
    public bool isPressing;
    public bool isWind;
    private bool isCoyote;
    public bool StopGravity;
    public bool MoveEnding;
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
        if (canMove)
        {
            inputAction.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector3>();
        }
        inputAction.Player.Jump.performed += ctx => Sauter();
        inputAction.Player.Jump.performed += ctx => NarrationMenuManager.instance.CloseMenu();
        inputAction.Player.Jump.performed += ctx => isPressing = true;
        inputAction.Player.Jump.canceled += ctx => isPressing = false;
        inputAction.Player.MenuCarte.performed += ctx => CarnetManager.instance.OpenCloseCarnet();
        inputAction.Player.MenuPause.performed += ctx => PauseMenu.instance.PauseUnPause();
    }

     void FixedUpdate () // Gravité
     {
         if (!isEchelle && !StopGravity)
         {
             Vector3 gravity = globalGravity * gravityScale * Vector3.up;
             rb.AddForce(rb.mass*gravity, ForceMode.Force);
         }
     }

     //DirectionAlignment
     private Vector3 _moveDir;
     private void AlignInputWithCameraAngle()
     {
         moveInput.Normalize();
         var cameraAngle = -CameraController.instance.normalRotation.y*Mathf.Deg2Rad;
         var newX = moveInput.x * Mathf.Cos(cameraAngle) - moveInput.z * Mathf.Sin(cameraAngle);
         var newZ = moveInput.x * Mathf.Sin(cameraAngle) + moveInput.z * Mathf.Cos(cameraAngle);
        // _moveDir = new Vector3(newX,0,newZ);
        _moveDir = new Vector3(moveInput.x,0,moveInput.z);  // J'AI ENLEVÉ LES DEPALCEMENTS DANS L'AXE DE LA CAMERA
     }
     
    void Update()
    {
        AlignInputWithCameraAngle();

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Dev_map");
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("Temple Test");
        }
        
        if (_moveDir.x != 0 && _moveDir.z != 0) // Limite la vitesse lors des déplacements en diagonale
        {
            Vector2 groundMovement = Vector2.ClampMagnitude(new Vector2(rb.velocity.x, rb.velocity.z), 7.8f);
            rb.velocity = new Vector3(groundMovement.x, rb.velocity.y, groundMovement.y);
        }

        if (CinématiqueManager.instance.isCinématique)
        {
            anim.SetBool("isIdle",false);
            anim.SetBool("isFlying",false);
            anim.SetBool("isWalking",false);
        }
        
        if (_moveDir != Vector3.zero && !isEchelle && canMove) //rotations
        {
            Quaternion newRotation = Quaternion.LookRotation(_moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation,rotationSpeed*Time.deltaTime);
        }
        else if(!isEchelle && !StopGravity)
        {
            rb.useGravity = true;
            canPlaner = true;
        }

        if (MoveEnding)
        {
           rb.velocity += (new Vector3(0f,0f,1f) * (airControlSpeed));
           Vector3 velocityClamp = new Vector3(Mathf.Clamp(rb.velocity.x, -3f,3f),0,Mathf.Clamp(rb.velocity.z, -5f,5f));
           rb.velocity = velocityClamp; 
           canMove = false;
        }

        if (isEchelle) // Si le perso est sur une echelle, son anim dépend de sa vitesse et il peut en sortir en touchant le sol
        {
            trail.emitting = false;
            trail2.emitting = false;
            anim.SetBool("isClimbing",true);
            anim.SetBool("isFlying",false);
            anim.SetBool("isWalking",false);
            anim.SetBool("isIdle",false);
            canPlaner = false;
            rb.useGravity = false;
            float animationSpeed = _moveDir.magnitude;
            animationSpeed = Mathf.Clamp(animationSpeed,0f, 1f);
            anim.SetFloat("ClimbingSpeed",animationSpeed);

            
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
            anim.SetBool("isIdle",true);
            if (CinématiqueManager.instance.isCinématique == false)
            {
                anim.speed = 1;
            }
            StopAllCoroutines();
            
            isCoyote = false;
            if (DoOnce)
            {
                if (!ultraBlock)
                {
                    canJump = true;
                    canPlaner = true;
                }
                gravityScale = -4;
                globalGravity = 9.81f;
                isGrounded = true;
                DoOnce = false;
                //StopPlaner();
            }
            if (!isEchelle && canMove)
            {
                FixSpeedOnSlope();
                if (_moveDir != Vector3.zero && CinématiqueManager.instance.isCinématique == false) // Modifie la vitesse de l'animation selon la vitesse du perso
                {
                    anim.SetBool("isWalking",true);
                    anim.SetBool("isFlying",false);
                    anim.SetBool("isIdle",false);
                    float animationSpeed = _moveDir.magnitude;
                    animationSpeed = Mathf.Clamp(animationSpeed,0f, 1f);
                    anim.speed = animationSpeed;
                }
                else
                {
                    if (CinématiqueManager.instance.isCinématique == false)
                    {
                        anim.speed = 1;  
                    }
                    anim.SetBool("isIdle",true);
                    anim.SetBool("isWalking",false);
                }
            }
        }
        else // Si le personnage n'est pas au sol
        {
            if (CinématiqueManager.instance.isCinématique == false)
            {
                anim.speed = 1;  
            }
            anim.SetBool("isWalking",false);
            anim.SetBool("isIdle",false);
            Planer();
            DoOnce = true;
            isGrounded = false;
            if (!isEchelle && !isWind)
            {
                gravityScale -= 5f * Time.deltaTime;
            }
            
            if (!isEchelle) 
            { 
                rb.velocity +=(new Vector3((float)_moveDir.x,0,_moveDir.z) * (airControlSpeed * Time.deltaTime));
            }

            CheckIfStuck();
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
                anim.SetBool("isFlying",true);
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            canJump = false;
            isCoyote = false;
            StopAllCoroutines();
            Debug.DrawRay(transform.position, Vector3.down*0.2f, Color.green,2);
            rb.AddForce(new Vector3(0,onHeightChangingPlatform?onMoveJumpForce:jumpForce,0),ForceMode.VelocityChange);
            _stuckFrameAmount = 0;
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
                if (CinématiqueManager.instance.isCinématique == false)
                {
                    anim.speed = 1;  
                }
            }
            else
            {
                if (CinématiqueManager.instance.isCinématique == false)
                {
                    anim.speed = 1;  
                }
                anim.SetBool("isFlying",false);
                trail.emitting = false;
                trail2.emitting = false;
                globalGravity = 9.81f;
            }
        }
    }

    public void ChangeAnimSpeed(float newSpeed)
    {
        anim.speed = newSpeed;
    }

    //Fix to prevent player from getting stuck between
    private Vector3 _lastPos;
    private const float PosRange = 0.05f;
    private int _stuckFrameAmount;
    private void CheckIfStuck()
    {
        if(isPressing) return;
        
        if ((transform.position - _lastPos).magnitude < PosRange) _stuckFrameAmount++;
        else _stuckFrameAmount = 0;

        if (_stuckFrameAmount >= stuckBuffer)
        {
            gravityScale = -4;
            canJump = true;
        }
        
        _lastPos = transform.position;
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
            if(canJump && !onMovingPlank) rb.constraints = _moveDir.magnitude == 0
                ? RigidbodyConstraints.FreezeRotation|RigidbodyConstraints.FreezePositionY
                : RigidbodyConstraints.FreezeRotation;
            
            var direction = Vector3.Cross(hit.normal, _moveDir);
            Debug.DrawRay(hit.point,direction.normalized,Color.blue);
            direction = Vector3.Cross(direction,hit.normal);
            Debug.DrawRay(hit.point,direction.normalized,Color.red);

            rb.velocity = direction.normalized * slopeSpeed;
        }
        else
        {
            rb.velocity += _moveDir * (moveSpeed * Time.deltaTime);
        }
    }
}


