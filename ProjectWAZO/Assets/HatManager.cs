using System;
using System.Collections;
using System.Collections.Generic;
using _3C;
using Sound;
using UnityEngine;

public class HatManager : MonoBehaviour
{
    public bool isPut;
    public Vector3 putPosition;
    public ParticleSystem VFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            DataKeeper.instance.isHat = true;
            isPut = true;
            transform.parent = other.transform;
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            transform.localPosition = putPosition;
            transform.Rotate(new Vector3(-18, 0, 0));
            AudioList.Instance.PlayOneShot(AudioList.Instance.hatOnHead, AudioList.Instance.hatOnHeadVolume);
            VFX.gameObject.SetActive(false);
        }
    }

    public void PutHat()
    {
        gameObject.SetActive(true);
        isPut = true;
        transform.parent = Controller.instance.transform;
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        transform.localPosition = putPosition;
        transform.Rotate(new Vector3(-18, 0, 0));
        AudioList.Instance.PlayOneShot(AudioList.Instance.hatOnHead, AudioList.Instance.hatOnHeadVolume);
        VFX.gameObject.SetActive(false);
    }
}
