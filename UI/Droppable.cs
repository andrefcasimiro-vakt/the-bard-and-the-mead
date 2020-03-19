using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

namespace RPG.UI {
    public class Droppable : MonoBehaviour, IDropHandler {

        // In the future change to enum because we can have 2 cases: remove item or swap places
        public bool removeItem = true;

        public void OnDrop(PointerEventData ped) 
        {
            
            ItemData itemData = ped.pointerDrag.gameObject.GetComponent<ItemData>();

            if (itemData != null)
            {
                if (removeItem)
                {
                    itemData.RemoveItem();
                }
            }
            
        }

    }
}
