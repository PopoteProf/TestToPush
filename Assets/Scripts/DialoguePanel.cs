using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour {
    public TMP_Text _txtDialogue;
    public Transform _panel;

    public void DisplayText(string text) {
        _txtDialogue.text = text;
        _panel.gameObject.SetActive(true);
    }

    public void ClosePanel() {
        _panel.gameObject.SetActive(false);
    }
}