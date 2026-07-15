using TMPro;
using UnityEngine;

public class SpeechBubbleUI : MonoBehaviour
{
    //show and hide the bubble wrapper, and set the text that send from DialogueManager
    [SerializeField] private GameObject bubble;
    [SerializeField] private TextMeshProUGUI dialogue;

    private void Awake()
    {
        Hide();
    }

    public void Show(string text)
    {
        bubble.SetActive(true);
        dialogue.text = text;
    }

    public void Hide()
    {
        bubble.SetActive(false);
    }
}
