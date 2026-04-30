using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "Configs/ConfigGame")]

public class ConfigGame : ScriptableObject
{
    [SerializeField] private Platform _platform;
    [SerializeField] private GameLanguage _language;
    [SerializeField] private int _startCoins;
    [SerializeField] private ConfigCar[] _cars;

    public Platform Platform => _platform;

    public GameLanguage Language => _language;

    public int StartCoins => _startCoins;

    public ConfigCar[] Cars => _cars;  
    
    public ConfigCar Car(CarType carType)
    {
        foreach(ConfigCar car in _cars)
        {
            if (car.CarType == carType)
                return car;
        }

        Debug.LogError("Config " + carType.ToString() + " was not founded!");
        return null;
    }    
}
