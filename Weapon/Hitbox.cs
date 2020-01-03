using UnityEngine;
using UnityEngine.Events;
using RPG.Core;

namespace RPG.Weapon {

    public class Hitbox: MonoBehaviour {

        // UnityEvent
        public UnityEvent OnHit;

        // Current hit target
        public GameObject target;

        MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
        Collider boxCollider => GetComponent<Collider>();

        void Start()
        {
            Disable();
        }

        public void Enable() 
        {
            boxCollider.enabled = true;
            meshRenderer.enabled = true;
        }

        public void Disable() 
        {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }

        void OnTriggerEnter(Collider col)
        {
            Health targetHealth = col.gameObject.GetComponent<Health>();

            if (targetHealth != null)
            {
                target = col.gameObject;
                OnHit.Invoke();
            }
        }

    }

}
