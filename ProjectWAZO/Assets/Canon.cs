
using System.Collections;
using _3C;
using DG.Tweening;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject canon;
    public float canonForce;
    public float gravityScaleToGive;
    public float AirSpeedToGive;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            StartCoroutine(FireWazo());
            Controller.instance.isOnHugeWind = true;
            Controller.instance.hugeWindAirSpeed = AirSpeedToGive;
            Controller.instance.currentWindGravityScale = gravityScaleToGive;
            Controller.instance.canMove = false;
            Controller.instance.canJump = false;
            Controller.instance.ultraBlock = true;
        }
    }

    IEnumerator FireWazo()
    {
        canon.transform.DOMove(transform.position + new Vector3(0, 3, 0),2f);
        yield return new WaitForSeconds(2.5f);
        Controller.instance.rb.AddForce(new Vector3(0,canonForce,0),ForceMode.Impulse);
        Controller.instance.ultraBlock = false;
        yield return new WaitForSeconds(1f);
        Controller.instance.canMove = true;
        Controller.instance.canJump = true;
    }
}
