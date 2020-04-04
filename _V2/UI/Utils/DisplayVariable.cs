using UnityEngine;
using UnityEngine.UI;
using RPG.V2.UI.Utils.Interfaces;

namespace RPG.V2.UI.Utils {

    /// Displays a monobehaviour variable on a UI text component
    /// E.g. Display current amount of money to the screen

    /// Monobehaviour where the target variable lives must implement IDisplayVariable interface
    /// Which exposes a GetVariable method so we can obtain the target variable that will be displayed
    /// to the screen.

    public class DisplayVariable : MonoBehaviour
    {
        [Header("UI Elements")]
        [Tooltip("Ui element to display the variable")] public Text textUI;
        [Tooltip("Text that precedes the variable")] public string textPrefix = "";
        [Tooltip("Text that succedes variable")] public string textSuffix = "";

        [Header("Variable owner")]
        public IDisplayVariable variableOwner;

        public void Update()
        {
            if (textUI != null && variableOwner != null)
            {
                Display();
            }
            else
            {
                Debug.LogError("Incorrect setup of variables for Display Variable instance.");
            }
        }

        public void Display()
        {
            textUI.text = textPrefix + variableOwner.GetVariable() + textSuffix;
        }

    }
}
