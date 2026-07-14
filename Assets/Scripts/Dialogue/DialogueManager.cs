using UnityEngine;
using Ink.Runtime;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    //main task: start dialogue, typing, instant finish typing
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private float typeSpeed = 0.03f;

    private Story story;
    private Coroutine typCoroutine;
    private SpeechBubbleUI activeBubble;
    private string currLine;
    private bool isTyping = false;


    public bool IsDialogueActive { get; private set; }

    //cek dialog cases in handleinteract (which is dialog ga active aka baru pertama kali, atau dialog sudah active dengan kondisi masih typing atau ngga typing)
    private void OnEnable()
    {
        input.OnInteractPressed += HandleInteract;
    }

    private void OnDisable()
    {
        input.OnInteractPressed -= HandleInteract;
    }

    private void HandleInteract()
    {
        if (!IsDialogueActive)
        {
            return;
        } else
        {
            if (isTyping)
            {
                InstantFinishTyp();
            } else
            {
                DisplayLine();
            }
        }
    }

    //case baru mulai dialog
    public void SetupDialogue(SpeechBubbleUI bubble, TextAsset inkJson)
    {
        story = new Story(inkJson.text);
        IsDialogueActive = true;
        activeBubble = bubble;
        input.DisableMove();
        DisplayLine();
    }

    private void DisplayLine()
    {
        if (story.canContinue)
        {
            currLine = story.Continue().Trim();
            typCoroutine = StartCoroutine(Typing(currLine));
        } else
        {
            EndDialogue();
        }
    }

    private IEnumerator Typing(string line)
    {
        isTyping = true;
        string shown = "";
        foreach (char c in line)
        {
            shown += c;
            activeBubble.Show(shown);
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    private void InstantFinishTyp()
    {
        if (typCoroutine != null) StopCoroutine(typCoroutine); 
        activeBubble.Show(currLine);
        isTyping = false;
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;
        activeBubble.Hide();
        activeBubble = null;
        input.EnableMove();
    }

}
