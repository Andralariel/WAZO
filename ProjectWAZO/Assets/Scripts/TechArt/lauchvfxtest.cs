using System.Collections;
using UnityEngine;

namespace TechArt
{
    public class lauchvfxtest : MonoBehaviour
    {
        [SerializeField] private ParticleSystem vfxtoplayonenter;
        [SerializeField] private ParticleSystem vfxtostop;
        [SerializeField] private GameObject spirit;

        // Start is called before the first frame update
        public void OnTriggerEnter(Collider other)
        {
            vfxtoplayonenter.Play();
            vfxtostop.Stop();
            StartCoroutine(SpawnDelay());
        }
    
        IEnumerator SpawnDelay()
        {
            yield return new WaitForSeconds(1);
            spirit.SetActive(true);
        }
    
    }
}

