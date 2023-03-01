using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
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
    
    [Header("Valeurs")]
    public float pickUpSpeed;
    public Transform pickUpPosition; 
    
    [Header("Data")]
    private float velocity;
    public GameObject pickedObject;
    public List<GameObject> objectsInRange;
  
  
    private void Awake()
    {
        inputAction.Player.PickUp.performed += ctx => Prendre();
        inputAction.Player.PickUp.canceled += ctx => Lacher();
    }
    
    private void Prendre()
    {
        pickedObject = GetClosestObject();
        if (pickedObject != null)
        {
            Debug.Log("je prend ce qui est devant moi");
            pickedObject.transform.DOMove(pickUpPosition.transform.position, pickUpSpeed);
            pickedObject.transform.parent = pickUpPosition.transform;
            pickedObject.GetComponent<WindSpirit>().canFall = false;
        }
    }
    
    private void Lacher()
    {
        if (pickedObject != null)
        {
            Debug.Log("je relache l'objet devant moi");
            pickedObject.GetComponent<WindSpirit>().canFall = true;
            pickedObject.transform.parent = null;
            pickedObject = null;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickableObject"))
        {
            objectsInRange.Add(other.gameObject);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PickableObject"))
        {
            objectsInRange.Remove(other.gameObject);
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
            }
        }
        return closestObject;
    }
}
