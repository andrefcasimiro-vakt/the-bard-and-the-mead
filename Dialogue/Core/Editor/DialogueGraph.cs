using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using RPG.Dialogue.Core;
using System;
using UnityEditor.Experimental.GraphView;

namespace RPG.Dialogue.Core {
    public class DialogueGraph : EditorWindow
    {
        // Resources
        private string searchModalIcon = "SearchModalIcon";

        private DialogueGraphView _graphView;

        private TextField _fileNameTextField;
        private string _fileName = "New Narrative";

        [MenuItem("Graph/Dialogue Graph")]
        public static void OpenDialogueGraphWindow()
        {
            var window = GetWindow<DialogueGraph>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();

        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        /// <summary>
        ///  Generates the toolbar with several editing functionalities.
        /// </summary>
        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            // ==> FILE SEARCH <=========================

            // Add File Name Text Field
            toolbar.Add(ConstructFileNameField());

            // Create Node Button
            var searchButton = new Button(() => HandleSearch())
            {
                text = "Search",
                name = "searchButton"
            };
            toolbar.Add(searchButton);



            // ==> DATA MANAGEMENT <=========================

            // Create Save Button
            toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });

            // Create Load Button
            toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });



            // ==> CREATE NEW NODE <=========================

            // Create Node Button
            var nodeCreateButton = new Button(() => _graphView.CreateNode("Dialogue Node"))
            {
                text = "Create Node",
                tooltip = "Creates a new dialogue node.",
                name = "createNode"
            };
            toolbar.Add(nodeCreateButton);


            // Add style to toolbar
            toolbar.styleSheets.Add(Resources.Load<StyleSheet>("Toolbar"));

            // Register toolbar
            rootVisualElement.Add(toolbar);
        }

        private void HandleSearch()
        {
            // Checks if any window of type MyWindow is open
            if (SearchWindow.HasOpenInstances<SearchWindow>())
            {
                var _window = SearchWindow.GetWindow(typeof(SearchWindow));
                _window.Close();
            }

            SearchWindow window = (SearchWindow)SearchWindow.GetWindow(typeof(SearchWindow));

            window.titleContent = new GUIContent("Search", (Texture2D)Resources.Load<Texture2D>(searchModalIcon));

            window.Show();

            AddFilesToSearch(window);
        }

        private void AddFilesToSearch(SearchWindow window)
        {
            DialogueContainer[] targetFiles = Resources.FindObjectsOfTypeAll<DialogueContainer>();

            List<string> fileNames = new List<string>();
            foreach(DialogueContainer t in targetFiles)
            {
                if (!fileNames.Contains(t.name))
                    fileNames.Add(t.name);
            }

            foreach(string f in fileNames)
            {
                var btn = new Button(() =>
                {
                    LoadDialogue(f);
                })
                {
                    text = f
                };

                window.rootVisualElement.Add(btn);
            }

        }

        void LoadDialogue(string fileName)
        {
            _fileName = fileName;
            _fileNameTextField.SetValueWithoutNotify(fileName);
            RequestDataOperation(false);
        }

        private TextField ConstructFileNameField()
        {
            _fileNameTextField = new TextField("");
            _fileNameTextField.SetValueWithoutNotify("");
            _fileNameTextField.MarkDirtyRepaint();
            _fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            _fileNameTextField.name = "fileName";

            return _fileNameTextField;
        }

        /// <summary>
        /// Generates a mini map on the top-left sideof the graph editor.
        /// </summary>
        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap
            {
                anchored = true,
            };

            miniMap.SetPosition(new Rect(10, 40, 200, 140));
            _graphView.Add(miniMap);
        }

        /// <summary>
        /// Instantiates a new instance of DialogueGraphView
        /// </summary>
        private void ConstructGraphView()
        {
            _graphView = new DialogueGraphView
            {
                name = "Dialogue Graph"
            };

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        /// <summary>
        /// Save / Load request operation.
        /// </summary>
        /// <param name="save">Bool to decide if saveUtility should call save or load method.</param>
        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid filename.", "OK");
                return;
            }

            var saveUtility = GraphSaveUtility.GetInstance(_graphView);

            if (save)
            {
                saveUtility.SaveGraph(_fileName);
                return;
            }

            saveUtility.LoadGraph(_fileName);
        }


    }
} 