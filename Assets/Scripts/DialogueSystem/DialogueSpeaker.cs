using UnityEngine;

public class DialogueSpeaker : MonoBehaviour
{
    //main taks: ngedaftarin bubble setiap orang lewat tags ink #speaker
    [SerializeField] private string speakerId; //harus sama persis yg ditulis di Ink
    [SerializeField] private SpeechBubbleUI bubble;

    public string SpeakerId => speakerId;
    public SpeechBubbleUI Bubble => bubble;

    public void BindDialogue(DialogueManager tar)
    {
        tar.RegSpeaker(this);
    }
}
