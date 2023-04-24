using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Utilitaire;

namespace EventSystem
{
    public class SpiritEvent : ScriptedEvent
    {
        [SerializeField] private Spirit[] linkedObjects;
        
        public Waypoint[] waypoints;
        
        [SerializeField] private float stepStopDistanceIncrease = 0.25f;
        private float _currentStopDistance;
        private int _spiritsWaitingAmount;

        [Header("Disappearance")]
        public bool disappearOnEnd = true;
        public bool disappearDuringLastStep;
        public float disappearLowerBound = 0.2f;
        public float disappearUpperBound = 1f;

        [Header("Dispersion")]
        public float dispersionDistance = 10f;
        public float spreadMaxAngle = 180;
        public float endDispersionSpeed = 3.5f;
        
        public override void OnEventActivate()
        {
            foreach (var spirit in linkedObjects)
            {
                spirit.StartEvent(this);
            }
        }

        public void SpiritWait()
        {
            _spiritsWaitingAmount++;
            
            if (_spiritsWaitingAmount < linkedObjects.Length) return;
            foreach (var spirit in linkedObjects)
            {
                spirit.AllSpiritStartToWait();
            }
            _spiritsWaitingAmount = 0;
        }

        public float StopDistance()
        {
            var stopDistance = _currentStopDistance + _spiritsWaitingAmount * stepStopDistanceIncrease;
            return stopDistance;
        }

        public void ResetStopDistance(float stopDistance)
        {
            _currentStopDistance = stopDistance;
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
    
    //WaypointElement
    [Serializable]
    public class Waypoint
    {
        public enum Behaviour
        {
            Move,
            Gather,
            Disperse,
            Disappear
        }
        
        public Vector3 position;
        public Behaviour behaviour;
        public float spiritSpeed = 3.5f;
        public float spiritStopDistance = 0.5f;
        public float waitBetweenStep;
        public bool allSpiritsWait;
    }
    
#if UNITY_EDITOR
    //Custom Editor for editing waypoints
    [CustomEditor(typeof(SpiritEvent))] 
    public class WaypointsEditor : Editor
    {
        public void OnSceneGUI()
        {
            var t = target as SpiritEvent;
            if (t.waypoints == default) return;
            for (int i = 0; i < t.waypoints.Length; i++)
            {
                t.waypoints[i].position = Handles.PositionHandle(t.waypoints[i].position, quaternion.identity);

                if (i <= 0) continue;
                var color = t.waypoints[i].behaviour switch
                {
                    Waypoint.Behaviour.Move => Color.white,
                    Waypoint.Behaviour.Gather => Color.blue,
                    Waypoint.Behaviour.Disperse => Color.red,
                    _ => Color.red
                };
                Handles.color = color;
                Handles.DrawLine(t.waypoints[i-1].position,t.waypoints[i].position);
            }
        }
    }
#endif
}
