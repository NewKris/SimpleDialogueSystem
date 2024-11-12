using System;
using Dialogue.Nodes.Attributes;
using Dialogue.Nodes.Enums;
using UnityEngine;

namespace Dialogue.Nodes {
    [NodeInfo("Text")]
    [Serializable]
    public class TextNode : DialogueNode {
        public string speaker;
        [TextArea] public string text;
        
        public override void Invoke(CommandDependencies dependencies) {
            dependencies.pauseDialogueCallback(PauseReason.TEXT);
            dependencies.uiController.ShowText(text, speaker);
        }
        
        protected override string CreateSummary() {
            return string.IsNullOrEmpty(speaker) ? $"{text}" : $"{speaker}: {text}";
        }
    }
}
