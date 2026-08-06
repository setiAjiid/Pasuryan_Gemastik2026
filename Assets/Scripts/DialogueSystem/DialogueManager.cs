using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour, IInteractionSession
{
    //main task: start dialogue, typing, instant finish typing, n milih kotak sesuai pembicara
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private PlayerInteractor interactor;
    [SerializeField] private float typeSpeed = 0.03f;
    [SerializeField] private BatinState batin;
    [SerializeField] private NarrationBoxUI narration;
    [SerializeField] private ChoiceSelectorUI choices;

    private readonly Dictionary<string, DialogueSpeaker> speakers = new();
    private readonly Dictionary<string, Story> stories = new(); //story hidup trus

    private Story story;
    private Coroutine typCoroutine;
    private IDialogueView activeView;
    private string activeSpeaker;
    private string currLine;
    private bool isTyping = false;
    private bool isChoosing = false;

    public bool IsDialogueActive { get; private set; }

    //dicall DialogueSpeaker lwat SceneContext tiap scene baru kebuka
    public void RegSpeaker(DialogueSpeaker tar)
    {
        if (string.IsNullOrEmpty(tar.SpeakerId))
        {
            Debug.LogWarning($"[{tar.name}] speakerId masi kosong", tar);
            return;
        }
        speakers[tar.SpeakerId.ToLower()] = tar; 
    }

    //dicall PlayerInteractor tiep E dipencet slama sesi ini aktif
    public void OnContinue()
    {
        if (isChoosing) ChooseCurrent();
        else if (isTyping) InstantFinishTyp();
        else DisplayLine();
    }

    public void OnNavigate(int dir)
    {
        if (!isChoosing) return;
        choices.MoveIndex(dir);
    }

    //case baru mulai dialog, dicall dr DialogueTrigger.Interact()
    public void StartDialogue(TextAsset inkJson, string entryKnot)
    {
        if (IsDialogueActive) return;
        story = GetStory(inkJson);
        if (!string.IsNullOrEmpty(entryKnot)) story.ChoosePathString(entryKnot);

        IsDialogueActive = true;
        input.DisableMove();
        interactor.BeginSession(this);
        DisplayLine();
    }

    private Story GetStory(TextAsset inkJson)
    {
        string key = inkJson.name;
        if (!stories.TryGetValue(key, out Story fnd))
        {
            fnd = new Story(inkJson.text);
            stories[key] = fnd;
        }
        return fnd;
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
                ShowLine(line);
                return;
            } 
        }

        if (story.currentChoices.Count > 0)
        {
            ShowChoices();
            return;
        }
        EndDialogue();
    }

    private void ShowChoices()
    {
        isChoosing = true;
        List<string> labels = new();
        foreach (Choice choice in story.currentChoices)
        {
            labels.Add(choice.text);
        }
        choices.Show(labels);
        input.EnableNavigate();
    }


    private void ChooseCurrent()
    {
        int picked = choices.Idx;
        isChoosing = false;
        choices.Hide();
        input.DisableNavigate();

        story.ChooseChoiceIndex(picked); //ink lanjut dari cabang yg dipilih
        DisplayLine();
    }

    //nentuin korak mana yg dipake, tutup kotak lama klo pembicara ganti
    private void ShowLine(DialogueLine line)
    {
        IDialogueView view = ResolveView(line);
        if (activeView != null && activeView != view) activeView.Hide();

        activeView = view;
        activeSpeaker = line.Speaker;
        currLine = line.Text;
        typCoroutine = StartCoroutine(Typing(currLine));

    }

    private IDialogueView ResolveView(DialogueLine line)
    {
        if (line.Mode == DialogueMode.Narration) return narration;
        DialogueSpeaker sp = FindSpeaker(line.Speaker);
        if (sp == null)
        {
            Debug.LogWarning($"[Dialogue] speaker '{line.Speaker}' ga ketemu, lempar ke narasi");
            return narration;
        }
        return sp.Bubble;
    }

    private DialogueSpeaker FindSpeaker(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;

        string key = id.ToLower();
        if (!speakers.TryGetValue(key, out DialogueSpeaker sp)) return null;
        if (sp == null) //npc dah mati bareng scene lama
        {
            speakers.Remove(key);
            return null;
        }
        return sp;
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
            activeView.Show(activeSpeaker, shown);
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    private void InstantFinishTyp()
    {
        if (typCoroutine != null) StopCoroutine(typCoroutine); 
        activeView.Show(activeSpeaker, currLine);
        isTyping = false;
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;
        activeView?.Hide();
        activeView = null;
        activeSpeaker = null;
        interactor.EndSession(this);
        input.EnableMove();
    }

}
