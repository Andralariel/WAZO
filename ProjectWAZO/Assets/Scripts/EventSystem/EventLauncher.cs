using UnityEngine;

namespace EventSystem
{
    public class EventLauncher : MonoBehaviour
    {
        [SerializeField] private ScriptedEvent[] events;

        [SerializeField] private bool repeatable;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                foreach (var scriptedEvent in events)
                {
                    scriptedEvent.OnEventActivate();
                }

                if (!repeatable) enabled = false;
            }
           
        }
    }
}
