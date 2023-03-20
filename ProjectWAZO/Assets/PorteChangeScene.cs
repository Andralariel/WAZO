using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PorteChangeScene : MonoBehaviour
{
  public bool isGD;

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == 6)
    {
      if (isGD)
      {
        SceneManager.LoadScene("OpenWorld");
      }
      else
      {
        SceneManager.LoadScene("SceneGAPoc");
      }
    }
  }
}
