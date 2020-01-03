using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RPG.Movement {
    [System.Serializable]
    public class Ground
    {
        public string key;
        public AudioClip[] clips;
        public LayerMask layer;
    }

    public class FootstepManager : MonoBehaviour
    {

        [SerializeField]
        public List<Ground> grounds = new List<Ground>(); 


        public Ground GetGround(int layerIndex)
        {
            return grounds.Find(ground => Contains(ground.layer, layerIndex));
        }

        public bool Contains(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

    }
}
