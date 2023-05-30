using UnityEngine;

namespace Optimisation
{
    public class FPSViewer : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void FixedUpdate()
        {
            var delay = Time.deltaTime;
            if(delay>Time.fixedDeltaTime) Debug.Log("Game is not fine : " + delay);
            else Debug.Log("Game is fine");
        }
    }
}
