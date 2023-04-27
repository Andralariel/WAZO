using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utilitaire;

public class PickUpObjects : MonoBehaviour
{
    
    #region Trucs Chelou pour les Input
    public PlayerControls inputAction;
    private void OnEnable()
    {
       
        inputAction.Player.Enable();
    }

    private void OnDisable()
    {
      
        inputAction.Player.Disable();
    }

    #endregion

    [Header("Valeurs")] 
    public bool isThingTaken;
    public float pickUpSpeed;
    public Transform pickUpPosition; 
    
    [Header("Data")]
    private float velocity;
    public GameObject pickedObject;
    public Animator anim;
    public List<GameObject> objectsInRange;
    public int pickedObjectMass;
    public bool isEchelle;
    public GameObject currentEchelle;

    public static PickUpObjects instance;

    //CleanerRbReference
    private Rigidbody _rbObject;
    
    //BUG fix : pickUp button state
    private bool _beakPinch;

    private void Awake()
    {
        if (instance != default && instance!=this)
        {
            DestroyImmediate(this);
        }
        instance = this;
        
        inputAction = new PlayerControls();
        inputAction.Player.PickUp.performed += ctx => Prendre();
        inputAction.Player.PickUp.canceled += ctx => Lacher();
    }

    public void Prendre()
    {
        _beakPinch = true;
        if (!isThingTaken&& !isEchelle)
        {
            pickedObject = GetClosestObject();
            if (pickedObject != null)
            {
                StartCoroutine(Animation());
                Debug.Log("je prend ce qui est devant moi");
                _rbObject = pickedObject.GetComponent<Rigidbody>();
                _rbObject.angularDrag = -1;
                pickedObjectMass = Mathf.RoundToInt(_rbObject.mass);
                transform.parent.gameObject.GetComponent<Rigidbody>().mass += pickedObjectMass;
                _rbObject.mass -= pickedObjectMass;
                pickedObject.transform.parent = pickUpPosition.transform;
                MoveToBeak();
                pickedObject.GetComponent<Spirit>().isTaken = true;
                Controller.instance.ResetWeightOnDetector(pickedObject.transform);
            }
        }

        if (isEchelle && !isThingTaken)
        {
            EnterEchelle();
        }
    }

    IEnumerator Animation()
    {
        anim.SetBool("isTaking",true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isTaking",false);
    }
    
    private void MoveToBeak()
    {
        pickedObject.transform.DOLocalMove(Vector3.zero, pickUpSpeed).OnComplete(CheckBeak);
    }

    private void CheckBeak()
    {
        isThingTaken = true;
        if (!_beakPinch) Lacher();
        anim.SetBool("isTaking",false);
    }

    private void Lacher()
    {
        anim.SetBool("isTaking",false);
        _beakPinch = false;
        if (pickedObject != null && isThingTaken && !_moveOnLadder)
        {
            Debug.Log("je relache l'objet devant moi");
            isThingTaken = false;
            transform.parent.gameObject.GetComponent<Rigidbody>().mass -= pickedObjectMass;
            _rbObject.mass += pickedObjectMass;
            _rbObject.angularDrag = 0;
            pickedObject.GetComponent<Spirit>().isTaken = false;
            pickedObject.transform.parent = null;
            pickedObject = null;
            pickedObjectMass = 0;

            Controller.instance.ResetWeightOnDetector();
        }

        if (_moveOnLadder)
        {
            QuitEchelle();
        }
    }

    // BUG fix
    private bool _moveOnLadder;
    
    public void EnterEchelle()
    {
        _moveOnLadder = true;
        anim.SetTrigger("startClimb");
        Controller.instance.gravityScale = -4;
        Controller.instance.canJump = true;
        Controller.instance.canPlaner = false;
        Controller.instance.canMove = false;
        Controller.instance.GetComponent<Rigidbody>().useGravity = false;
        switch (currentEchelle.GetComponent<echelleData>().orientation)
        {
            case echelleData.Orientation.nord:
                Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x, Controller.instance.transform.position.y +0.7f, currentEchelle.transform.position.z-0.4f),0.5f).OnComplete(SetIsEchelle);
                break;
            case echelleData.Orientation.sud:
                Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x, Controller.instance.transform.position.y +0.7f, currentEchelle.transform.position.z+0.4f),0.5f).OnComplete(SetIsEchelle);
                break;
            case echelleData.Orientation.est:
                Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x-0.4f, Controller.instance.transform.position.y +0.7f, currentEchelle.transform.position.z),0.5f).OnComplete(SetIsEchelle);
                break;
            case echelleData.Orientation.ouest:
                Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x+0.4f, Controller.instance.transform.position.y +0.7f, currentEchelle.transform.position.z),0.5f).OnComplete(SetIsEchelle);
                break;
        }
    }
    private void SetIsEchelle()
    {
        Controller.instance.isEchelle = true;
    }
    
    public void QuitEchelle()
    {
        Debug.Log("quit");
        anim.SetBool("isClimbing",false);
        anim.ResetTrigger("startClimb");
        _moveOnLadder = false;
        Controller.instance.canPlaner = true;
        Controller.instance.canMove = true;
        Controller.instance.rb.useGravity = true;
        Controller.instance.isEchelle = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            objectsInRange.Add(other.gameObject);
            GetClosestObject();
        }
        
        if (other.gameObject.layer == 9) // Si l'objet est une echelle
        {
            isEchelle = true;
            currentEchelle = other.gameObject;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            objectsInRange.Remove(other.gameObject);
            GetClosestObject();
        }
        
        if (other.gameObject.layer == 9) // Si l'objet est une echelle
        {
            QuitEchelle();
            isEchelle = false;
            currentEchelle = null;
        }
    }

    GameObject GetClosestObject()
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        if (objectsInRange != null)
        {
            for (int i = 0; i < objectsInRange.Count; i++)
            {
                float newDistance = Vector3.Distance(transform.position, objectsInRange[i].transform.position);
                if (newDistance < closestDistance)
                {
                    closestDistance = newDistance;
                    closestObject = objectsInRange[i];
                }
                else
                {
                    objectsInRange[i].GetComponent<Spirit>().isClosest = false;
                }
            }
            
            if (objectsInRange.Count == 1)
            {
                closestObject = objectsInRange[0];
                closestObject.GetComponent<Spirit>().isClosest = true;
            }
        }
      

        if (closestObject != null)
        {
            closestObject.GetComponent<Spirit>().isClosest = true;
        }
        return closestObject;
    }
}
