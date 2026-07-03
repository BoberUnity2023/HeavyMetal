using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private Car[] _cars = new Car[4];
    [SerializeField] private WindowSelectCar _windowSelectCar;
    [SerializeField] private Transform _carPosition;
    private Car _currentCar;

    public SceneController SceneController => _sceneController;
    public WindowSelectCar WindowSelectCar => _windowSelectCar;

    public Car CurrentCar => _currentCar;    

    private void Start()
    {
        for (int i = 0; i < _sceneController.Game.ConfigGame.Cars.Length; i++)
        {
            Car carPrefab = _sceneController.Game.ConfigGame.Cars[i].Prefab;
            _cars[i] = Instantiate(carPrefab, _carPosition.position, _carPosition.rotation, _carPosition);
            _cars[i].Init(Mode.Garage, _sceneController.Game);
            bool isSeleced = i == _sceneController.Game.SelectedCar;
            _cars[i].gameObject.SetActive(isSeleced);
            if (isSeleced)
            {                
                _currentCar = _cars[_sceneController.Game.SelectedCar];
            }
            //ShowCar(_sceneController.Game.SelectedCar);
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
