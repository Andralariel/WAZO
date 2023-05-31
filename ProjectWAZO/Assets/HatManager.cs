using System;
using System.Collections;
using System.Collections.Generic;
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
            isPut = true;
            transform.parent = other.transform;
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            transform.localPosition = putPosition;
            transform.Rotate(new Vector3(-18, 0, 0));
            VFX.gameObject.SetActive(false);
        }
    }
}
