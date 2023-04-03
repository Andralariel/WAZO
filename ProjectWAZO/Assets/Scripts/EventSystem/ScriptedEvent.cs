using UnityEngine;

namespace EventSystem
{
    public abstract class ScriptedEvent : MonoBehaviour
    {
        protected GameObject[] LinkedObjects;
        
        public abstract void OnEventActivate();
    }
}
