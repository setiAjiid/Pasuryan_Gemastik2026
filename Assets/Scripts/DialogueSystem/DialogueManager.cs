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

    private readonly Dictionary<string, DialogueSpeaker> speakers = new();
    private Story story;
    private Coroutine typCoroutine;
    private IDialogueView activeView;
    private string activeSpeaker;
    private string currLine;
    private bool isTyping = false;

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
        if (isTyping) InstantFinishTyp();
        else DisplayLine();
    }

    //case baru mulai dialog, dicall dr DialogueTrigger.Interact()
    public void StartDialogue(TextAsset inkJson)
    {
        if (IsDialogueActive) return;
        story = new Story(inkJson.text);
        IsDialogueActive = true;
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
                ShowLine(line);
                return;
            } 
        }
        EndDialogue();
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
