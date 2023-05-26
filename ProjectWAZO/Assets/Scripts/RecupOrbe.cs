using System.Collections;
using _3C;
using DG.Tweening;
using Sound;
using UnityEngine;

public class RecupOrbe : MonoBehaviour
{
    public GameObject orbe;
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
    [SerializeField] private AudioSource earthquakeSound;
    
    private void Start()
    {
        earthquakeSound.clip = AudioList.Instance.earthquake;
    }
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
            player.ChangeAnimSpeed(1);
            player.anim.SetBool("isWalking",false);
            player.anim.SetBool("isIdle",true);
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
            Controller.instance.ultraBlock = false;
            CinématiqueManager.instance.isCinématique = true;
            isMoving = true;
            player.canMove = false;
            player.ChangeAnimSpeed(0.3f);
            player.anim.SetBool("isWalking",true);
            player.anim.SetBool("isIdle",false);
            player.canJump = false;
            AudioList.Instance.PlayOneShot(AudioList.Instance.cinematiqueOrbe, AudioList.Instance.cinematiqueOrbeVolume);
            Vector3 pointToGo = new Vector3(PointToGo.position.x, player.transform.position.y, PointToGo.position.z);
            player.transform.DOMove(pointToGo, timeToGo).SetEase(Ease.Linear).OnComplete((() => StartCoroutine(EndCinématic())));
        }
    }

    IEnumerator EndCinématic()
    {
        isMoving = false;
        EndedMoving = true;
        player.ChangeAnimSpeed(1f);
        player.anim.SetBool("isWalking",false);
        player.anim.SetBool("isIdle",true);
        CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*5, 8f);
        yield return new WaitForSeconds(2);
        orbe.transform.DOMove(Controller.instance.transform.position, 3f);
        yield return new WaitForSeconds(1.5f);
        vfxreplacing.Play();
        basic.Stop();
        yield return new WaitForSeconds(4.5f);
        earthquakeSound.Play();
        CameraController.instance.camShake = true;
        yield return new WaitForSeconds(2f);
        Eboulement.SetActive(true);
        Eboulement.transform.DOMove(new Vector3(Eboulement.transform.position.x, Eboulement.transform.position.y - 20, 
            Eboulement.transform.position.z), 0.5f);
        yield return new WaitForSeconds(2f);
        CinématiqueManager.instance.isCinématique = false;
        player.canMove = true;
        player.canJump = true;
        player.enabled = true;
        Destroy(gameObject);
    }
    
    
}
