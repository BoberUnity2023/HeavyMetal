using DG.Tweening;
using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform[] _carPositions;
    private Car[] _cars = new Car[4];

    public Car[] Cars => _cars;        

    private void Start()
    {
        //CreateCars();
    }

    private void CreateCars()
    {
        GameController _game = _sceneController.Game;
        for (int i = 0; i < _game.CarPropses.Length; i++)
        {
            Car prefab = _game.CarPropses[i].Prefab;
            Transform parent = _carPositions[i];
            _cars[i] = Instantiate(prefab, parent.position, parent.rotation, parent);
        }
    }

    public void ShowCar(int id)
    {
        _camera.transform.LookAt(_carPositions[id]);
    }
}
