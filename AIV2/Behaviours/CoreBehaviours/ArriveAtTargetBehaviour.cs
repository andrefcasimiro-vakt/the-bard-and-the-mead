using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using RPG.Saving;
using RPG.AI;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;

namespace RPG.AIV2 {

    public class ArriveAtTargetBehaviour : Behaviour {


        public UnityEvent OnArrive;

        public override IEnumerator Dispatch() {

            OnArrive.Invoke();

            yield return null;
        }

    }
}
