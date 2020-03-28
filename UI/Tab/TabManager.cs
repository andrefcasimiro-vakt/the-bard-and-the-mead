using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

namespace RPG.UI.Tabs {
    public class TabManager : MonoBehaviour {

        [Header("Tabs")]
        public List<Tab> tabs = new List<Tab>();

        public int defaultTabIndex = 0;

        int currentTabIndex = 0;

        void Start()
        {
            UpdateTab();
        }

        void UpdateTab()
        {
            // Deactivate all tabs
            tabs.ForEach(tab => {
                // Deactivate tab content
                tab.tabContent.SetActive(false);

                // Turn TAB button disabled color
                var disabledColors = tab.GetComponent<Button>().colors;
                disabledColors.normalColor = tab.disabledColor;
                disabledColors.highlightedColor = tab.disabledColor;
                disabledColors.selectedColor = tab.disabledColor;
                disabledColors.pressedColor = tab.disabledColor;
                disabledColors.disabledColor = tab.disabledColor;
                tab.GetComponent<Button>().colors = disabledColors;
            });

            // Activate the current tab
            Tab activeTab = tabs[currentTabIndex];
            activeTab.tabContent.SetActive(true);
            
            // Turn TAB button enabled
            var enabledColors = activeTab.GetComponent<Button>().colors;
            enabledColors.normalColor = activeTab.activeColor;
            enabledColors.highlightedColor = activeTab.activeColor;
            enabledColors.selectedColor = activeTab.activeColor;
            enabledColors.pressedColor = activeTab.activeColor;
            enabledColors.disabledColor = activeTab.activeColor;
            activeTab.GetComponent<Button>().colors = enabledColors;
        }

        public void UpdateIndex(Tab clickedTab)
        {
            int index = tabs.FindIndex(tab => tab == clickedTab);

            if (index == -1)
                return;

            currentTabIndex = index;

            UpdateTab();
        }
    }
}
