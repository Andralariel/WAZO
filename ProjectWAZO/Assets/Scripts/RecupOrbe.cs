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
    public float timeToGo;
   
    [SerializeField] private AudioSource earthquakeSound;
    
    private void Start()
    {
        earthquakeSound.clip = AudioList.Instance.earthquake;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Controller.instance.isGoing = true;
            Controller.instance.pointToGo = PointToGo.gameObject;
            Controller.instance.cineSpeed = 0.8f;
            Controller.instance.thingToLook = thingToLook;
            CinématiqueManager.instance.isCinématique = true;
            AudioList.Instance.PlayOneShot(AudioList.Instance.cinematiqueOrbe, AudioList.Instance.cinematiqueOrbeVolume);
            StartCoroutine(EndCinématic());
        }
    }

    IEnumerator EndCinématic()
    {
        yield return new WaitForSeconds(timeToGo);
        player.isGoing = false;
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
