using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Configs/ConfigEnemy")]

public class ConfigEnemy : ScriptableObject
{
    [SerializeField] ConfigCar _car;
    
    [SerializeField] private int _tuningEngine;
    [SerializeField] private int _tuningShields;
    [SerializeField] private int _tuningTires;
    [SerializeField] private int _tuningWeapon;
    [SerializeField] private int _tuningNitro;
    [SerializeField] private int _tuningShield;
    [SerializeField] private int _tuningColor;

    public ConfigCar Car => _car;    

    public int TuningEngine => _tuningEngine;

    public int TuningShields => _tuningShields;

    public int TuningTires => _tuningTires;

    public int TuningWeapon => _tuningWeapon;

    public int TuningNitro => _tuningNitro;

    public int TuningShield => _tuningShield;

    public int TuningColor => _tuningColor;
}
