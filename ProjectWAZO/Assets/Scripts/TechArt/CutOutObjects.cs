using UnityEngine;

namespace TechArt
{
    public class CutOutObjects : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Material[] mats;
        [SerializeField] private LayerMask maskToTarget;
        [SerializeField] private float size;
        [SerializeField] private float detectionRadius;
        [SerializeField] private Camera mainCamera;
        
        void Update()
        {
            var offset = player.position - transform.position;
            for (int i = 0; i < mats.Length; i++)
            {
                if (Physics.SphereCast(transform.position, detectionRadius, offset,out RaycastHit hit,offset.magnitude,maskToTarget))
                {
                    mats[i].SetFloat("_CutoutSize",size);
                }
                else mats[i].SetFloat("_CutoutSize",0);
            
                var view = mainCamera.WorldToViewportPoint(player.position);
                mats[i].SetVector("_CutoutPosition", view);
            }
        }
    }
}
