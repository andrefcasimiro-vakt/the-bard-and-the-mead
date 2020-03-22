using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Quest {
    public class QuestManager : MonoBehaviour
    {
        // Ui parent
        public GameObject questUI;

        // Quest Header
        public GameObject ButtonActiveFilter;
        public GameObject ButtonDoneFilter;

        // Quest List
        public GameObject QuestListPanel;
        public GameObject QuestListButtonPrefab;

        // Quest Details
        public GameObject QuestDetailsPanel;
        public GameObject QuestTitle;
        public GameObject QuestLocation;
        public GameObject QuestDescription;
        public GameObject QuestObjectivesPanel;

        public GameObject QuestObjectivePrefab;

        [Header("Quest Keyboard Settings")]
        [SerializeField]
        public KeyCode questKey;

        CharacterQuests characterQuests;

        List<ScriptableQuest> currentQuests = new List<ScriptableQuest>();

        // Current quest
        RPG.Quest.Quest selectedQuest;

        GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            questUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(questKey))
            {

                // By default, open the player inventory
                if (!questUI.activeSelf)
                {
                    Open(player.GetComponent<CharacterQuests>());
                }

                questUI.SetActive(!questUI.activeSelf);

                HandleSystem();
            }
        }


        void HandleSystem()
        {
            player.SetActive(!questUI.activeSelf);

            if (questUI.activeSelf)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // Quest GUI
        public void Open(CharacterQuests characterQuests)
        {
            this.characterQuests = characterQuests;

            Draw();
        }

        public void Draw()
        {
            // On opening panel, disable text for right side panel.
            QuestDetailsPanel.SetActive(false);

            // Clean quest list panel first
            foreach (Transform child in QuestListPanel.transform)
            {
                Destroy(child.gameObject);
            }

            List<RPG.Quest.Quest> currentQuests = characterQuests?.currentQuests;

            if (currentQuests == null)
            {
                return;
            }

            // Clean quest list panel first
            foreach (RPG.Quest.Quest quest in currentQuests)
            {
                GameObject questButton = Instantiate(QuestListButtonPrefab);
                questButton.transform.SetParent(QuestListPanel.transform);

                Button b = questButton.GetComponent<Button>();
                b.transform.GetChild(0).GetComponent<Text>().text = quest.questTitle;

                b.onClick.RemoveAllListeners();

                b.onClick.AddListener(() =>
                {
                    SetActiveQuest(quest);
                });
            }

            // Is there any active quest?
            if (selectedQuest != null)
            {
                DrawActiveQuest();
            }
            QuestDetailsPanel.SetActive(selectedQuest != null);

        }

        void DrawActiveQuest()
        {
            QuestTitle.GetComponent<Text>().text = selectedQuest.questTitle;
            QuestLocation.GetComponent<Text>().text = selectedQuest.questLocation;
            QuestDescription.GetComponent<Text>().text = selectedQuest.questDescription;

            // Clean quest list panel first
            foreach (Transform child in QuestObjectivesPanel.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestObjective questObjective in selectedQuest.objectives)
            {
                GameObject questObjGO = Instantiate(QuestObjectivePrefab, QuestObjectivesPanel.transform);
                Text questObjectiveText = questObjGO.transform.GetChild(0).GetComponent<Text>();

                if (questObjectiveText == null)
                {
                    Debug.LogError("Could not find quest objective text in child of quest objective button instance");
                    return;
                }

                questObjectiveText.text = questObjective.questObjectiveDescription;

                if (questObjective.isDone)
                {
                    questObjectiveText.color = Color.green;
                }
            }

        }

        void SetActiveQuest (RPG.Quest.Quest quest)
        {
            selectedQuest = quest;

            // Redraw UI
            Draw();
        }

    }
}
