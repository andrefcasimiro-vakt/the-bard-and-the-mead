using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils { 
    public class UnrenderOnStart : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }

    }
}
