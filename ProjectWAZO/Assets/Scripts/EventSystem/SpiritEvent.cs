using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace EventSystem
{
    public class SpiritEvent : ScriptedEvent
    {
        public Vector3[] waypoints;
        
        [SerializeField] private float speed = 10;
        
        public override void OnEventActivate()
        {
            foreach (var spirit in linkedObjects)
            {
                spirit.transform.DOPath(waypoints, speed, PathType.CubicBezier);
            }
        }
    }
    
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
