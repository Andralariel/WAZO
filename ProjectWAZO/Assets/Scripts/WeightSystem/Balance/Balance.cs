using UnityEngine;

namespace WeightSystem.Balance
{
    public abstract class Balance : MonoBehaviour
    {
        public abstract void HighState();
        public abstract void MiddleState();
        public abstract void LowState();
    }
}
