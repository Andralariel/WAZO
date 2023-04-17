using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Activator = WeightSystem.Activator.Activator;

public class Moulin : Activator
{
    public GameObject hélice;
    public float rotationSpeed;
    public int activatedAltar;
    public bool isActive;
    public List<GameObject> plateformes;
    public List<GameObject> plateformesPoints;

    private void Update()
    {
        if (activatedAltar == 1)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
        
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
        activatedAltar += 1;
    }
    
    public override void Deactivate()
    {
        activatedAltar -= 1;
    }
}
    
