using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Rigidbody rb;

    private PlayerControls inputAction;
   
    private void OnEnable()
    {
       
        inputAction.Player.Enable();
    }

    private void OnDisable()
    {
      
       inputAction.Player.Disable();
    }
    
    public Vector3 moveInput;
    public float moveSpeed;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputAction = new PlayerControls();
        inputAction.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector3>();
        inputAction.Player.Jump.performed += ctx => Sauter();
        inputAction.Player.Jump.started += ctx => Planer();
        inputAction.Player.PickUp.performed += ctx => Prendre();
    }

    
    void Update()
    {
    
        moveInput = moveInput.normalized;
        rb.velocity +=(new Vector3(moveInput.x,moveInput.y,moveInput.z) * (moveSpeed * Time.deltaTime));
    }
    
    
    private void Sauter()
    {
        rb.AddForce(new Vector3(0,999,0));
    }
    
    private void Planer()
    {
       Debug.Log("je crois que je peux voler");
    }
    
    private void Prendre()
    {
        Debug.Log("objet en bec");
    }
}


