using System;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using Activator = WeightSystem.Activator.Activator;

public class Moulin : Activator
{
    public GameObject hélice;
    public float rotationSpeed;
    public bool isActive;
    public List<GameObject> plateformes;
    public List<GameObject> plateformesPoints;
    [SerializeField] private AudioSource audioWindMill;

    private void Start()
    {
        audioWindMill.clip = AudioList.Instance.turnWindmill;
    }

    private void Update()
    {
        if (isActive)
        {
            hélice.transform.Rotate ( Vector3.forward * ( rotationSpeed * Time.deltaTime));
            for (int i = 0; i < 3; i++)
            {
                plateformes[i].transform.position = plateformesPoints[i].transform.position;
            }
        }
    }

    public override void Activate()
    {
        isActive = true;
        audioWindMill.Play();
    }
    
    public override void Deactivate()
    {
        isActive = false;
        audioWindMill.Stop();
    }
}
    
