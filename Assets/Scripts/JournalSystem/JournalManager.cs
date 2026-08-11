using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Ink.Runtime;

public enum JournalPhase
{
    Idle, 
    Opening,
    Index, //milih hari di hal kiri
    Writing, //naskah lagi nulis by system
    Picking, //milih kartu sentimen
    Reading, //just read ulang hari yg dah tuntas
    Done,
    Closing
}

public class JournalManager : MonoBehaviour, IInteractionSession
{
    //main taks: ngatur sesi journaling malam, antrian fragmen, n konsekuens tiap pilihan
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private PlayerInteractor interactor;
    [SerializeField] private JournalLog log;
    [SerializeField] private BatinState batin;
    [SerializeField] private DiaryBookUI book;
    [SerializeField] private float typeSpeed = 0.04f;

    [Header("Animasi Buku")]
    [SerializeField] private Animator bookAnim;
    [SerializeField] private float openDur = 0f;
    [SerializeField] private float closeDur = 0f;

    private readonly StringBuilder buffer = new(); //sluruh entri hari iini
    private readonly List<string> cardIds = new(); //redam/pahami per card

    private JournalPhase phase = JournalPhase.Idle;
    private DiaryEntry currDay;
    private Story story;
    private Coroutine typCoroutine;
    private bool isTyping;

    public bool CanOpen => log.HasFlags;
    public bool IsOpen => phase != JournalPhase.Idle;

    private void Start()
    {
        if (input == null) Debug.LogError("[Journal] slot input kosong wok", this);
        if (interactor == null) Debug.LogError("[Journal] slot interactor kosong wok", this);
        if (batin == null) Debug.LogError("[Journal] slot batin kosong wok", this);
        if (log == null) Debug.LogError("[Journal] slot log kosong wok", this);
        if (book == null) Debug.LogError("[Journal] slot book kosong wok", this);
    }

    //dicall JournalInteractable.Interact()
    public void OpenJournal()
    {
        Debug.Log("Jurnal dah kebuka cuy");
        if (IsOpen) return;
        input.DisableMove();
        interactor.BeginSession(this); //daftarin skrng so E slama animasi g bocor
        StartCoroutine(OpenRoutine());
    }
    public void OnContinue()
    {
        switch (phase)
        {
            case JournalPhase.Index:
                OpenDay();
                break;
            case JournalPhase.Writing:
                if (isTyping) InstantFinish();
                else NextLine();
                break;
            case JournalPhase.Picking:
                ConfirmCard();
                break;
            case JournalPhase.Reading:
                phase = JournalPhase.Index;
                book.ShowIndex(log.AllEntries);
                break;
            case JournalPhase.Done:
                StartCoroutine(CloseRoutine());
                break;
        }
    }

    public void OnNavigate(int dir)
    {
        switch (phase)
        {
            case JournalPhase.Index:
                book.MoveDay(dir);
                break;
            case JournalPhase.Picking:
                book.MoveCard(dir);
                break;
            case JournalPhase.Writing:
            case JournalPhase.Reading:
            case JournalPhase.Done:
                book.MoveSpread(dir); //balik baca hal prev
                break;
        }
    }

    private IEnumerator OpenRoutine()
    {
        phase = JournalPhase.Opening;
        book.ShowShell();
        if (bookAnim != null) bookAnim.SetTrigger("Open");
        yield return new WaitForSeconds(openDur);

        book.ShowIndex(log.AllEntries);
        phase = JournalPhase.Index;
        input.EnableNavigate();
    }

    private void OpenDay()
    {
        currDay = book.PickedDay;
        if (currDay == null) return;

        buffer.Clear();
        book.ShowEntry(currDay.Title);

        //hari yg dah tuntas cukup baca
        if (log.IsWritten(currDay.Day))
        {
            buffer.Append(log.GetWritten(currDay.Day));
            book.SetText(buffer.ToString());
            book.SetVisible(book.CharCount);
            phase = JournalPhase.Reading;
            return;
        }

        story = new Story(currDay.InkJson.text);
        story.ChoosePathString(currDay.EntryKnot);
        PushFlags();
        phase = JournalPhase.Writing;
        NextLine();
    }

    //cara ink malam tau apa kejadian sebelum2nya
    private void PushFlags()
    {
        foreach(string flag in log.Flags)
        {
            if (story.variablesState[flag] == null)
            {
                Debug.LogWarning($"[Journal] VAR '{flag}' blom dideklrasiin di naskah diary");
                continue;
            }
            story.variablesState[flag] = true;
        }
    }

    private void NextLine()
    {
        while (story.canContinue)
        {
            string text = story.Continue().Trim();
            DialogueLine line = TagParser.Parse(text, story.currentTags);
            ApplyTags(line);

            if (!line.IsTagOnly)
            {
                WriteLine(line.Text);
                return;
            }
        }

        if (story.currentChoices.Count > 0)
        {
            ShowCards();
            return;
        }
        FinishEntry();
    }

    private void WriteLine(string text)
    {
        int from = book.CharCount; //diitung pk satuan tmp
        if (buffer.Length > 0) buffer.Append("\n\n");
        buffer.Append(text);
        book.SetText(buffer.ToString()); //teks dipasang penuh sekaligus biar paginasi g goyang
        phase = JournalPhase.Writing;
        typCoroutine = StartCoroutine(AutoWrite(from, book.CharCount));
    }

    private IEnumerator AutoWrite(int from, int to)
    {
        isTyping = true;
        for (int i = from; i <= to; i++)
        {
            book.SetVisible(i);
            book.FocusChar(i);
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    private void InstantFinish()
    {
        if (typCoroutine != null) StopCoroutine(typCoroutine);
        book.SetVisible(book.CharCount);
        book.FocusChar(book.CharCount);
        isTyping = false;
    }

    //teks pilihan ink bentuknya "id: kalimat konteks"
    private void ShowCards()
    {
        cardIds.Clear();
        List<string> labels = new();
        List<string> contexts = new();

        foreach(Choice choice in story.currentChoices)
        {
            string raw = choice.text.Trim();
            int sep = raw.IndexOf(':');
            string id = (sep < 0 ? raw : raw[..sep]).Trim().ToLower();

            cardIds.Add(id);
            labels.Add(id.ToUpper());
            contexts.Add(sep < 0 ? "" : raw[(sep + 1)..].Trim());
        }

        phase = JournalPhase.Picking;
        book.ShowCards(labels, contexts);
    }

    private void ConfirmCard()
    {
        int at = book.CardIdx;
        log.AddPick(cardIds[at] == "redam" ? Sentiment.Redam : Sentiment.Pahami);

        book.HideCards();
        phase = JournalPhase.Writing;
        story.ChooseChoiceIndex(at);
        NextLine();
    }

    private void ApplyTags(DialogueLine line)
    {
        if (line.HasResource) batin.CalcStat(line.MHDelta, line.EnergyDelta, line.TensiDelta);
    }

    private void FinishEntry()
    {
        log.SetWritten(currDay.Day, buffer.ToString()); //disimpen jd teks jadi, bukan diputar ulang
        phase = JournalPhase.Done;
        book.ShowDonePrompt();
    }

    private IEnumerator CloseRoutine()
    {
        phase = JournalPhase.Closing;
        if (bookAnim != null) bookAnim.SetTrigger("Close");
        yield return new WaitForSeconds(closeDur);

        book.HideAll();
        phase = JournalPhase.Idle;
        input.DisableNavigate();
        interactor.EndSession(this);
        input.EnableMove();
        Debug.Log($"[Journal] dominan = {log.Dominance()}"); //langkah D: transisi tidur
    }
}
