using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.AI
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] const float waypointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 currentWaypoint = GetWaypoint(i);
                Vector3 nextWaypoint = GetWaypoint(GetNextIndex(i));

                Gizmos.DrawSphere(currentWaypoint, waypointGizmoRadius);
                Gizmos.DrawLine(currentWaypoint, nextWaypoint);
            }
        }

        public int GetNextIndex(int i)
        {
            int index = i + 1;
            return index == transform.childCount ? 0 : index;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
