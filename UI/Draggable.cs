using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

namespace RPG.UI {
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {


        public Vector2 originalPos;

        void Start()
        {
            originalPos = this.transform.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;

        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;
        }


    }
}
