using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Utilitaire;

namespace EventSystem
{
    public class SpiritEvent : ScriptedEvent
    {
        public Vector3[] waypoints;
        
        [SerializeField] private float speed = 1;

        private float _totalDistance;
        
        public override void OnEventActivate()
        {
            //CalculateDistance();
            foreach (var spirit in linkedObjects)
            {
                var spiritScript = spirit.GetComponent<Spirit>();
                spiritScript.SetDestination(this);
            }
        }

        private void CalculateDistance()
        {
            var lastPoint = transform.position;
            foreach (var waypoint in waypoints)
            {
                _totalDistance += (waypoint - lastPoint).magnitude;
                lastPoint = waypoint;
            }
        }

#if UNITY_EDITOR
        //Fonction Debug
        [ContextMenu("DoPath")]
        private void DoPath()
        {
            OnEventActivate();
        }
#endif
    }
    
    // //WaypointElement
    // public class Waypoint
    // {
    //     public enum Behaviour
    //     {
    //         Move,
    //         Gather,
    //         Disperse
    //     }
    //     
    //     public Vector3 position;
    //     public Behaviour behaviour;
    //     public float spiritSpeed;
    // }
    
    //Custom Editor for editing waypoints
    [CustomEditor(typeof(SpiritEvent))]
    public class WaypointsEditor : Editor
    {
        public void OnSceneGUI()
        {
            var t = target as SpiritEvent;
            for (int i = 0; i < t.waypoints.Length; i++)
            {
                t.waypoints[i] = Handles.PositionHandle(t.waypoints[i], quaternion.identity);
            }
            Handles.DrawPolyLine(t.waypoints);
            
            
        }
    }
}
