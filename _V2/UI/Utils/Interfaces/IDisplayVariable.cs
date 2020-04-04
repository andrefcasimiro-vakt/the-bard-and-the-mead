using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.V2.UI.Utils.Interfaces {

    /// Displays a variable on a text component
    [System.Serializable]
    public class IDisplayVariable : MonoBehaviour
    {
        public virtual string GetVariable() {
            return "";
        }
    }
}
