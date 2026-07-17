using UnityEngine;
using UnityEngine.UI;

public class UISegmentedBar : MonoBehaviour
{
    //main task: update visual enegy UI bar yg segmented atas perubahan dari BatinState class
    [SerializeField] private BatinStat statType; //per bar dikasih identifier
    [SerializeField] private BatinState batin;
    [SerializeField] private Image[] fillImg;
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
        //just nyocokin antara type stat yg mo diubah dengan tipe stat asli dari suatu bar batin 
        if (changedStat == statType)
        {
            UpdateVisualBar(newVal); 
        }
    }


    private void UpdateVisualBar(int curVal)
    {
        if (fillImg == null || fillImg.Length == 0)
        {
            Debug.LogWarning("fill image kau ga ada");
            return;
        } else
        {
            float totalFill= (float)curVal / batin.MaxVal * fillImg.Length;
            for (int i = 0; i <  fillImg.Length; i++)
            {
                if (fillImg[i] == null) continue;
                fillImg[i].fillAmount = Mathf.Clamp01(totalFill- i);
            }
        }
    }

}
