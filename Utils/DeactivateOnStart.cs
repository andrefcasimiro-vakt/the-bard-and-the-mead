using UnityEngine;
using System.Collections;

namespace RPG.Utils
{
    public class DeactivateOnStart : MonoBehaviour
    {
        void Start()
        {
            this.gameObject.SetActive(false);
        }
    }
}
