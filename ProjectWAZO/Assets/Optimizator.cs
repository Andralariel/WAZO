using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizator : MonoBehaviour
{
    public List<GameObject> worldParts;
    public float maxDistance;
    private void Update()
    {
       
        for (int i = 0; i < worldParts.Count; i++)
        {
            Debug.Log(new Vector2(Controller.instance.transform.position.x - worldParts[0].transform.position.x, 
                Controller.instance.transform.position.z - worldParts[0].transform.position.z).magnitude);
            Vector2 distanceWithZone = new Vector2(Controller.instance.transform.position.x - worldParts[i].transform.position.x, 
                Controller.instance.transform.position.z - worldParts[i].transform.position.z);
               
            if (distanceWithZone.magnitude > maxDistance)
            {
                worldParts[i].SetActive(false);
            }
            
            if (distanceWithZone.magnitude < maxDistance)
            {
                worldParts[i].SetActive(true);
            }
        }
      
    }
}
