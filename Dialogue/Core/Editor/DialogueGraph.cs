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
        private DialogueGraphView _graphView;

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

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap
            {
                anchored = true,
            };

            miniMap.SetPosition(new Rect(10, 30, 200, 140));
            _graphView.Add(miniMap);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            // Add File Name Text Field
            var fileNameTextField = new TextField("File Name: ");
            fileNameTextField.SetValueWithoutNotify("New Narrative");
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() =>
            {
                RequestDataOperation(true);
            })
            { text = "Save Data" });
            toolbar.Add(new Button(() =>
            {
                RequestDataOperation(false);
            })
            { text = "Load Data" });

            // Create Node Button
            var nodeCreateButton = new Button(() =>
            {
                _graphView.CreateNode("Dialogue Node");
            });

            nodeCreateButton.text = "Create Node";
            toolbar.Add(nodeCreateButton);
            rootVisualElement.Add(toolbar);
        }

        private void ConstructGraphView()
        {
            _graphView = new DialogueGraphView
            {
                name = "Dialogue Graph"
            };

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }


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