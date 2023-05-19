using System.Collections;
using _3C;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Controller player;
    public GameObject thingToLook;
    public Transform PointToGo;
    public Transform CameraPoint;
    public GameObject crédits;
    public Image BlackScreen;
    public bool EndedMoving;
    public float creditSpeed;
    public float timeToEnd;
    public float timeToGo;
    private bool rollCredits;
    public float RotateSpeed;
    public float cameraSpeed1;
    public float cameraSpeed2;
    public float playerSpeed;
    private void Update()
    {
        if (EndedMoving)
        {
            Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
            var rotation = Quaternion.LookRotation(pointToGo);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotation, Time.deltaTime * RotateSpeed);
        }

        if (rollCredits)
        {
            crédits.transform.position += new Vector3(0, creditSpeed, 0)*Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            EndedMoving = true;
            CinématiqueManager.instance.isCinématique = false;
            player.canMove = false;
            CameraController.instance.transform.DOMoveX(CameraPoint.position.x,0.2f).SetEase(Ease.Linear)
                .OnComplete( (() => CameraController.instance.transform.DOMove(CameraController.instance.transform.position + Vector3.forward* 11.1f + 
                                                                               CameraController.instance.transform.forward*3, 2.5f)));
            CameraController.instance.SmoothMoveFactor = 0.8f;
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
        CameraController.instance.camShake = false;
        EndedMoving = false;
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + Vector3.forward * cameraSpeed1, 4f);
        player.canPlaner = true;
        player.isPressing = true;
        player.planingGravity = 6;
        player.moveInput = transform.InverseTransformDirection(transform.forward/playerSpeed);
        player.rb.AddForce(new Vector3(0,20,15),ForceMode.Impulse);
        yield return new WaitForSeconds(4f);
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + Vector3.forward * cameraSpeed2, 4f);
        player.planingGravity = -2;
        yield return new WaitForSeconds(4f);
        CameraController.instance.canMove = true;
        CameraController.instance.SmoothMoveFactor = 1.5f;
        CameraController.instance.offset = new Vector3(8.5f,10f,1.5f);
        player.MoveEnding = true;
        player.StopGravity = true;
        player.rb.useGravity = false;
        player.planingGravity = 0;
        yield return new WaitForSeconds(5f);
        crédits.gameObject.SetActive(true);
        rollCredits = true;
        yield return new WaitForSeconds(timeToEnd);
        CameraController.instance.SmoothMoveFactor = 10;
        CameraController.instance.offset = new Vector3(8.5f,10f,-100.5f);
        BlackScreen.DOFade(1, 3f);
        yield return new WaitForSeconds(3.2f);
        SceneManager.LoadScene("Dev_CinematicIntro");
    }
}
