using System;
using System.Linq;
using Dialogue.Nodes.Attributes;
using Dialogue.Nodes.Enums;
using Dialogue.UI;

namespace Dialogue.Nodes {
    [Serializable]
    public struct Choice {
        public string text;
        public DialogueBranch branch;
    }
    
    [NodeInfo("Choice", "Branching")]
    [Serializable]
    public class ChoiceNode : DialogueNode {
        public Choice[] choices;
        
        public override void Invoke(CommandDependencies dependencies) {
            dependencies.pauseDialogueCallback(PauseReason.CHOICE);
            
            OptionConfig[] optionConfigs = choices.Select(choice => new OptionConfig() {
                optionText = choice.text,
                toBranch = choice.branch
            }).ToArray();
            
            dependencies.uiController.ShowOptions(optionConfigs,config => {
                dependencies.uiController.HideButtons();
                dependencies.switchBranchCallback.Invoke(config.toBranch);
            });
        }
        
        protected override string CreateSummary() {
            return choices == null || choices.Length == 0
                ? "No choices provided!" 
                : choices.Aggregate("", (current, choice) => current + (choice.text + " | "));
        }
    }
}
