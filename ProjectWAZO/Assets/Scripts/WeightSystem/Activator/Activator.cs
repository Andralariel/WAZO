using UnityEngine;

namespace WeightSystem.Activator
{
    public abstract class Activator : MonoBehaviour
    {
        public abstract void Activate();
        public abstract void Deactivate();
    }
}
