using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace RPG.EventSystem { 
    public class GoToScene : MonoBehaviour
    {
        [Header("---")]
        [Header("Generic event to interact with monobehaviour components")]

        public int sceneId;

        public void Execute() {
            SceneManager.LoadSceneAsync(sceneId);
        }

    }
}
