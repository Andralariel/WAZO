using System;
using System.Collections;
using System.Collections.Generic;
using _3C;
using DG.Tweening;
using Sound;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Utilitaire
{
   public class TempleOpener : MonoBehaviour
   {
      public float currentAmount;
      public int AmountToOpen;
      public bool canOpen;
      public static TempleOpener instance;
      public Animator animDoor;
      public GameObject Serrure;
      public GameObject Clé;
      public GameObject rassemblementClé;
      public Image whiteScreen;
      public List<GameObject> keyShardCinématique;
      public List<GameObject> emptyPosition;
      public BoxCollider colliderPorte;
      private float graphValue;
      public AnimationCurve curveChromatic;
      public AnimationCurve curveSaturation;
      public AnimationCurve curveBloom;
      public AnimationCurve curveBloomT;
      [SerializeField] private VolumeProfile v;
      private ChromaticAberration c;
      private ColorAdjustments ca;
      private Bloom b;
      private float time;
      private bool keyed;
      [SerializeField] private ParticleSystem vfxsmoke;

      private void Start()
      {
         time = 0;
         keyed = false;
         v.TryGet(out c);
         v.TryGet(out ca);
         v.TryGet(out b);
      }

      
      private void Awake()
      {
         if (instance == null)
         {
            instance = this;
         }
      }
      
      void Update()
      {
         if (keyed)
         {
            time ++;
            graphValue = curveChromatic.Evaluate(time/250);
            c.intensity.value = graphValue;
            graphValue = curveSaturation.Evaluate(time/250);
            ca.saturation.value = graphValue;
            graphValue = curveBloom.Evaluate(time/250);
            b.intensity.value = graphValue;
            graphValue = curveBloomT.Evaluate(time/250);
            b.threshold.value = graphValue;
         }

      }

      public void CheckKeyState()
      {
         if (currentAmount >= AmountToOpen)
         {
            canOpen = true;
         }
      }

      public void OnTriggerEnter(Collider other)
      {
         if (other.gameObject.layer == 6 && canOpen)
         {
            StartCoroutine(CinémtiqueOuverture());
         }

         if (other.gameObject.layer == 6)
         {
            KeyUI.instance.ShowMapKey();
         }
      }
   
      public void OnTriggerExit(Collider other)
      {
         if (other.gameObject.layer == 6)
         {
            KeyUI.instance.HideMapKey();
         }
      }

      IEnumerator CinémtiqueOuverture()
      {
         CinématiqueManager.instance.isCinématique = true;
         CarnetManager.instance.canOpen = false;
         Controller.instance.anim.SetBool("isWalking",false);
         Controller.instance.anim.SetBool("isIdle",true);
         Controller.instance.canMove = false;
         Controller.instance.canJump = false;
         Controller.instance.ultraBlock = true;
         CameraController.instance.canMove = false;
         CameraController.instance.transform.DOMove(CameraController.instance.transform.position + CameraController.instance.transform.forward*18, 7f); 
         yield return new WaitForSeconds(3f);
         Controller.instance.anim.SetBool("isIdle",true);
         AudioList.Instance.PlayOneShot(AudioList.Instance.keyOpenDoor, AudioList.Instance.keyOpenDoorVolume);
         
         foreach (GameObject obj in keyShardCinématique)
         {
            obj.transform.position = Controller.instance.transform.position;
         }

         keyed = true;
         for (int i = 0; i < 6; i++)
         {
            keyShardCinématique[i].SetActive(true);
            float randomNumber = Random.Range(2f, 3f);
            keyShardCinématique[i].transform.DOMove(emptyPosition[i].transform.position, randomNumber);
         }
         yield return new WaitForSeconds(3.5f);
         for (int i = 0; i < 6; i++)
         {
            float randomNumber = Random.Range(2f, 3f);
            keyShardCinématique[i].transform.DOMove(rassemblementClé.transform.position, randomNumber);
         }
         
         yield return new WaitForSeconds(3f);
         whiteScreen.DOFade(1, 0.3f);
         yield return new WaitForSeconds(0.3f);
         Clé.SetActive(true);
         yield return new WaitForSeconds(0.3f);
         whiteScreen.DOFade(0, 0.3f);
         for (int i = 0; i < 6; i++)
         {
            Destroy(keyShardCinématique[i]);
         }
         yield return new WaitForSeconds(3f);
         
         Clé.transform.DOMove(Serrure.transform.position, 5f);
         yield return new WaitForSeconds(4.5f);
         AudioList.Instance.PlayOneShot(AudioList.Instance.keyOnDoor, AudioList.Instance.keyOnDoorVolume);
         Clé.transform.DOLocalRotate(new Vector3(0,90,-45), 0.5f);
         yield return new WaitForSeconds(0.5f);
         animDoor.SetBool("Open",true);
         vfxsmoke.Play();
         CameraController.instance.transform.DOShakePosition(5, 1, 11);
         AudioList.Instance.PlayOneShot(AudioList.Instance.openDoorTemple, AudioList.Instance.openDoorTempleVolume);
         yield return new WaitForSeconds(0.5f);
         Clé.transform.DOMove(Clé.transform.position - new Vector3(0,10,0), 5f);
         yield return new WaitForSeconds(4.5f);
         vfxsmoke.Stop();
         CinématiqueManager.instance.isCinématique = false;
         Controller.instance.canMove = true;
         Controller.instance.canJump = true;
         Controller.instance.ultraBlock = false;
         CameraController.instance.canMove = true;
         colliderPorte.enabled = false;
         Destroy(gameObject);
      }
   }
}
