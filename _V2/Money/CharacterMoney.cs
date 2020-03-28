using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.V2.UI.Utils.Interfaces;

namespace RPG.V2.Money {

    /// Controls the current amount of money a character holds
    /// And is responsible for persisting the amount using the save system
    public class CharacterMoney : MonoBehaviour, ISaveable, IDisplayVariable
    {
        [Header("Amount")]
        [Tooltip("The current amount this character starts with")]
        public int initialAmount = 0;

        /// Current money the character holds
        int currentAmount = 0;

        // Getters
        public int GetCurrentAmount()
        {
            return initialAmount;
        }

        // Modifiers
        public void IncreaseAmount(int amountToReceive)
        {
            currentAmount += Mathf.Abs(amountToReceive);
        }
        public void DecreaseAmount(int amountToDecrease)
        {
            int newAmount = currentAmount - amountToDecrease;

            // If new amount is negative, clamp it to zero
            currentAmount = newAmount <= 0 ? 0 : newAmount;
        }

        /// UI
        public string GetVariable()
        {
            return GetCurrentAmount().ToString();   
        }

        /// Saving
        public object CaptureState()
        {
            return currentAmount;
        }
        public void RestoreState(object state)
        {
            currentAmount = (int)state;
        }
        public void OnCleanState() {
            currentAmount = initialAmount;   
        }
    }
}
