using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public Controller player;
    public GameObject thingToLook;
    public Transform PointToGo;
     public Transform CameraPoint;
    public  bool isMoving;
    public bool EndedMoving;
    public float timeToGo;
    public float RotateSpeed;
    public float cameraSpeed;
    public float playerSpeed;
    private void Update()
    {
       
        if (EndedMoving)
        {
            player.ChangeAnimSpeed(1);
            player.anim.SetBool("isWalking",false);
            player.anim.SetBool("isIdle",true);
            isMoving = false;
            var lookPos = player.transform.position - thingToLook.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * RotateSpeed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        { 
            CameraController.instance.transform.DOMove(CameraPoint.position, 1f);
            CameraController.instance.canMove = false;
            CinématiqueManager.instance.isCinématique = true;
            isMoving = true;
            player.canMove = false;
            CameraController.instance.transform.DOMove(CameraController.instance.transform.position + Vector3.forward* cameraSpeed/4 + CameraController.instance.transform.forward*5, 2.5f);
            player.ChangeAnimSpeed(0.3f);
            player.anim.SetBool("isWalking",true);
            player.anim.SetBool("isIdle",false);
            player.canJump = false;
            Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
            player.transform.DOMove(pointToGo, timeToGo).SetEase(Ease.Linear).OnComplete((() => StartCoroutine(EndCinématic())));
            
        }
    }

    IEnumerator EndCinématic()
    {
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + Vector3.forward * cameraSpeed, 10f);
        player.canPlaner = true;
        player.isPressing = true;
        player.planingGravity = 6;
        player.moveInput = transform.InverseTransformDirection(transform.forward/playerSpeed);
        Controller.instance.rb.AddForce(new Vector3(0,20,20),ForceMode.Impulse);
        yield return new WaitForSeconds(3.5f);
        player.planingGravity = -3;
        yield return new WaitForSeconds(5f);
        player.planingGravity = 0;
        player.rb.useGravity = false;
        player.globalGravity = 0;
        player.rb.mass = 0;
        player.StopGravity = true;
    }
}
