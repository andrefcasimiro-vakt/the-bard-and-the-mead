using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Quest;

namespace RPG.Switch {

    public class LocalSwitchIsOn : ICondition {
        
        public LocalSwitch localSwitch;

        public override bool Check() {
            return localSwitch.GetLocalSwitch();
        }
    
    }
}
