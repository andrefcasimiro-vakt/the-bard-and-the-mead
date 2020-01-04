using UnityEngine;
using System.Collections;
using RPG.Core;

namespace RPG.AI
{
    public class AIHudManager : MonoBehaviour
    {

        [SerializeField] Health ownerHealth;
        [SerializeField] GameObject objectToActivate;

        // We set it here so the gizmo doesn't get in the way in the scene mode
        [SerializeField] float colliderRadius;

        bool showHUD = false;

        private void Start()
        {
            this.GetComponent<SphereCollider>().radius = colliderRadius;
        }

        private void Update()
        {
            if (ownerHealth.IsDead())
            {
                objectToActivate.SetActive(false);

                if (showHUD)
                {
                    StartCoroutine(DisableHUD());
                }
            }

            // Update health
            objectToActivate.SetActive(showHUD);
        }

        IEnumerator DisableHUD()
        {
            yield return new WaitForSeconds(3f);
            showHUD = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && !ownerHealth.IsDead())
            {
                showHUD = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player" && !ownerHealth.IsDead())
            {
                showHUD = false;
            }
        }
    }
}
