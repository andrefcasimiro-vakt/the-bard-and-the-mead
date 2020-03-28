using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RPG.EventSystem
{
    public enum FadeType {
        IN_AND_OUT,
        IN,
        OUT
    }

    public class FlashScreen : Template
    {
        [Header("---")]
        [Header("Flashes a screen during the given time. Optionally plays Unity Events in the middle.")]

        public GameObject flashPrefab;

        CanvasGroup canvasGroup;

        public float flashSpeed = 0.5f;
        public float flashDuration = 0.1f;

        public UnityEvent events;

        public FadeType fadeType = FadeType.IN_AND_OUT;

        public override IEnumerator Dispatch() {

            GameObject cachedInstance = Instantiate(flashPrefab);
            canvasGroup = cachedInstance.GetComponent<CanvasGroup>();

            if (canvasGroup == null) {
                Debug.Log("No canvas group found on your Flash screen prefab!");
                yield return null;
            }

            if (fadeType == FadeType.IN)
            {
                yield return StartCoroutine(FadeIn());
                events.Invoke();

                yield return null;
            }

            if (fadeType == FadeType.OUT)
            {
                yield return StartCoroutine(FadeOut());
                events.Invoke();

                yield return null;
            }

            yield return StartCoroutine(FadeIn());

            events.Invoke();

            yield return StartCoroutine(FadeOut());

            GameObject.Destroy(cachedInstance);
        }

        public IEnumerator FadeIn()
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / flashSpeed;
                yield return null; // Yield one frame;
            }

            yield return new WaitForSeconds(flashDuration);
        }

        public IEnumerator FadeOut()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / flashSpeed;
                yield return null; // Yield one frame;
            }
            yield return null; // Yield one frame;
        }
    }
}
