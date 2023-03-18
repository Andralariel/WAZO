using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echelleData : MonoBehaviour
{
    private enum Orientation
    {
        nord,
        sud,
        est,
        ouest
    }
        
    [SerializeField] private Orientation orientation;
    private void Update()
    {
        if (Controller.instance.isEchelle && PickUpObjects.instance.currentEchelle == gameObject)
        {
            switch (orientation)
            {
                case Orientation.nord:
                    Controller.instance.rb.velocity += new Vector3(0,Controller.instance.moveInput.z,0) * (Controller.instance.airControlSpeed * Time.deltaTime);
                    break;
                case Orientation.sud:
                    Controller.instance.rb.velocity += new Vector3(0,-Controller.instance.moveInput.z,0) * (Controller.instance.airControlSpeed * Time.deltaTime);
                    break;
                case Orientation.est:
                    Controller.instance.rb.velocity += new Vector3(0,Controller.instance.moveInput.x,0) * (Controller.instance.airControlSpeed * Time.deltaTime);
                    break;
                case Orientation.ouest:
                    Controller.instance.rb.velocity += new Vector3(0,-Controller.instance.moveInput.x,0) * (Controller.instance.airControlSpeed * Time.deltaTime);
                    break;
            }
        }
    }
}
