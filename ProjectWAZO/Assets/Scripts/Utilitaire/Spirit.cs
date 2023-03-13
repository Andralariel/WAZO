using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    public bool isTaken;
    public bool isClosest;
    private Rigidbody rb;
    public Material selectedMaterial;
    public Material unselectedMaterial;
    private MeshRenderer meshRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

   
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
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
