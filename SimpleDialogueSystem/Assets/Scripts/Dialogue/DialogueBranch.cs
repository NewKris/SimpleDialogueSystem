using System.Collections.Generic;
using Dialogue.Nodes;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(menuName = "Dialogue/Dialogue Branch")]
    public class DialogueBranch : ScriptableObject {
        [SerializeReference]
        public List<DialogueNode> dialogueNodes;

        public int Length => dialogueNodes.Count;
        public DialogueNode this[int i] => dialogueNodes[i];
        
        public void AddNode(DialogueNode dialogueNode) {
            dialogueNodes ??= new List<DialogueNode>();
            
            dialogueNodes.Add(dialogueNode);
            dialogueNode.UpdateSummary();
        }

        private void OnValidate() {
            foreach (DialogueNode node in dialogueNodes) {
                node.UpdateSummary();
            }
        }
    }
}
