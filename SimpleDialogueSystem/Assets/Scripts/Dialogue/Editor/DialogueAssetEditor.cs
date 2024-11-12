using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dialogue.Nodes;
using Dialogue.Nodes.Attributes;
using UnityEditor;
using UnityEngine;

namespace Dialogue.Editor {
    [CustomEditor(typeof(DialogueBranch), true)]
    public class DialogueAssetEditor : UnityEditor.Editor {
        private static Dictionary<string, bool> FoldoutOpenStatuses;
        private static Dictionary<string, List<Type>> NodeGroups;

        private void OnEnable() {
            Type[] nodeTypes = Assembly.GetAssembly(typeof(DialogueNode)).GetTypes().Where(type => type.IsSubclassOf(typeof(DialogueNode))).ToArray();

            NodeGroups = new Dictionary<string, List<Type>>();
            FoldoutOpenStatuses = new Dictionary<string, bool>();
            
            foreach (Type nodeType in nodeTypes) {
                NodeInfo nodeInfo = GetNodeInfo(nodeType);
                if (nodeInfo == null) continue;

                if (NodeGroups.ContainsKey(nodeInfo.nodeGroup)) {
                    NodeGroups[nodeInfo.nodeGroup].Add(nodeType);
                } else {
                    NodeGroups.Add(nodeInfo.nodeGroup, new List<Type>() { nodeType} );
                    FoldoutOpenStatuses.Add(nodeInfo.nodeGroup, false);
                }
            }
        }

        public override void OnInspectorGUI() {
            foreach (KeyValuePair<string, List<Type>> group in NodeGroups) {
                FoldoutOpenStatuses[group.Key] = EditorGUILayout.Foldout(FoldoutOpenStatuses[group.Key], group.Key);
                if (!FoldoutOpenStatuses[group.Key]) continue;
                
                foreach (Type nodeType in group.Value) {
                    NodeInfo nodeInfo = GetNodeInfo(nodeType);
                    string buttonText = nodeInfo != null ? nodeInfo.nodeName : nodeType.ToString();
                
                    if (GUILayout.Button(buttonText)) 
                        CreateNode(nodeType);
                }
            }
            
            base.OnInspectorGUI();
        }

        private void CreateNode(Type dialogueNodeType) {
            DialogueNode newNode = Activator.CreateInstance(dialogueNodeType) as DialogueNode;
            (target as DialogueBranch)?.AddNode(newNode);
            EditorUtility.SetDirty(target);
        }

        private NodeInfo GetNodeInfo(Type nodeType) {
            return nodeType.GetCustomAttributes(typeof(NodeInfo)).Cast<NodeInfo>().FirstOrDefault();
        }
    }
}
