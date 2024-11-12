using System;
using System.Collections;
using Dialogue.Nodes.Enums;
using TMPro;
using UnityEngine;

namespace Dialogue.UI {
    public class DialogueUIController : MonoBehaviour {
        [Header("Typewriter")] 
        public float timePerCharacter;
        
        [Header("Text")]
        public GameObject textBox;
        public TextMeshProUGUI dialogueText;

        [Header("Name")]
        public GameObject nameBox;
        public TextMeshProUGUI speakerName;

        [Header("Other")] 
        public OptionButton[] optionButtons;

        private bool _finishedTyping;
        
        public Action<PauseReason> ReadNextCallback { get; set; }
        private int VisibleDialogueCharacters {
            get => dialogueText.maxVisibleCharacters;
            set => dialogueText.maxVisibleCharacters = value;
        }

        private int TotalDialogueCharacters => dialogueText.textInfo.characterCount;
        
        public void OpenDialogueUI() {
            ResetUI();
            gameObject.SetActive(true);
        }

        public void CloseDialogueUI() {
            ResetUI();
            gameObject.SetActive(false);
        }
        
        public void ShowText(string text, string speaker = "") {
            _finishedTyping = false;
            
            dialogueText.SetText(text);
            
            nameBox.SetActive(!string.IsNullOrEmpty(speaker));
            speakerName.SetText(speaker);

            StartCoroutine(TypeText());
        }

        public void ShowOptions(OptionConfig[] options, Action<OptionConfig> onOptionPicked) {
            for (int i = 0; i < options.Length; i++) {
                OptionConfig config = options[i];
                
                optionButtons[i].Initialize(
                    i,
                    config.optionText,
                    () => onOptionPicked(config)
                );
            }
        }
        
        public void HideButtons() {
            foreach (OptionButton option in optionButtons) {
                option.Clean();
                option.gameObject.SetActive(false);
            }
        }

        public void TryReadNext() {
            if (!_finishedTyping) {
                ForceFinishText();
            }
            else {
                ReadNextCallback?.Invoke(PauseReason.TEXT);
            }
        }

        private void ForceFinishText() {
            _finishedTyping = true;
            VisibleDialogueCharacters = TotalDialogueCharacters;
            StopCoroutine(TypeText());
        }

        private IEnumerator TypeText() {
            float t = 0;
            dialogueText.maxVisibleCharacters = 0;
            
            while (VisibleDialogueCharacters == 0 || VisibleDialogueCharacters < TotalDialogueCharacters) {
                t += Time.deltaTime;

                if (t >= timePerCharacter) {
                    t = 0;
                    VisibleDialogueCharacters++;
                }
                
                yield return null;
            }
            
            _finishedTyping = true;
        }
        
        private void ResetUI() {
            textBox.SetActive(true);
            dialogueText.text = "";
            
            nameBox.SetActive(false);
            speakerName.text = "";

            HideButtons();
        }
    }
}
