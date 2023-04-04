using UnityEngine;

namespace EventSystem
{
    public abstract class ScriptedEvent : MonoBehaviour
    { 
        public abstract void OnEventActivate();
    }
}
