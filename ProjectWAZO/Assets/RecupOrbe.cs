using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RecupOrbe : MonoBehaviour
{
    [SerializeField] private ParticleSystem vfxreplacing;
    [SerializeField] private ParticleSystem basic;
    public Controller player;
    public GameObject thingToLook;
    public Transform PointToGo;
    public GameObject Eboulement;
    public  bool isMoving;
    public bool EndedMoving;
    public float timeToGo;
    public float RotateSpeed;
    private void Update()
    {
        if (isMoving)
        {
            var lookPos = PointToGo.position - player.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * RotateSpeed*5);
        }
      
        if (EndedMoving)
        {
            isMoving = false;
            var lookPos = thingToLook.transform.position - PointToGo.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * RotateSpeed);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            CinématiqueManager.instance.isCinématique = true;
            isMoving = true;
            player.canMove = false;
            player.anim.SetBool("isWalking",true);
            player.anim.SetBool("isIdle",false);
            player.canJump = false;
            Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
            player.transform.DOMove(pointToGo, timeToGo).SetEase(Ease.Linear).OnComplete((() => StartCoroutine(EndCinématic())));
        }
    }

    IEnumerator EndCinématic()
    {
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*5, 8f);
        yield return new WaitForSeconds(2.5f);
        vfxreplacing.Play();
        basic.Stop();
        yield return new WaitForSeconds(4.5f);
        CameraController.instance.transform.DOShakePosition(5, 2, 15);
        yield return new WaitForSeconds(1f);
        Eboulement.SetActive(true);
        Eboulement.transform.DOMove(new Vector3(Eboulement.transform.position.x, Eboulement.transform.position.y - 20,
            Eboulement.transform.position.z), 0.5f);
        yield return new WaitForSeconds(5f);
        player.canMove = true;
        player.canJump = true;
        Destroy(gameObject);
    }
}
