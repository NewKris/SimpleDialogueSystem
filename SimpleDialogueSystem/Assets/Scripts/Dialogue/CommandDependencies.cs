using System;
using Dialogue.Nodes.Enums;
using Dialogue.UI;

namespace Dialogue {
    public readonly struct CommandDependencies {
        public readonly DialogueUIController uiController;
        public readonly Action<DialogueBranch> switchBranchCallback;
        public readonly Action<PauseReason> pauseDialogueCallback;
        public readonly Action<PauseReason> unPauseDialogueCallback;
        
        public CommandDependencies(
            DialogueUIController uiController,
            Action<DialogueBranch> switchBranchCallback,
            Action<PauseReason> pauseDialogueCallback,
            Action<PauseReason> unPauseDialogueCallback 
        ) {
            this.uiController = uiController;
            this.switchBranchCallback = switchBranchCallback;
            this.pauseDialogueCallback = pauseDialogueCallback;
            this.unPauseDialogueCallback = unPauseDialogueCallback;
        }
    }
}