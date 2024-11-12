using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.UI {
    [RequireComponent(typeof(Button))]
    public class OptionButton : MonoBehaviour {
        public TextMeshProUGUI text;
        public TextMeshProUGUI index;

        private int _index;
        private KeyCode _key;
        private Button _button;
        
        public void Initialize(
            int buttonIndex, 
            string optionText,
            Action onClickCallback
        ) {
            index.text = (buttonIndex + 1).ToString();
            text.text = optionText;
            _key = IndexToKeyCode(buttonIndex);

            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => onClickCallback());

            gameObject.SetActive(true);
        }

        public void Clean() {
            GetComponent<Button>().onClick.RemoveAllListeners();
            text.text = "";
        }

        private void Update() {
            if (Input.GetKeyDown(_key) && _button.interactable) {
                _button.onClick.Invoke();
            }
        }

        private static KeyCode IndexToKeyCode(int i) {
            return i switch {
                0 => KeyCode.Alpha1,
                1 => KeyCode.Alpha2,
                2 => KeyCode.Alpha3,
                3 => KeyCode.Alpha4,
                4 => KeyCode.Alpha5,
                5 => KeyCode.Alpha6,
                _ => throw new NotImplementedException()
            };
        }
    }
}
