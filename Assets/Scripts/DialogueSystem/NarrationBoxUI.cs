using TMPro;
using UnityEngine;

public class NarrationBoxUI : MonoBehaviour, IDialogueView
{
    //main taks: kotak lebar di bawh UI, for narasi n monolog + namanya
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject nameBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogue;

    private void Awake()
    {
        Hide();
    }

    public void Show(string speaker, string text)
    {
        box.SetActive(true);
        dialogue.text = text;

        bool hasName = !string.IsNullOrEmpty(speaker);
        nameBox.SetActive(hasName);
        if (hasName) nameText.text = speaker;
    }

    public void Hide()
    {
        box.SetActive(false);
        nameBox.SetActive(false);
    }
}
