using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpirit : MonoBehaviour
{
    public bool canFall;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (canFall)
        {
            //rb.constraints =  RigidbodyConstraints.FreezeRotation;
            rb.isKinematic = false;
        }
        else
        {
            //rb.constraints =  RigidbodyConstraints.FreezeRotation;
            rb.isKinematic = true;
        }
    }
}
