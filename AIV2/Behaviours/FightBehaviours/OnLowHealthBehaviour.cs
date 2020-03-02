using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.AI;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;

namespace RPG.AIV2 {

    public class OnLowHealthBehaviour : Behaviour {

        public Behaviour customBehaviour = null;


        public override IEnumerator Dispatch() {

            StartCoroutine(customBehaviour.Dispatch());

            yield return null;
        }

    }
}
