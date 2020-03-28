using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

namespace RPG.UI.Tabs {
    public class Tab : MonoBehaviour {
        
        [Header("Tab Manager")]
        public TabManager tabManager;


        [Header("Tab Content")]
        public GameObject tabContent;

        [Header("Colors")]
        public Color activeColor;
        public Color disabledColor;

        public void Toggle()
        {
            tabManager.UpdateIndex(this);
        }
    }
}
