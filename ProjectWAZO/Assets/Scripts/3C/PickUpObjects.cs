using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
    public List<GameObject> objectsInRange;
    public int pickedObjectMass;
    public bool isEchelle;
    public GameObject currentEchelle;

    public static PickUpObjects instance;

 

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
        if (isThingTaken == false && !isEchelle)
        {
            pickedObject = GetClosestObject();
            if (pickedObject != null)
            {
                Debug.Log("je prend ce qui est devant moi");
                isThingTaken = true;
                pickedObjectMass = Mathf.RoundToInt(pickedObject.GetComponent<Rigidbody>().mass);
                transform.parent.gameObject.GetComponent<Rigidbody>().mass += pickedObjectMass;
                pickedObject.GetComponent<Rigidbody>().mass -= pickedObjectMass;
                var tween = pickedObject.transform.DOMove(pickUpPosition.transform.position, pickUpSpeed);
                tween.OnComplete(ChangeParent);
                pickedObject.GetComponent<Spirit>().isTaken = true;
                Controller.instance.ResetWeightOnDetector();
            }
        }

        if (isEchelle)
        {
            isThingTaken = false;
            Controller.instance.isEchelle = true;
            Controller.instance.GetComponent<Rigidbody>().useGravity = false;
            Controller.instance.transform.LookAt(currentEchelle.transform);
            Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x, Controller.instance.transform.position.y , currentEchelle.transform.position.z),0.5f);
        }
      
    }

    private void ChangeParent()
    {
        pickedObject.transform.parent = pickUpPosition.transform;
    }
    
    private void Lacher()
    {
        if (pickedObject != null && isThingTaken && !isEchelle)
        {
            Debug.Log("je relache l'objet devant moi");
            isThingTaken = false;
            transform.parent.gameObject.GetComponent<Rigidbody>().mass -= pickedObjectMass;
            pickedObject.GetComponent<Rigidbody>().mass += pickedObjectMass;
            pickedObject.GetComponent<Spirit>().isTaken = false;
            pickedObject.transform.parent = null;
            pickedObject = null;
            pickedObjectMass = 0;
            
            Controller.instance.ResetWeightOnDetector();
        }

        if (isEchelle)
        {
            isThingTaken = false;
            Controller.instance.isEchelle = false;
            Controller.instance.GetComponent<Rigidbody>().useGravity = true;
        }
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
            isEchelle = false;
            Controller.instance.isEchelle = false;
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
