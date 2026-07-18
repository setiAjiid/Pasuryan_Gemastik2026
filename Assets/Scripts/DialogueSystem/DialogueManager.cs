using UnityEngine;
using Ink.Runtime;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    //main task: start dialogue, typing, instant finish typing
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private float typeSpeed = 0.03f;
    [SerializeField] private BatinState batin;

    private Story story;
    private Coroutine typCoroutine;
    private SpeechBubbleUI activeBubble;
    private string currLine;
    private bool isTyping = false;
    private DialogueTrigger nearbyTrigger; //npc terdekat yg bakalan diisi sama regTrigger

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

    public void RegTrigger(DialogueTrigger trigger)
    {
        nearbyTrigger = trigger;
    }

    public void UnregTrigger(DialogueTrigger trigger)
    {
        if (trigger == nearbyTrigger) nearbyTrigger = null;
    }

    private void HandleInteract()
    {
        if (!IsDialogueActive)
        {
            if (nearbyTrigger != null) SetupDialogue(nearbyTrigger.Bubble, nearbyTrigger.Text);
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
    private void SetupDialogue(SpeechBubbleUI bubble, TextAsset inkJson)
    {
        story = new Story(inkJson.text);
        IsDialogueActive = true;
        activeBubble = bubble;
        input.DisableMove();
        DisplayLine();
    }

    private void DisplayLine()
    {
        while (story.canContinue) //why while? not if? cz buat handle klo isinya "line" cuman tags doang, tanpa teks
        {
            string text = story.Continue().Trim();
            DialogueLine line = TagParser.Parse(text, story.currentTags); //jadiin line bersih, textnya sendiri, tagsnya sendiri
            ApplyTags(line); //apply efek samping
            
            if (!line.IsTagOnly)
            {
                currLine = line.Text;
                typCoroutine = StartCoroutine(Typing(currLine));
                return;
            } 
        }
        EndDialogue();
    }

    private void ApplyTags(DialogueLine line)
    {
        if (line.HasResource)
        {
            batin.CalcStat(line.MHDelta, line.EnergyDelta, line.TensiDelta);
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
