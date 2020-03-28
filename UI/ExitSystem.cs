using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

namespace RPG.UI {
    public class ExitSystem : MonoBehaviour {

        public void Exit()
        {
            Application.Quit();
        }

    }
}
