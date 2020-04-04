using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.V2.UI.Utils.Interfaces;
using RPG.V2.UI;

namespace RPG.V2.Money {

    /// Controls the current amount of money a character holds
    /// And is responsible for persisting the amount using the save system
    public class CharacterMoney : IDisplayVariable, ISaveable
    {
        [Header("UI")]
        public NotificationManager notificationManager;

        [Header("Amount")]
        [Tooltip("The current amount this character starts with")]
        public int initialAmount = 0;

        /// Current money the character holds
        int currentAmount = 0;

        // Getters
        public int GetCurrentAmount()
        {
            return currentAmount;
        }

        // Modifiers
        public void IncreaseAmount(int amountToReceive)
        {
            currentAmount += Mathf.Abs(amountToReceive);

            NotifyPlayer();
        }
        public void DecreaseAmount(int amountToDecrease)
        {
            int newAmount = currentAmount - amountToDecrease;

            // If new amount is negative, clamp it to zero
            currentAmount = newAmount <= 0 ? 0 : newAmount;

            NotifyPlayer();
        }

        protected void NotifyPlayer()
        {
            if (notificationManager == null)
            {
                Debug.LogError("Incorrect setup for Character Money. Missing Notification Manager!");
                return;
            }

            notificationManager.Wake();
        }

        /// UI
        public override string GetVariable()
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
