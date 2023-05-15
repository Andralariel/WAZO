using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.Rendering;

public class CutOutObjects : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Material[] mats;
    [SerializeField] private LayerMask masktotarget;
    [SerializeField] private float size;
    private Camera mainCamera;
 
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }


    void Update()
    {
        var offset = player.position - transform.position;
        for (int i = 0; i < mats.Length; i++)
        {
            if (Physics.Raycast(transform.position, offset, offset.magnitude, masktotarget))
            {
                mats[i].SetFloat("_CutoutSize",size);
                Debug.Log("touché");
            }
            else
                mats[i].SetFloat("_CutoutSize",0);
            
            var view = mainCamera.WorldToViewportPoint(player.position);
            mats[i].SetVector("_CutoutPosition", view);
        }
        

    }
}
