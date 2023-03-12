using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_Grass : MonoBehaviour
{
    public Material mat;
    public GameObject character;

    // Update is called once per frame
    void Update()
    {
        mat.SetVector("_Characterpos", character.transform.position);
    }
}
