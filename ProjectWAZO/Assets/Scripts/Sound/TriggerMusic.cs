using UnityEngine;

namespace Sound
{
    public class TriggerMusic : MonoBehaviour
    {
        [SerializeField] private AudioList.Music music;
        [SerializeField] private bool loop = true;
        private void OnTriggerEnter(Collider other)
        {
            AudioList.Instance.StartMusic(music,loop);
            gameObject.SetActive(false);
        }
    }
}
