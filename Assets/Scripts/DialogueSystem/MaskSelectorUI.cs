using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaskSelectorUI : MonoBehaviour
{
    //main task:
    //tampilin 3 topeng aktif + tandai yg kepilih, yg ga bisa dipake ttp kerender cuman beku
    [SerializeField] private GameObject panel;
    [SerializeField] private MaskSlotUI[] slots;

    private readonly List<bool> selectable = new();
    private int cnt;
    private int idx;

    public int Idx => idx;
    public bool IsPickable => cnt > 0 && selectable[idx];

    private void Awake()
    {
        Hide();
    }

    public void Show(List<MaskDefinition> defs, List<bool> canPick)
    {
        if (defs.Count > slots.Length)
        {
            Debug.LogWarning($"[Mask] topeng ada {defs.Count} tp slot cuman {slots.Length}, sisanya ga kelihatan", this);
        }
        cnt = Mathf.Min(defs.Count, slots.Length);

        selectable.Clear();
        selectable.AddRange(canPick);
        for (int i =  0; i < slots.Length; i++)
        {
            if (i < cnt) slots[i].Bind(defs[i], canPick[i]);
            else slots[i].gameObject.SetActive(false);
        }
        idx = FirstSelectable();
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

    //mulai dari topeng pertama yg beneran kepake aka ga beku
    private int FirstSelectable()
    {
        for(int i = 0; i < cnt; i++)
        {
            if (selectable[i]) return i;
        }
        return 0;
    }

    private void Repaint()
    {
        for (int i = 0; i < cnt; i++)
        {
            slots[i].SetPicked(i == idx);
        }
    }

}
