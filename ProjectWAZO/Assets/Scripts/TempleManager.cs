using System.Collections;
using _3C;
using DG.Tweening;
using UnityEngine;
using WeightSystem.Activator;

public class TempleManager : Activator
{
    public static TempleManager instance;
    public bool EnableCinématique;
    public int indexCalling;
    public bool Escalier1Done;
    public bool Escalier2Done;
    private Vector3 originalOffset;
    public GameObject Escalier1;
    public GameObject Escalier2;
    private bool oneDone;
    private bool twoDone;
    public bool isCine;
    
    void Start()
    {
        DataKeeper.instance.CheckHat();
        if (instance == null)
        {
            instance = this;
        }
    }

    public override void Activate()
    {
        if (indexCalling == 1 && !Escalier1Done)
        {
            if (!oneDone)
            {
                StartCoroutine(ActivateEscalier1());
            }
        }

        if (indexCalling == 2 && !Escalier2Done)
        {
            if (!twoDone)
            {
                StartCoroutine(ActivateEscalier2());
            }
        }
    }
    public override void Deactivate()
    {
        
    }
    IEnumerator ActivateEscalier1()
    {
        isCine = true;
        oneDone = true;
        CinématiqueManager.instance.isCinématique = true;
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = true;
        originalOffset = CameraController.instance.offset;
        CameraController.instance.offset = new Vector3(4, 25, -20.2f);
        CameraController.instance.SmoothMoveFactor = 0.6f;
        CameraController.instance.walkingLookFactor = 0;
        CameraController.instance.player = Escalier1;
        yield return new WaitForSeconds(4f);
        CameraController.instance.camShake = true;
        Escalier1.transform.DOMove(
            new Vector3(Escalier1.transform.position.x, Escalier1.transform.position.y + 6,
                Escalier1.transform.position.z), 2f);
        yield return new WaitForSeconds(2f);
        CameraController.instance.camShake = false;
        yield return new WaitForSeconds(1f);
        CameraController.instance.SmoothMoveFactor = 0.2f;
        CameraController.instance.walkingLookFactor = 0.1f;
        CameraController.instance.player = Controller.instance.gameObject;
        CameraController.instance.offset = originalOffset;
        yield return new WaitForSeconds(2);
        Controller.instance.canMove = true;
        Controller.instance.canJump = true;
        CinématiqueManager.instance.isCinématique = false;
        isCine = false;
    }
    
    IEnumerator ActivateEscalier2()
    {
        isCine = true;
        twoDone = true;
        CinématiqueManager.instance.isCinématique = true;
        Controller.instance.canMove = false;
        Controller.instance.canJump = false;
        CameraController.instance.canMove = true;
        originalOffset = CameraController.instance.offset;
        CameraController.instance.offset = new Vector3(4, 25, -20.2f);
        CameraController.instance.SmoothMoveFactor = 0.3f;
        CameraController.instance.walkingLookFactor = 0;
        CameraController.instance.player = Escalier2;
        yield return new WaitForSeconds(3f);
        CameraController.instance.camShake = true;
        Escalier2.transform.DOMove(
            new Vector3(Escalier2.transform.position.x, Escalier2.transform.position.y + 6,
                Escalier2.transform.position.z), 2f);
        yield return new WaitForSeconds(2f);
        CameraController.instance.camShake = false;
        yield return new WaitForSeconds(1f);
        CameraController.instance.SmoothMoveFactor = 0.2f;
        CameraController.instance.walkingLookFactor = 0.1f;
        CameraController.instance.offset = originalOffset;
        CameraController.instance.player = Controller.instance.gameObject;
        yield return new WaitForSeconds(2);
        Controller.instance.canMove = true;
        Controller.instance.canJump = true;
        CinématiqueManager.instance.isCinématique = false;
        isCine = false;
    }
}
