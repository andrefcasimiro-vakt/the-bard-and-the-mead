using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.EventSystem { 
    public class DisplayMessage : Template
    {
        [Header("---")]
        [Header("Displays a text message using the Dialogue GUI")]

        public string actorName;
        public string[] paragraphs;

        [Header("Settings")]
        public float letterPause = 3f;
        [Tooltip("A character to pause the text for 1 second.")]
        public string PauseCharacter = "@";

        [Header("UI Dependencies")]
        public GameObject DisplayTextUI_Prefab;

        private void Start()
        {
            letterPause = (float)(letterPause * 0.01);
        }

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(DisplayText());
        }

        IEnumerator DisplayText()
        {
            GameObject DisplayTextUIInstance = Instantiate(DisplayTextUI_Prefab);

            Text textComponent = DisplayTextUIInstance.GetComponentInChildren<Text>();

            // Add actor name
            if (!(string.IsNullOrEmpty(actorName))) { 
                textComponent.text = actorName + ":";
                textComponent.text += "\n";
            }

            foreach (string paragraph in paragraphs)
            {
                foreach (char letter in paragraph.ToCharArray())
                {
                    // Delay Case
                    if (letter.ToString() == PauseCharacter)
                    {
                        yield return new WaitForSeconds(1);
                    }
                    else
                    {
                        textComponent.text += letter;
                        yield return new WaitForSeconds(letterPause);
                    }
                }

                textComponent.text += "\n";
            }

            // Arrow Logic
            GameObject arrow = DisplayTextUIInstance.transform.GetChild(0).Find("Arrow").gameObject;

            if (arrow != null)
                arrow.GetComponent<Image>().enabled = true;

            // Wait for user input to continue
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            // Play exit animation for UI
            DisplayTextUIInstance.GetComponentInChildren<Animator>().SetTrigger("Exit");

            yield return new WaitForSeconds(1);

            // Destroy UI
            Destroy(DisplayTextUIInstance);

            // End event
            yield return null;
        }

    }
}
