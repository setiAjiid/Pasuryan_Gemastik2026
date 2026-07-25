using UnityEngine;
using Ink.Runtime;
using System.Collections;

public class DialogueManager : MonoBehaviour, IInteractionSession
{
    //main task: start dialogue, typing, instant finish typing
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private PlayerInteractor interactor;
    [SerializeField] private float typeSpeed = 0.03f;
    [SerializeField] private BatinState batin;

    private Story story;
    private Coroutine typCoroutine;
    private SpeechBubbleUI activeBubble;
    private string currLine;
    private bool isTyping = false;

    public bool IsDialogueActive { get; private set; }

    //dicall PlayerInteractor tiep E dipencet slama sesi ini aktif
    public void OnContinue()
    {
        if (isTyping) InstantFinishTyp();
        else DisplayLine();
    }

    //case baru mulai dialog, dicall dr DialogueTrigger.Interact()
    public void StartDialogue(SpeechBubbleUI bubble, TextAsset inkJson)
    {
        if (IsDialogueActive) return;
        story = new Story(inkJson.text);
        IsDialogueActive = true;
        activeBubble = bubble;
        input.DisableMove();
        interactor.BeginSession(this);
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
        interactor.EndSession(this);
        input.EnableMove();
    }

}
