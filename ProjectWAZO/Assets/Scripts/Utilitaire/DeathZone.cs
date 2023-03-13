using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      other.transform.position = new Vector3(-0.9f, 1, -4.3f);
   }
}
