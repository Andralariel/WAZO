using UnityEngine;
using UnityEngine.VFX;

namespace TechArt
{
    public class InteractiveGrass : MonoBehaviour
    {
        [SerializeField] Material[] mats;
        [SerializeField] VisualEffect[] beacons;
        [SerializeField] private GameObject character;

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].SetVector("_Characterpos", character.transform.position);
            }
            
            for (int i = 0; i < beacons.Length; i++)
            {
                float distance = Vector3.Distance (character.transform.position, new Vector3(beacons[i].transform.position.x,beacons[i].transform.position.y-6,beacons[i].transform.position.z));
                Debug.Log(distance);
                beacons[i].SetFloat("Distance_Joueur", distance);
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].SetVector("_Characterpos", new Vector3(0,0,0));
            }
            for (int i = 0; i < beacons.Length; i++)
            {
                beacons[i].SetVector3("Distance_Joueur", new Vector3(0,0,0));
            }
        }
    }
}
