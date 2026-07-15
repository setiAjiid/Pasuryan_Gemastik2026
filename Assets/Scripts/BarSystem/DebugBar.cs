using UnityEngine;

public class DebugBar : MonoBehaviour
{
    //main task: mancing perubahan bar pake keyboard buat ngetes, will be delete klo dah aman

    [SerializeField] private BatinState batin;

    private void OnEnable()
    {
        batin.OnChangedBar += ShowStat;
        batin.OnTensiMax += ShowExploded;
    }

    private void OnDisable()
    {
        batin.OnChangedBar -= ShowStat;
        batin.OnTensiMax -= ShowExploded;
    }

    private void ShowStat(BatinStat stat, int value)
    {
        Debug.Log($"[Batin] {stat} = {value}");
    }

    private void ShowExploded()
    {
        Debug.LogWarning("[Batin] Tension meledek!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) batin.CalcStat(0, 0, +10); // Tension naik
        if (Input.GetKeyDown(KeyCode.Alpha2)) batin.CalcStat(0, 0, -10); // Tension turun
        if (Input.GetKeyDown(KeyCode.Alpha3)) batin.CalcStat(0, +10, 0); // Energy naik
        if (Input.GetKeyDown(KeyCode.Alpha4)) batin.CalcStat(0, -10, 0); // Energy turun
        if (Input.GetKeyDown(KeyCode.Alpha5)) batin.CalcStat(+10, 0, 0); // MH naik
        if (Input.GetKeyDown(KeyCode.Alpha6)) batin.CalcStat(-10, 0, 0); // MH turun
    }
}