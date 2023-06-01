using System;
using System.Collections;
using _3C;
using DG.Tweening;
using Sound;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RecupOrbe : MonoBehaviour
{
    public GameObject orbe;
    [SerializeField] private ParticleSystem vfxreplacing;
    [SerializeField] private ParticleSystem basic;
    public Controller player;
    public GameObject thingToLook;
    public Transform PointToGo;
    public GameObject Eboulement;
    public GameObject Eboulement2;
    public GameObject Eboulement3;
    public float timeToGo;
    public AudioSource earthquakeSound;
    private float graphValue;
    public AnimationCurve curveChromatic;
    public AnimationCurve curveSaturation;
    private bool orbed;
    [SerializeField] private VolumeProfile v;
    private ChromaticAberration c;
    private ColorAdjustments ca;
    private float time;
    private ParticleSystem vfxsmoke1;
    private ParticleSystem vfxsmoke3;
    private ParticleSystem vfxsmoke2;
    private void Start()
    {
        time = 0;
        orbed = false;
        v.TryGet(out c);
        v.TryGet(out ca);
        Eboulement.TryGetComponent(out vfxsmoke1);
        Eboulement2.TryGetComponent(out vfxsmoke2);
        Eboulement3.TryGetComponent(out vfxsmoke3);
    }
    void Update()
    {
        if (orbed)
        {
            time ++;
            graphValue = curveChromatic.Evaluate(time/120);
            c.intensity.value = graphValue;
            graphValue = curveSaturation.Evaluate(time/120);
            ca.saturation.value = graphValue;
        }

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
        orbe.transform.DOMove(new Vector3(Controller.instance.transform.position.x,Controller.instance.transform.position.y+1,Controller.instance.transform.position.z), 3f);
        yield return new WaitForSeconds(1.5f);
        basic.Stop();
        vfxreplacing.Play();
        orbed = true;
        yield return new WaitForSeconds(4.5f);
        CameraController.instance.camShake = true;
        earthquakeSound.Play();
        yield return new WaitForSeconds(2f);
        Eboulement.SetActive(true);
        Eboulement2.SetActive(true);
        Eboulement3.SetActive(true);
        Eboulement.transform.DOMove(new Vector3(Eboulement.transform.position.x, Eboulement.transform.position.y - 20, 
            Eboulement.transform.position.z), 0.5f);
        Eboulement2.transform.DOMove(new Vector3(Eboulement2.transform.position.x, Eboulement2.transform.position.y - 20, 
            Eboulement2.transform.position.z), 0.2f);
        Eboulement3.transform.DOMove(new Vector3(Eboulement3.transform.position.x, Eboulement3.transform.position.y - 20, 
            Eboulement.transform.position.z), 0.4f);
        yield return new WaitForSeconds(0.2f);
        vfxsmoke2.Play();
        AudioList.Instance.PlayOneShot(AudioList.Instance.fallingRock, AudioList.Instance.fallingRockVolume);
        yield return new WaitForSeconds(0.2f);
        vfxsmoke1.Play();
        yield return new WaitForSeconds(0.1f);
        vfxsmoke3.Play();
        yield return new WaitForSeconds(1f);
        CinématiqueManager.instance.isCinématique = false;
        player.canMove = true;
        player.canJump = true;
        player.enabled = true;
        CameraController.instance.CameraCollider(false);
        Destroy(gameObject);
    }
}
