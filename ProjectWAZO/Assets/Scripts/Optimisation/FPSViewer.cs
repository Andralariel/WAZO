using TMPro;
using UnityEngine;

namespace Optimisation
{
    public class FPSViewer : MonoBehaviour
    {
        [SerializeField] private Canvas debugMenu;
        [SerializeField] private TextMeshProUGUI textMesh;
        
        private void Awake()
        {
            DontDestroyOnLoad(debugMenu);
        }

        private void Update()
        {
            var delay = Time.unscaledDeltaTime;
            if(delay>0.016) textMesh.text = "Bad  : " + Time.unscaledDeltaTime;
            else textMesh.text = "Good : " + Time.unscaledDeltaTime;
        }
    }
}
