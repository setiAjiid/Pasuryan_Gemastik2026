using UnityEngine;

[CreateAssetMenu(fileName = "Mask_", menuName = "Pasuryan/MaskDefinition")]
public class MaskDefinition : ScriptableObject
{
    //main task: data statis satu topeng
    [SerializeField] private MaskType type;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int energyCost = 10;
    [TextArea][SerializeField] private string note; //catatan sendiri, tanpa llogic

    public MaskType Type => type;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public int EnergyCost => energyCost;

    //id yg dipk di ink, misal "* [tenang]"
    public string InkId => type.ToString().ToLower();
}
