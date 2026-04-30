using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private Car[] _cars = new Car[4];
    [SerializeField] private WindowSelectCar _windowSelectCar;
    private Car _currentCar;

    public SceneController SceneController => _sceneController;
    public WindowSelectCar WindowSelectCar => _windowSelectCar;

    public Car CurrentCar => _currentCar;

    private void Awake()
    {  
        foreach (Car car in _cars)
        {
            car.enabled = false;            
        }
    }

    private void Start()
    {
        ShowCar(_sceneController.Game.SelectedCar);

        foreach (Car car in _cars)
        {
            car.Tuning.Init(car, _sceneController.Game);
        }
    }

    public void ShowCar(int id)
    {  
        for (int i = 0; i < _cars.Length; i++)
        {
            _cars[i].gameObject.SetActive(i == id);
            if (i == id)
            {                
                _currentCar = _cars[i];
                SetCarColor(i);
            }
        }
    }

    private void SetCarColor(int number)
    {
        int id = _sceneController.Game.Saves.GetCarColor(_cars[number].CarType);
        Material material = _sceneController.Game.ConfigGame.Cars[number].Tuning.CarColors[id].Material;
        _cars[number].Paint.SetMaterial(material);        
    }
    
}
