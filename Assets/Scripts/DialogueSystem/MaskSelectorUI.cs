using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaskSelectorUI : MonoBehaviour
{
    //main task: ngasih daftar pilihan horizon + nandai yg kepilih (without seeing isinya pilihan dialog/topeng)
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI[] slots;
    [SerializeField] private Color idleColor = Color.gray;
    [SerializeField] private Color pickedColor = Color.white;

    private int cnt;
    private int idx;

    public int Idx => idx;
    private void Awake()
    {
        Hide();
    }

    public void Show(List<string> labels)
    {
        if (labels.Count > slots.Length)
        {
            Debug.LogWarning($"[Choice] pilihan ada {labels.Count} tp slot cuman {slots.Length}, sisanya ga kelihatan", this);
        }

        cnt = Mathf.Min(labels.Count, slots.Length);
        for (int i =  0; i < slots.Length; i++)
        {
            bool used = i < cnt;
            slots[i].gameObject.SetActive(used);
            if (used) slots[i].text = labels[i];
        }
        idx = 0;
        panel.SetActive(true);
        Repaint();
    }

    public void MoveIndex(int dir)
    {
        if (cnt == 0) return;
        idx = Mathf.Clamp(idx + dir, 0, cnt - 1); //mentok di ujung, g muter
        Repaint();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void Repaint()
    {
        for (int i = 0; i < cnt; i++)
        {
            slots[i].color = (i == idx) ? pickedColor : idleColor;
        }
    }

}
