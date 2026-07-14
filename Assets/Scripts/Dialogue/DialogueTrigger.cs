using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //main task: cek syarat pra-dialog sebelum ke dialogue manager
    //trigger saat apa? collision terjadi + ketika "E" ditekan (which is butuh input)
    //akibatnya, mesin memulai dialog (which is butuh bubble, text dialog, dan DialogueManager) dengan cara melakukan list method ke OnInteractPressed event

    [SerializeField] private PlayerInputReader input;
    [SerializeField] private SpeechBubbleUI bubble;
    [SerializeField] private TextAsset text;
    [SerializeField] private DialogueManager dialogueManager;

    private bool playerInRange = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OnEnable()
    {
        input.OnInteractPressed += TryStartDialogue;
    }

    private void OnDisable()
    {
        input.OnInteractPressed -= TryStartDialogue;
    }

    private void TryStartDialogue()
    {
        if (playerInRange && !dialogueManager.IsDialogueActive)
        {
            dialogueManager.SetupDialogue(bubble, text);
        }
    }



}
