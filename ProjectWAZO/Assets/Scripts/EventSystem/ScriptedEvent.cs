using UnityEngine;

namespace EventSystem
{
    public abstract class ScriptedEvent : MonoBehaviour
    {
        [SerializeField] protected GameObject[] linkedObjects;
        
        public abstract void OnEventActivate();
    }
}
