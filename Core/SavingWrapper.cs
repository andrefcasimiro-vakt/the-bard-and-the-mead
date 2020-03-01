using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
namespace RPG.Core
{

    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeInTime = 0.5f;

        const string defaultSaveFile = "save";
        SavingSystem savingSystem;
        Fader fader;


        IEnumerator Start()
        {

           savingSystem = GetComponent<SavingSystem>();
           fader = FindObjectOfType<Fader>();

            // Start any scene with black screen
            fader.FadeOutImmediate();

            yield return savingSystem.LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadMethod();
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveMethod();
            }
        }

        public void SaveMethod()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void LoadMethod()
        {
            savingSystem.Load(defaultSaveFile);
        }
    }

}
