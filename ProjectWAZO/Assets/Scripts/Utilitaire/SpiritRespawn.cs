using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilitaire;

public class SpiritRespawn : MonoBehaviour
{
    //VARIABLES ************************************************************************************************************
    public List<Spirit> spiritsToRespawn;
    public float durationUntilReset = 3;

    [Header("DEBUG")] 
    public bool isInTrigger;
    public float holdingDuration;
    
    //FONCTIONS SYSTEMES ****************************************************************************************************
    void Start()
    {
        holdingDuration = 0f;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        isInTrigger = true;
        PickUpObjects.instance.spiritRespawn = GetComponent<SpiritRespawn>();
    }

    private void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
        PickUpObjects.instance.StopDoRespawn();
        PickUpObjects.instance.spiritRespawn = null;
    }
}
