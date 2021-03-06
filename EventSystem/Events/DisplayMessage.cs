﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Control;

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

        [Header("SFX")]
        public AudioClip typewriterSoundClip;
        public AudioClip audioClipOnInput;

        GameObject player;

        private void Start()
        {
            letterPause = (float)(letterPause * 0.01);
            
            player = GameObject.FindWithTag("Player");
        }

        public void Display()
        {
            StartCoroutine(Dispatch());
        }

        public override IEnumerator Dispatch()
        {
            if (player != null)
            {
                player.GetComponent<ComponentManager>().ToggleComponents(false);
            }

            yield return StartCoroutine(DisplayText());
        }

        IEnumerator DisplayText()
        {
            GameObject DisplayTextUIInstance = Instantiate(DisplayTextUI_Prefab);

            Text textComponent = DisplayTextUIInstance.GetComponentInChildren<Text>();

            AudioSource audioSource = this.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;

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
                        // If you want more pause, just add more pausecharacters: "Pause@@@@..."
                        yield return new WaitForSeconds(1);
                    }
                    else
                    {
                        textComponent.text += letter;

                        if (typewriterSoundClip != null) {
                            audioSource.PlayOneShot(typewriterSoundClip);
                        }
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
            yield return new WaitUntil(() => HasPressedKey());

            if (audioClipOnInput != null) {
                audioSource.clip = audioClipOnInput;

                yield return new WaitForSeconds(audioClipOnInput.length);
                audioSource.Play();
            }

            // Play exit animation for UI
            DisplayTextUIInstance.GetComponentInChildren<Animator>().SetTrigger("Exit");

            yield return new WaitForSeconds(.1f);

            // Destroy UI
            Destroy(DisplayTextUIInstance);
            Destroy(audioSource);

            if (player != null)
            {
                player.GetComponent<ComponentManager>().ToggleComponents(true);
            }
        }

        public bool HasPressedKey() {

            return (
                Input.GetButtonDown("Action")
                || Input.GetKeyDown(KeyCode.E)
                || Input.GetKeyDown(KeyCode.KeypadEnter)
                || Input.GetKeyDown(KeyCode.Return)
                || Input.GetKeyDown(KeyCode.Space)
                || Input.GetButtonDown("Fire1")
            );
        }

    }
}
