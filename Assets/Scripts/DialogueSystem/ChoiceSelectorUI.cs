using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceSelectorUI : MonoBehaviour
{
    //main task: ngasih daftar pilihan horizon + nandai yg kepilih
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject arrowLeft;
    [SerializeField] private GameObject arrowRight;

    private readonly List<string> options = new();
    private int idx;

    public int Idx => idx;
    private void Awake()
    {
        Hide();
    }

    public void Show(List<string> labels)
    {
        options.Clear();
        options.AddRange(labels);
        idx = (options.Count - 1) / 2;
        panel.SetActive(true);
        Repaint();
    }

    public void MoveIndex(int dir)
    {
        if (options.Count == 0) return;
        idx = Mathf.Clamp(idx + dir, 0, options.Count - 1); //mentok di ujung, g muter
        Repaint();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void Repaint() //panah nyala klo masih ada pilihan di arah itu
    {
        label.text = options[idx];
        arrowLeft.SetActive(idx > 0);
        arrowRight.SetActive(idx < options.Count - 1);
    }

}
