using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using RPG.Saving;

namespace RPG.Core { 
    public class DeleteSaveGame : MonoBehaviour
    {
        [Header("---")]
        [Header("Generic event to interact with monobehaviour components")]

        public string saveGameName = "save";

        SavingSystem savingSystem => GetComponent<SavingSystem>();

        public void Execute() {
            savingSystem.SafeDelete(saveGameName);
        }

    }
}
