using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //main task: cek syarat pra-dialog sebelum ke dialogue manager
    //trigger saat apa? collision terjadi
    //npc pasif cuman ngasih data dan lapor jarak

    [SerializeField] private SpeechBubbleUI bubble;
    [SerializeField] private TextAsset text;
    [SerializeField] private DialogueManager dialogueManager;

    public SpeechBubbleUI Bubble => bubble;
    public TextAsset Text => text;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueManager.RegTrigger(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueManager.UnregTrigger(this);
        }
    }
}
