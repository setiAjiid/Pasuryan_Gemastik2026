using System.Collections.Generic;
using UnityEngine;

public class JournalLog : MonoBehaviour
{
    //main task: nampung kejadian hari ini + hasil
    [SerializeField] private DiaryEntry[] allEntries;
    [SerializeField] private string[] seedFlags; //for test, nanti diisi tag " fragmen: " pas siang

    private readonly HashSet<string> flags = new();
    private readonly Dictionary<int, string> written = new(); //hari yg dah tuntas ke simpen jadi teks

    private int redamCnt;
    private int pahamiCnt;

    public IReadOnlyList<DiaryEntry> AllEntries => allEntries;
    public IReadOnlyCollection<string> Flags => flags;
    public bool HasFlags => flags.Count > 0;


    private void Awake()
    {
        if (allEntries == null || allEntries.Length == 0)
        {
            Debug.LogError("[JournalLog] slot allEntries kosong wok", this);
        }

        foreach(string flag in seedFlags)
        {
            AddFlag(flag);
        }
            
    }

    //dicall DialogueManager pas nemu tag #fragmen: id
    public void AddFlag(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        flags.Add(id.ToLower());
    }

    public DiaryEntry GetEntry(int day)
    {
        foreach(DiaryEntry entry in allEntries)
        {
            if (entry != null && entry.Day == day) return entry;
        }
        return null;
    }
    
    //hari yg dah ditulis g boleh dijalanin ulang, cukup dibaca lagi
    public bool IsWritten(int day)
    {
        return written.ContainsKey(day);
    }

    public string GetWritten(int day)
    {
        return written.TryGetValue(day, out string text) ? text : "";
    }

    public void SetWritten(int day, string text)
    {
        written[day] = text;

    }

    public void AddPick(Sentiment pick)
    {
        if (pick == Sentiment.Redam) redamCnt++;
        else if (pick == Sentiment.Pahami) pahamiCnt++;
    }

    //buat narasi tidur, + condong pahami, - condong redam, 0 seimbang
    public int Dominance()
    {
        return pahamiCnt - redamCnt;
    }

    public void ClearDay()
    {
        flags.Clear();
        redamCnt = 0;
        pahamiCnt = 0;
    }

}
