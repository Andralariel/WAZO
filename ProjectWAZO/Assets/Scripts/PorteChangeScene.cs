using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PorteChangeScene : MonoBehaviour
{
  public bool isGD;
  public bool isGA;
  public bool isOpenWorld;

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == 6)
    {
      if (isGD)
      {
        SceneManager.LoadScene("SceneGDPoc");
      }
      else if(isGA)
      {
        SceneManager.LoadScene("SceneGAPoc");
      }
      else if(isOpenWorld)
      {
        SceneManager.LoadScene("OpenWorld");
      }
    }
  }
}
