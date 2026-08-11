using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DiaryBookUI : MonoBehaviour
{
    //main task: gambarin buku diary, spread indeks + spread entri berpaginasi + kartu sentimen
    [Header("Cangkang")]
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject content;

    [Header("Spread Indeks")]
    [SerializeField] private GameObject indexSpread;
    [SerializeField] private TextMeshProUGUI dayList;

    [Header("Spread Entri")]
    [SerializeField] private GameObject entrySpread;
    [SerializeField] private TextMeshProUGUI pageLeft;
    [SerializeField] private TextMeshProUGUI pageRight;
    [SerializeField] private GameObject arrowPrev;
    [SerializeField] private GameObject arrowNext;
    [SerializeField] private TextMeshProUGUI entryTitle;

    [Header("Kartu sentimen")]
    [SerializeField] private GameObject cardRow;
    [SerializeField] private SentimentCardUI[] cards;

    [Header("Prompt")]
    [SerializeField] private GameObject donePrompt;

    private readonly List<DiaryEntry> days = new();
    private int dayIdx;
    private int cardIdx;
    private int cardCnt;
    private int spread;

    public DiaryEntry PickedDay => days.Count > 0 ? days[dayIdx] : null;
    public int CardIdx => cardIdx;
    public int CharCount => pageLeft.textInfo.characterCount;


    private void Awake()
    {
        HideAll();
    }

    public void HideAll()
    {
        root.SetActive(false);
    }

    public void ShowShell()
    {
        root.SetActive(true);
        content.SetActive(false); //teks blom boleh ke baca msih setengah ke buka
    }


    public void ShowIndex(IReadOnlyList<DiaryEntry> entries)
    {
        days.Clear();
        foreach(DiaryEntry entry in entries)
        {
            if (entry != null) days.Add(entry);
        }
        dayIdx = 0;

        content.SetActive(true);
        indexSpread.SetActive(true);
        entrySpread.SetActive(false);
        HideCards();
        donePrompt.SetActive(false);
        RepaintDayList();
    }

    public void MoveDay(int dir)
    {
        if (days.Count == 0) return;
        dayIdx = Mathf.Clamp(dayIdx + dir, 0, days.Count - 1); //mentok di ujung g muter
        RepaintDayList();
    }


    public void ShowEntry(string title)
    {
        entryTitle.text = title;
        indexSpread.SetActive(false);
        entrySpread.SetActive(true);
        spread = 0;
        SetText("");
    }

    //dua tmp diisi string yg sama, bedanya cuma halaman ke berapa yg ditampilin
    public void SetText(string text)
    {
        pageLeft.text = text;
        pageRight.text = text;
        pageLeft.ForceMeshUpdate();
        pageRight.ForceMeshUpdate();
        RepaintSpread();
    }

    public void SetVisible(int count)
    {
        pageLeft.maxVisibleCharacters = count;
        pageRight.maxVisibleCharacters= count;
    }

    //lompas ke spread tepat huruf ke sekian (at) mendarat so tulisan yg jalan selalu kelihatan
    public void FocusChar(int at)
    {
        TMP_TextInfo info = pageLeft.textInfo;
        if (info.characterCount == 0) return;

        int i = Mathf.Clamp(at - 1, 0, info.characterCount - 1);
        int target = info.characterInfo[i].pageNumber / 2; //pageNumber mulai dr 0, satu spread 2 hal
        if (target == spread) return;
        spread = target;
        RepaintSpread();
        
    }

    public void MoveSpread(int dir)
    {
        int last = Mathf.Max(0, (pageLeft.textInfo.pageCount - 1) / 2);
        spread = Mathf.Clamp(spread + dir, 0, last);
        RepaintSpread();
    }

    public void ShowCards(List<string> labels, List<string> contexts)
    {
        cardCnt = Mathf.Min(labels.Count, cards.Length);
        for (int i = 0; i < cards.Length; i++)
        {
            if (i < cardCnt) cards[i].Bind(labels[i], contexts[i]);
            else cards[i].gameObject.SetActive(false);
        }
        cardIdx = 0;
        cardRow.SetActive(true);
        RepaintCards();
    }

    public void MoveCard(int dir)
    {
        if (cardCnt == 0) return;
        cardIdx = Mathf.Clamp(cardIdx + dir, 0, cardCnt - 1);
        RepaintCards();
    }

    public void HideCards()
    {
        cardRow.SetActive(false);
    }

    public void ShowDonePrompt()
    {
        donePrompt.SetActive(true);
    }

    private void RepaintSpread()
    {
        pageLeft.pageToDisplay = spread * 2 + 1; //pagetodisplay mulai dr 1
        pageRight.pageToDisplay = spread * 2 + 2;
        entryTitle.gameObject.SetActive(spread == 0); //tgl ditulis sekali di awal entri
        int total = pageLeft.textInfo.pageCount;
        arrowPrev.SetActive(spread > 0);
        arrowNext.SetActive((spread + 1) * 2 < total); //padam di ujung just like panah pilihan
    }


    private void RepaintCards()
    {
        for (int i = 0; i < cardCnt; i++)
        {
            cards[i].SetPicked(i == cardIdx);
        }
    }

    private void RepaintDayList()
    {
        StringBuilder sb = new();
        for (int i = 0; i < days.Count; i++)
        {
            if (i == dayIdx) sb.AppendLine($"<b>{days[i].Title}</b>");
            else sb.AppendLine($"<alpha=#60>{days[i].Title}<alpha=#FF>");
        }
        dayList.text = sb.ToString();
    }




}
