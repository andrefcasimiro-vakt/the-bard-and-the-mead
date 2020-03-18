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
            Debug.Log ("END DRAG");
            Debug.Log ("DRAGGED ON " + eventData.pointerDrag.gameObject.name);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log ("BEGIN DRAG");


            this.transform.position = eventData.position;

        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log ("ON DRAG");
            this.transform.position = eventData.position;


        }


    }
}
