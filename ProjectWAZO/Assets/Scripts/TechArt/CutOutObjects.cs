using System.Collections.Generic;
using UnityEngine;

namespace TechArt
{
    public class CutOutObjects : MonoBehaviour
    {
        [SerializeField] private Shader seeThroughShader;
        [SerializeField] private List<Renderer> meshes;
        [SerializeField] private List<Shader> meshesShader;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 3) return; //layer 3 = ground
            //Debug.Log(other.gameObject.name + " is transparent");
            var mesh = other.GetComponent<Renderer>();
            
            if (mesh == null) return;
            meshes.Add(mesh);
            meshesShader.Add(mesh.material.shader);
            mesh.material.shader = seeThroughShader;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != 3) return; //layer 3 = ground
            //Debug.Log(other.gameObject.name + " is opaque");
            var mesh = other.GetComponent<Renderer>();
            var index = meshes.IndexOf(mesh);
            
            if (index == -1) return;
            mesh.material.shader = meshesShader[index];
            meshesShader.RemoveAt(index);
            meshes.RemoveAt(index);
        }
    }
}
