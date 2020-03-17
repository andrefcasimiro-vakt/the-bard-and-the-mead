using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Switch {

    public class ICondition : MonoBehaviour {
        
        public virtual bool Check() {
            return true;
        }
    
    }
}
