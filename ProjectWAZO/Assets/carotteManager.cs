using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carotteManager : MonoBehaviour
{
    public bool isPlanted;
    public bool isTaken;
    public Rigidbody rb;
    public BoxCollider carotteColider;
    void Update()
    {
        if (isPlanted)
        {
            carotteColider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        if (isTaken)
        {
            carotteColider.enabled = false;
            isPlanted = false;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
                
        if(!isTaken && !isPlanted)
        {
            carotteColider.enabled = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
