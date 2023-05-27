using System.Collections;
using _3C;
using DG.Tweening;
using UnityEngine;
using WeightSystem.Activator;

public class TempleManager : Activator
{
    public static TempleManager instance;
    public Animator door;
    public bool EnableCinématique;
    public int indexCalling;
    public bool Escalier1Done;
    public bool Escalier2Done;
    private Vector3 originalOffset;
    public GameObject Escalier1;
    public GameObject Escalier2;
    
    void Start()
    {
        StartCoroutine(CinématiqueStart());
        door.SetBool("Open",true);
        if (instance == null)
        {
            instance = this;
        }
    }

    public override void Activate()
    {
        if (indexCalling == 1 && !Escalier1Done)
        {
            StartCoroutine(ActivateEscalier1());
        }

        if (indexCalling == 2 && !Escalier2Done)
        {
            StartCoroutine(ActivateEscalier2());
        }
    }
    public override void Deactivate()
    {
        
    }
    IEnumerator ActivateEscalier1()
    {
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
    }
    
    IEnumerator ActivateEscalier2()
    {
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
    }
        
    
    IEnumerator CinématiqueStart()
    {
        if (EnableCinématique)
        {
            Controller.instance.canMove = false;
            Controller.instance.canJump = false;
            Controller.instance.transform.DOMove(new Vector3(Controller.instance.transform.position.x, Controller.instance.transform.position.y,
                Controller.instance.transform.position.z + 6),1.5f);
            yield return new WaitForSeconds(1.5f);
            door.transform.DOMove(new Vector3(door.transform.position.x, door.transform.position.y + 5, door.transform.position.z), 1f);
            Controller.instance.canMove = true;
            Controller.instance.canJump = true;
        }
    }
}
