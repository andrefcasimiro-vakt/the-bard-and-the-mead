using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.V2.Utils {

    public static class RaycastUtils
    {

        /// Returns a bool that informs us if we hit a gameobject
        public static bool IsCameraLookingAtObject(GameObject target, LayerMask layersToConsider)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, layersToConsider))
            {
                GameObject _gameObject = hit.transform.gameObject;

                if (_gameObject == null)
                {
                    return false;
                }

                if (_gameObject == target)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
