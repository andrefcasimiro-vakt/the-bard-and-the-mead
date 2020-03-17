using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.EventSystem { 
    public class CloneGameObject : MonoBehaviour
    {
        public GameObject gameObjectToClone;
        public GameObject target;
        
        GameObject instance;

        public void Dispatch()
        {
            instance = Instantiate(gameObjectToClone, target.transform);
        }

        public void Destroy()
        {
            Destroy(instance.gameObject);
        }
    }
}
