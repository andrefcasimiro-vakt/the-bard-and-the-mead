using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

namespace RPG.UI {
    public class OpenURL : MonoBehaviour {

        public string url;

        public void OpenUrl()
        {
            Application.OpenURL(url);
        }

    }
}
