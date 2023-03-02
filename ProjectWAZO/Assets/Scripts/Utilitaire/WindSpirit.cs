using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpirit : MonoBehaviour
{
    public bool isTaken;
    public bool isClosest;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    public Material selectedMaterial;
    public Material unselectedMaterial;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

   
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PickUpTrigger"))
        {
            isClosest = false;
        }    
    }

    private void Update()
    {
        if (isClosest)
        {
            meshRenderer.material = selectedMaterial;
        }
        else
        {
            meshRenderer.material = unselectedMaterial;
        }
        
        if (isTaken)
        {
            rb.isKinematic = true;
            meshRenderer.material = unselectedMaterial;
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}
