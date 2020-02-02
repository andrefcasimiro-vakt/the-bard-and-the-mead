using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Dialogue.Core
{
    public class GraphSaveUtility
    {
        private DialogueGraphView _targetGraphicView;
        private DialogueContainer _containerCache;

        private List<Edge> Edges => _targetGraphicView.edges.ToList();
        private List<DialogueNode> Nodes => _targetGraphicView.nodes.ToList().Cast<DialogueNode>().ToList();

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
        {
            return new GraphSaveUtility
            {
                _targetGraphicView = targetGraphView
            };
        }

        public void SaveGraph(string fileName)
        {
            if (!Edges.Any()) return;

            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
            var connectedPorts = Edges.Where(x   => x.input.node != null).ToArray();

            for (var i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as DialogueNode;
                var inputNode = connectedPorts[i].input.node as DialogueNode;

                dialogueContainer.NodeLinks.Add(
                    new NodeLinkData
                    {
                        BaseNodeGuid = outputNode.GUID,
                        PortName = connectedPorts[i].output.portName,
                        TargetNodeGuid = inputNode.GUID
                    }
                );
            }

            foreach(var dialogueNode in Nodes.Where(node => !node.EntryPoint))
            {
                dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
                {
                    Guid = dialogueNode.GUID,
                    DialogueText = dialogueNode.DialogueText,
                    Position = dialogueNode.GetPosition().position
                });
            }

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateAsset(
                dialogueContainer,
                $"Assets/Resources/{fileName}.asset"
            );
            AssetDatabase.SaveAssets();
        }
        
        public void LoadGraph(string fileName)
        {
            _containerCache = Resources.Load<DialogueContainer>(fileName);

            if (_containerCache == null)
            {
                EditorUtility.DisplayDialog("File not found.", "Target dialogue graph file does not exist.", "OK");
                return;
            }

            ClearGraph();

            CreateNodes();

            ConnectNodes();
        }

        private void ClearGraph()
        {
            // Set entry points guid back from the save
            Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

            foreach (var node in Nodes)
            {
                if (node.EntryPoint) continue;

                Edges.Where(x => x.input.node == node).ToList()
                    .ForEach(edge => _targetGraphicView.RemoveElement(edge));

                _targetGraphicView.RemoveElement(node);
            }

        }

        private void CreateNodes()
        {
            foreach(var nodeData in _containerCache.DialogueNodeData)
            {
                var tempNode = _targetGraphicView.CreateDialogueNode(nodeData.DialogueText);
                tempNode.GUID = nodeData.Guid;
                _targetGraphicView.AddElement(tempNode);

                var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
                nodePorts.ForEach(x => _targetGraphicView.AddChoicePort(tempNode, x.PortName));
            }
        }

        private void ConnectNodes()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();

                for (var j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);

                    LinkNodes(
                        Nodes[i].outputContainer[j].Q<Port>(),
                        (Port)targetNode.inputContainer[0]
                    );

                    targetNode.SetPosition(
                        new Rect(
                            _containerCache.DialogueNodeData.First(x => x.Guid == targetNodeGuid).Position,
                            _targetGraphicView.defaultNodeSize
                        )
                    );
                } 
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge { output = output, input = input };

            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);

            _targetGraphicView.Add(tempEdge);

        }
    }
}
