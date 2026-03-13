using UnityEngine;

[CreateAssetMenu(fileName = "ConfigControl", menuName = "Configs/ConfigControl")]
public class ConfigControl : ScriptableObject
{
    [SerializeField] private ControlType _type;
    [SerializeField] private LayerMask _mask;

    public ControlType Type => _type;

    public LayerMask Mask => _mask;    
}
