using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BackpackMock : MonoBehaviour
{
    //main task: cangkang menu 2 lapis

    [System.Serializable]
    private class IconSwap
    {
        public Image tar;
        public Sprite off;
        public Sprite on;
    }

    [SerializeField] private PlayerInputReader input;
    [SerializeField] private GameObject root; //induk 
    [SerializeField] private GameObject[] panels; //isi per bagian

    [SerializeField] private Image[] topIcons; //3 icon main, which is jagat lakon, items, n buku
    [SerializeField] private IconSwap[] tabIcons; //4 icon second, ykni diary, observ, koleksi, n agenda
    
    [SerializeField] private int bookIdx = 2; //icon buku slot
    [SerializeField] private float dimAlpha = 0.4f; 

    [Header("Panah")]
    [SerializeField] private GameObject arrowL;
    [SerializeField] private GameObject arrowR;
 
    private bool isOpen;
    private int topIdx;
    private int tabIdx;

    private void Start()
    {
        root.SetActive(false);
    }

    private void Update()
    {
        Keyboard key = Keyboard.current;
        if (key == null) return;
        if (key.tabKey.wasPressedThisFrame) Toggle();
        if (!isOpen) return;
        if (key.dKey.wasPressedThisFrame || key.rightArrowKey.wasPressedThisFrame) MoveTop(1);
        if (key.aKey.wasPressedThisFrame || key.leftArrowKey.wasPressedThisFrame) MoveTop(-1);
        if (key.sKey.wasPressedThisFrame || key.downArrowKey.wasPressedThisFrame) MoveTab(1);
        if (key.wKey.wasPressedThisFrame || key.upArrowKey.wasPressedThisFrame) MoveTab(-1);
        if (key.escapeKey.wasPressedThisFrame) Toggle();
    }

    private void Toggle()
    {
        isOpen = !isOpen;
        root.SetActive(isOpen);
        if (isOpen)
        {
            input.DisableMove();
            Repaint();
        } else
        {
            input.EnableMove();
        }
    }

    private void MoveTop(int dir)
    {
        topIdx = Mathf.Clamp(topIdx + dir, 0, panels.Length - 1);
        Repaint();
    }

    private void MoveTab(int dir)
    {
        if (topIdx != bookIdx) return;
        tabIdx = Mathf.Clamp(tabIdx + dir, 0, tabIcons.Length - 1);
        Repaint();
    }

    private void Repaint()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            SetOn(panels[i], i == topIdx);
        }

        for (int i = 0; i < topIcons.Length; i++)
        {
            if (topIcons[i] == null) continue;
            Color c = topIcons[i].color;
            c.a = (i == topIdx) ? 1f : dimAlpha;
            topIcons[i].color = c;
        }

        for (int i = 0; i < tabIcons.Length; i++)
        {
            if (tabIcons[i] == null || tabIcons[i].tar == null) continue;
            tabIcons[i].tar.sprite = (i == tabIdx) ? tabIcons[i].on : tabIcons[i].off;
        }
        SetOn(arrowL, topIdx > 0);
        SetOn(arrowR, topIdx < panels.Length - 1);
    }

    private static void SetOn(GameObject go, bool on)
    {
        if (go != null) go.SetActive(on);
    }






}



