using UnityEngine;
using UnityEngine.UI;

public class UIStatBar : MonoBehaviour
{
    //main task: update visual UI bar atas perubahan dari BatinState class
    [SerializeField] private BatinStat statType;
    [SerializeField] private BatinState batin;
    [SerializeField] private Image fillImg;
    private void OnEnable()
    {
        if (batin != null)
        {
            batin.OnChangedBar += HandleVisualBar;
            UpdateVisualBar(batin.GetStat(statType)); //init gamebar
        }
    }

    private void OnDisable()
    {
        if (batin != null)
        {
            batin.OnChangedBar -= HandleVisualBar;
        }
    }


    private void HandleVisualBar(BatinStat changedStat, int newVal)
    {
        //just nyocokin antara type stat yg mo diubah dengan tipe stat asli dari batin 
        if (changedStat == statType)
        {
            UpdateVisualBar(newVal); 
        }
    }


    private void UpdateVisualBar(int curVal)
    {
        if (fillImg == null)
        {
            Debug.LogWarning("fill image kau ga ada");
        } else
        {
            float tarFill = (float)curVal / batin.MaxVal;
            fillImg.fillAmount = tarFill;
        }
    }

}
