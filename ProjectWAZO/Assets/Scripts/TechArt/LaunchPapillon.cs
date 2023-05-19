using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LaunchPapillon : MonoBehaviour
{
    [SerializeField] private VisualEffect papillon;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == 6)
        {
            Debug.Log("entrééé");
            papillon.Play();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.layer == 6)
        {
            Debug.Log("sortie");
            papillon.Stop();
        }
    }
}
