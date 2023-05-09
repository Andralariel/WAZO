using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopit : MonoBehaviour
{
    [SerializeField] private ParticleSystem vfxreplacing;
    [SerializeField] private ParticleSystem basic;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            vfxreplacing.Play();
            basic.Stop();
        }
    }
}
