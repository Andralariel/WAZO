using UnityEngine;

namespace TechArt
{
    public class InteractiveGrass : MonoBehaviour
    {
        public Material mat;
        public GameObject character;

        // Update is called once per frame
        void Update()
        {
            mat.SetVector("_Characterpos", character.transform.position);
        }

        void OnDestroy()
        {
            mat.SetVector("_Characterpos", new Vector3(0,0,0));
        }
    }
}
