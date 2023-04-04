using UnityEngine;

namespace EventSystem
{
    public class EventLauncher : MonoBehaviour
    {
        [SerializeField] private ScriptedEvent[] events;

        [SerializeField] private bool repeatable;
        
        private void OnTriggerEnter(Collider other)
        {
            foreach (var scriptedEvent in events)
            {
                scriptedEvent.OnEventActivate();
            }

            if (!repeatable) enabled = false;
        }
    }
}
