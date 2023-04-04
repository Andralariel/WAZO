using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace EventSystem
{
    public class SpiritEvent : ScriptedEvent
    {
        public Vector3[] waypoints;
        
        [SerializeField] private float speed = 1;

        private float _totalDistance;
        private int _currentWaypoint;
        
        public override void OnEventActivate()
        {
            //CalculateDistance();
            _currentWaypoint++;
            foreach (var spirit in linkedObjects)
            {
                var agent = spirit.GetComponent<NavMeshAgent>();
                agent.SetDestination(waypoints[_currentWaypoint]);
                //spirit.transform.DOPath(waypoints, _totalDistance*(1/speed), PathType.CatmullRom);
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
