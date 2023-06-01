using System;
using _3C;
using UnityEditor;
using UnityEngine;

public class Optimizator : MonoBehaviour
{
    public float loadDistance;
    public Batch[] batches;

    private Transform _player;

    private void Start()
    {
        _player = Controller.instance.transform;
    }

    private void FixedUpdate()
    {
        foreach (var batch in batches)
        {
            var distance = (_player.position-batch.position).magnitude;
            if (distance < loadDistance)
            {
                //if(batch.activated) continue;
                
                batch.activated = true;
                foreach (var thing in batch.objectsToLoad)
                {
                    thing.SetActive(true);
                }
            }
            else
            {
                //if(!batch.activated) continue;
                
                batch.activated = false;
                foreach (var thing in batch.objectsToLoad)
                {
                    thing.SetActive(false);
                }
            }
        }
    }
    
    //WaypointElement
    [Serializable]
    public class Batch
    {
        public string zoneName;
        public Vector3 position;
        public GameObject[] objectsToLoad;
        public bool activated = true;
    }
    
#if UNITY_EDITOR
    //Custom Editor for editing waypoints
    [CustomEditor(typeof(Optimizator))] 
    public class BatchEditor : Editor
    {
        public void OnSceneGUI()
        {
            var t = target as Optimizator;
            if (t.batches == default) return;
            for (int i = 0; i < t.batches.Length; i++)
            {
                if (t.batches[i].position == Vector3.zero) t.batches[i].position = t.transform.position;
                t.batches[i].position = Handles.PositionHandle(t.batches[i].position, Quaternion.identity);
            }
        }
    }
#endif
}
