using UnityEngine;
using System.Collections;
using RPG.Core;

namespace RPG.Effect
{

    public class RestoreHealth
    {
        Health targetHealth;
        float amountToRestore;

        public RestoreHealth(Health targetHealth, float amountToRestore)
        {
            this.targetHealth = targetHealth;
            this.amountToRestore = amountToRestore;
        }

        public void Restore()
        {
            targetHealth.Restore(amountToRestore);
        }

    }


}
