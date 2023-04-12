using UnityEngine;

public class echelleData : MonoBehaviour
{
    public enum Orientation
    {
        nord,
        sud,
        est,
        ouest
    }
        
    [SerializeField] public Orientation orientation;
    public float echelleSpeed = 7;
    private void Update()
    {
        if (Controller.instance.isEchelle && PickUpObjects.instance.currentEchelle == gameObject)
        {
            switch (orientation)
            {
                case Orientation.nord:
                   // Controller.instance.transform.rotation = new Quaternion(-90,0,0,0);
                    Controller.instance.rb.velocity = new Vector3(0,Controller.instance.moveInput.z,0) * (echelleSpeed * (Controller.instance.airControlSpeed * Time.deltaTime));
                    break;
                case Orientation.sud:
                   // Controller.instance.transform.rotation = new Quaternion(-90,180,0,0);
                    Controller.instance.rb.velocity = new Vector3(0,-Controller.instance.moveInput.z,0) * (echelleSpeed * (Controller.instance.airControlSpeed * Time.deltaTime));
                    break;
                case Orientation.est:
                   // Controller.instance.transform.rotation = new Quaternion(-90,90,0,0);
                    Controller.instance.rb.velocity = new Vector3(0,Controller.instance.moveInput.x,0) * (echelleSpeed * (Controller.instance.airControlSpeed * Time.deltaTime));
                    break;
                case Orientation.ouest:
                 //   Controller.instance.transform.rotation = new Quaternion(-90,-90,0,0);
                    Controller.instance.rb.velocity = new Vector3(0,-Controller.instance.moveInput.x,0) * (echelleSpeed * (Controller.instance.airControlSpeed * Time.deltaTime));
                    break;
            }
        }
    }
}
