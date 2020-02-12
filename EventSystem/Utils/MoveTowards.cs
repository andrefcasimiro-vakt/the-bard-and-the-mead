using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem { 

    public class MoveTowards : MonoBehaviour
    {
        public Transform target;
        public Transform destination;

        public float minimumDistance = 1f;

        public float moveSpeed = 0.5f;

        bool move = false;

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(destination.position, target.position) <= minimumDistance)
            {
                move = false;
            }

            if (move)
            {
                target.position = Vector3.MoveTowards(target.position, destination.position, Time.deltaTime * moveSpeed);
            }
        }

        public void Move()
        {
            move = true;
        }

    }
}
