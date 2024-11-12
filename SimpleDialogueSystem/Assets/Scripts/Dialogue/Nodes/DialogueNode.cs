using System;
using System.Linq;
using Dialogue.Nodes.Attributes;
using Dialogue.Nodes.Enums;
using UnityEngine;

namespace Dialogue.Nodes {
    [Serializable]
    public abstract class DialogueNode {
        private const int MAX_SUMMARY_LENGTH = 50;
        
        [HideInInspector]
        public string nodeSummary;
        
        public void UpdateSummary() {
            nodeSummary = CreateSummary();
            
            if (nodeSummary.Length > MAX_SUMMARY_LENGTH) {
                nodeSummary = nodeSummary.Substring(0, MAX_SUMMARY_LENGTH) + "...";
            }
        }

        public abstract void Invoke(CommandDependencies dependencies);

        protected abstract string CreateSummary();
    }
}
