using UnityEngine;

public class carotteManager : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider carotteColider;
    
    private void Start()
    { 
        carotteColider.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void IsTaken()
    {
        carotteColider.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void IsLeft()
    {
        carotteColider.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
