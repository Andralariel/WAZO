using UnityEngine;

namespace WeightSystem
{
    public class DoorOpener : Activator
    {
        public override void Activate()
        {
            Debug.Log("Door is Open");
        }

        public override void Deactivate()
        {
            Debug.Log("Door is Closed");
        }
    }
}
