using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using WeightSystem.Detector;

public class Spirit : MonoBehaviour
{
    public bool isTaken;
    public bool isClosest;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
      /*  if (isClosest)
        {
            meshRenderer.material = selectedMaterial;
        }
        else
        {
            meshRenderer.material = unselectedMaterial;
        }*/
        
        if (isTaken)
        {
            rb.isKinematic = true;
           // meshRenderer.material = unselectedMaterial;
        }
        else
        {
            rb.isKinematic = false;
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
}
