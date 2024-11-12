using System.Collections;
using Dialogue.Nodes;
using Dialogue.Nodes.Enums;
using Dialogue.UI;
using UnityEngine;

namespace Dialogue {
    public class DialogueSystem : MonoBehaviour {
        private static DialogueSystem Instance;
        private static bool Busy;

        public DialogueUIController uiController;

        private bool _inDialogue;
        private bool _readNextNode;
        private bool _hasNextBranch;
        private int _currentNodeIndex;
        private PauseReason _pauseReason;
        private CommandDependencies _commandDependencies;
        private DialogueBranch _currentBranch;
        private DialogueBranch _nextBranch;

        public static void StartDialogue(DialogueRoot dialogue) {
            if (!Instance || Busy) return;
            
            Instance.StartDialogueBranch(dialogue);
        }
        
        private void Awake() {
            Instance = this;
            
            _commandDependencies = new CommandDependencies(
                uiController,
                SetPendingNextBranch,
                PauseDialogue,
                UnpauseDialogue
            );
            
            uiController.ReadNextCallback = _commandDependencies.unPauseDialogueCallback;
        }

        private void OnDestroy() {
            Busy = false;
        }

        private void StartDialogueBranch(DialogueBranch branch) {
            uiController.OpenDialogueUI();
            
            _currentBranch = branch;
            _currentNodeIndex = 0;
            _readNextNode = true;
            
            StartCoroutine(RunDialogue());
        }

        private IEnumerator RunDialogue() {
            Busy = true;
            _inDialogue = true;
            
            while (_inDialogue) {
                if (!_readNextNode) {
                    yield return null;
                    continue;
                }
                
                if (_hasNextBranch) SwitchBranch(_nextBranch);
                
                if (_currentNodeIndex >= _currentBranch.Length) break;

                DialogueNode currentNode = _currentBranch[_currentNodeIndex];
                
                currentNode.Invoke(_commandDependencies);

                _currentNodeIndex++;
                yield return null;
            }
            
            ExitDialogue();
        }

        private void UnpauseDialogue(PauseReason pauseReason) {
            if (_pauseReason == pauseReason) {
                _readNextNode = true;
            }
        }

        private void SetPendingNextBranch(DialogueBranch nextBranch) {
            if (nextBranch == null) {
                _inDialogue = false;
            }
            else {
                _nextBranch = nextBranch;
                _hasNextBranch = true;
                _readNextNode = true;
            }
        }

        private void SwitchBranch(DialogueBranch toBranch) {
            _currentBranch = toBranch;
            _hasNextBranch = false;
            _currentNodeIndex = 0;

            _readNextNode = true;
        }

        private void ExitDialogue() {
            uiController.CloseDialogueUI();
            _currentBranch = null;
            Busy = false;
        }
        
        private void PauseDialogue(PauseReason pauseReason) {
            _readNextNode = false;
            _pauseReason = pauseReason;
        }
    }
}
