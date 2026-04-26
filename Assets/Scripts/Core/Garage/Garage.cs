using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private GameObject[] _cars = new GameObject[4];
    [SerializeField] private WindowSelectCar _windowSelectCar;
    [SerializeField] private WindowTuning _windowTuning;

    public WindowSelectCar WindowSelectCar => _windowSelectCar;
    public WindowTuning WindowTuning => _windowTuning;

    private void Start()
    {
        ShowCar(_sceneController.Game.SelectedCar);
    }

    public void ShowCar(int id)
    {  
        for (int i = 0; i < _cars.Length; i++)
        {
            _cars[i].SetActive(i == id);
        }
    }
}
