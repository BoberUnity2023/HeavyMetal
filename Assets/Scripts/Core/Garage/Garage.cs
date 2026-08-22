using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private WindowSelectCar _windowSelectCar;
    [SerializeField] private WindowSettings _windowSettings;
    [SerializeField] private WindowBase _windowAboutGame;
    [SerializeField] private CameraMovier _cameraMovier;
    [SerializeField] private Transform _carPosition;
    
    private Car[] _cars;
    private Car _currentCar;

    public SceneController SceneController => _sceneController;
    public MainMenu MainMenu => _mainMenu;
    public WindowSelectCar WindowSelectCar => _windowSelectCar;
    public WindowSettings WindowSettings => _windowSettings;
    public WindowBase WindowAboutGame => _windowAboutGame;
    public Car CurrentCar => _currentCar;    
    public CameraMovier CameraMovier => _cameraMovier;    

    public void Init(GameController game, bool fromLevel)
    {
        _mainMenu.Init(game);
        _windowSelectCar.Init(game);
        _windowSettings.Init(game);
        _windowAboutGame.Init(game);

        if (fromLevel)
        {
            _mainMenu.Hide();
            _windowSelectCar.Show();
            CreateCars();
        }
        else
        {
            _windowSelectCar.Hide();
            _mainMenu.Show();
            _cameraMovier.SetToCups();
        }
    }

    public void PressGame()
    {
        CreateCars();        
        _cameraMovier.MoveToCar();
        Invoke("ShowWindowSelectCar", 1);
    }

    private void ShowWindowSelectCar()
    {
        WindowSelectCar.Show();
    }

    private void CreateCars()
    {
        int count = _sceneController.Game.ConfigGame.Cars.Length;
        _cars = new Car[count];

        for (int i = 0; i < count; i++)
        {
            Car carPrefab = _sceneController.Game.ConfigGame.Cars[i].Prefab;
            _cars[i] = Instantiate(carPrefab, _carPosition.position, _carPosition.rotation, _carPosition);
            _cars[i].Init(Mode.Garage, _sceneController.Game);
            bool isSeleced = i == _sceneController.Game.SelectedCar;
            _cars[i].gameObject.SetActive(isSeleced);
            if (isSeleced)
            {
                _currentCar = _cars[_sceneController.Game.SelectedCar];
                SetCarColor(i);
            }
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
