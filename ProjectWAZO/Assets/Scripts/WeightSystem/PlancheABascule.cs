using System.Collections.Generic;
using UnityEngine;
using WeightSystem.Detector;

public class PlancheABascule : WeightDetector
{
    public enum HitboxType
    {
        gauche,
        droite,
        centre
    }

    public PlancheABascule master;
    public int poidGauche;
    public int poidDroite;
    public Quaternion positionNeutre;
    public Quaternion positionDroite;
    public Quaternion positionGauche;
    public float rotationSpeed;
    public HitboxType type;
    public List<Rigidbody> massListG;
    public List<Rigidbody> massListD;
    public WeightUI associatedLeftUI;
    public WeightUI associatedRightUI;
    
    void Update()
    {
        if (type == HitboxType.centre)
        {
            if (poidDroite > poidGauche && poidDroite != poidGauche)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,positionDroite,rotationSpeed*Time.deltaTime);
            }
            else if (poidDroite < poidGauche && poidDroite != poidGauche)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,positionGauche,rotationSpeed*Time.deltaTime);
            }
            
            if (poidDroite == poidGauche)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,positionNeutre,rotationSpeed*Time.deltaTime);
            }

            associatedLeftUI.currentWeight = poidGauche;
            associatedRightUI.currentWeight = poidDroite;
          
        }
        var transformRotation = transform.rotation;
        transformRotation.y = 0;
        transform.rotation = transformRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == HitboxType.gauche)
        {
            if (other.gameObject.layer == 7)
            {
                master.massListG.Add(other.attachedRigidbody);
            }
            else if(other.gameObject.layer == 6)
            {
                master.massListG.Add(other.attachedRigidbody);
            }
        }
        else if (type == HitboxType.droite)
        {
            if (other.gameObject.layer == 7)
            {
                master.massListD.Add(other.attachedRigidbody);
            }
            else if(other.gameObject.layer == 6)
            {
                master.massListD.Add(other.attachedRigidbody);
            }
        }
        
        master.ResetWeight();
        Controller.instance.SetDetector(master);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (type == HitboxType.droite)
        {
            if (other.gameObject.layer == 7)
            {
                master.massListD.Remove(other.attachedRigidbody);
            }
            else if(other.gameObject.layer == 6)
            {
                master.massListD.Remove(other.attachedRigidbody);
            }
        }
        else if (type == HitboxType.gauche && other.gameObject.layer == 7 || other.gameObject.layer == 6)
        {
            if (other.gameObject.layer == 7)
            {
                master.massListG.Remove(other.attachedRigidbody);
            }
            else if(other.gameObject.layer == 6)
            {
                master.massListG.Remove(other.attachedRigidbody);
            }
        }
        master.ResetWeight();
    }

    protected override void LimitCheck()
    {
        poidDroite = 0;
        poidGauche = 0;
        foreach (var rb in massListD)
        {
            poidDroite += (int)rb.mass;
        }
        
        foreach (var rb in massListG)
        {
            poidGauche += (int)rb.mass;
        }
    }
}
