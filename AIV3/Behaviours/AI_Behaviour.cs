using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;
using RPG.AI;
using RPG.Saving;
using System.Linq;

namespace RPG.AIV3 {

    public abstract class AI_Behaviour {

        public abstract void Dispatch();

    }

}
