using Dialogue;
using UnityEngine;

public class DialogueMock : MonoBehaviour {
    public DialogueRoot mockDialogue;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DialogueSystem.StartDialogue(mockDialogue);
        }
    }
}
