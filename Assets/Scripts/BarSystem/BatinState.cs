using UnityEngine;
using System;

public enum BatinStat
{
    MH, 
    Energy, 
    Tensi
}

public class BatinState : MonoBehaviour
{
    //main task: jadi db ttg bar sistem + ngitung/logic sistem pengurangan/penambahan bar sistem

    [SerializeField] private int maxVal = 100;
    [SerializeField] private int mhVal = 100;
    [SerializeField] private int energyVal= 100;
    [SerializeField] private int tensiVal = 0;

    private bool tensiExploded = false;

    public int MHVal => mhVal;
    public int EnergyVal => energyVal;
    public int TensiVal => tensiVal;

    public event Action<BatinStat, int> OnChangedBar;
    public event Action OnTensiMax;
    
    public int GetStat(BatinStat stat)
    {
        return stat switch
        {
            BatinStat.MH => mhVal,
            BatinStat.Energy => energyVal,
            BatinStat.Tensi => tensiVal,
            _ => 0,
        };
    }

    public void CalcStat(int mhDelta, int energyDelta, int tensiDelta)
    {
        if (mhDelta != 0) SetStat(BatinStat.MH, mhVal + mhDelta);
        if (energyDelta != 0) SetStat(BatinStat.Energy, energyVal + energyDelta);
        if (tensiDelta != 0) SetStat(BatinStat.Tensi, tensiVal + tensiDelta);
    }

    private void SetStat(BatinStat stat, int newVal)
    {
        newVal = Mathf.Clamp(newVal, 0, maxVal);
        switch (stat)
        {
            case BatinStat.MH:
                mhVal = newVal;
                break;
            case BatinStat.Energy:
                energyVal = newVal; 
                break;
            case BatinStat.Tensi:
                tensiVal = newVal;
                break;
        }
        OnChangedBar?.Invoke(stat, newVal);
        if (stat == BatinStat.Tensi) TensiCheck();
    }

    private void TensiCheck()
    {
        if (tensiVal >= maxVal && !tensiExploded)
        {
            tensiExploded = true;
            OnTensiMax?.Invoke();
        } 
    }

    public void ResetTensiExploded()
    {
        tensiExploded = false;
    }
}
