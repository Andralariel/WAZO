using UnityEngine;

namespace TechArt
{
    public class InteractiveGrass : MonoBehaviour
    {
        [SerializeField] Material[] mats;
        [SerializeField] private GameObject character;

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].SetVector("_Characterpos", character.transform.position);
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].SetVector("_Characterpos", new Vector3(0,0,0));
            }
        }
    }
}
