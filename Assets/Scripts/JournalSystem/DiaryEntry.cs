using UnityEngine;

public enum Sentiment
{
    None,
    Redam,
    Pahami
}

[CreateAssetMenu(fileName = "Diary_", menuName = "Pasuryan/DiaryEntry")]
public class DiaryEntry : ScriptableObject
{
    //main taks: definisi satu hari di buku, judul di ddaftar, n naskah yg dipake buat nulis
    [SerializeField] private int day = 1;
    [SerializeField] private string title;
    [SerializeField] private TextAsset inkJson;
    [SerializeField] private string entryKnot = "day1";

    public int Day => day;
    public string Title => title;
    public TextAsset InkJson => inkJson;
    public string EntryKnot => entryKnot;   

}
